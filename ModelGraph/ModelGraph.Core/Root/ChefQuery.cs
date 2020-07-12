using System.Collections.Generic;
using System.Text;

namespace ModelGraph.Core
{
    public partial class Root
    {
        #region ValidateQueryXStore  ==========================================
        private void ValidateQueryXStore()
        {
            var ComputeXDomain = Get<ComputeXRoot>();
            var GraphXDomain = Get<GraphXRoot>();

            foreach (var cx in ComputeXDomain.Items)
            {
                ValidateComputeQuery(cx);
            }
            RevalidateUnresolved();
            foreach (var gx in GraphXDomain.Items)
            {
                ValidateGraphQuery(gx);
            }
        }

        private void ValidateQueryDependants(QueryX qx)
        {
            var QueryX_QueryX = Get<Relation_QueryX_QueryX>();
            var ComputeX_QueryX = Get<Relation_ComputeX_QueryX>();
            var GraphX_QueryX = Get<Relation_GraphX_QueryX>();
            var SymbolX_QueryX = Get<Relation_SymbolX_QueryX>();
            var QueryXDomain = Get<QueryXRoot>();

            while (QueryX_QueryX.TryGetParent(qx, out QueryX qp)) { qx = qp; }

            if (ComputeX_QueryX.TryGetParent(qx, out ComputeX cx))
                ValidateComputeQuery(cx, true);
            else if (GraphX_QueryX.TryGetParent(qx, out GraphX gx))
            {
                ValidateGraphQuery(gx, true);
            }
            else if (SymbolX_QueryX.TryGetParent(qx, out SymbolX sx))
            {
                ValidateWhere(qx, qx, Get<Property_QueryX_Where>(), true);
            }
        }

        private void ValidateComputeQuery(ComputeX cx, bool clearError = false)
        {
            cx.Value.Clear();
            cx.Value = ValuesUnknown;
            if (clearError)
            {
                ClearError(cx);
                ClearError(cx, Get<Property_ComputeX_Select>());
            }
            cx.ModelDelta++;
            if (!Get<Relation_ComputeX_QueryX>().TryGetChild(cx, out QueryX qx))
            {
                cx.Value = ValuesInvalid;
                TryAddErrorNone(cx, IdKey.ComputeMissingRootQueryError);
            }
            else
            {
                if (qx.Select == null)
                {
                    if (cx.CompuType == CompuType.RowValue)
                    {
                        TryAddErrorNone(cx, Get<Property_ComputeX_Select>(), IdKey.ComputeMissingSelectError);
                    }
                }
                else
                {
                    ValidateSelect(qx, cx, Get<Property_ComputeX_Select>(), clearError);
                }
            }
            if (Get<Relation_QueryX_QueryX>().TryGetChildren(qx, out IList<QueryX> children))
                ValidateQueryHierarchy(children, clearError);
            AllocateValueCache(cx);
        }
        private void ValidateGraphQuery(GraphX gx, bool clearError = false)
        {
            if (clearError) ClearError(gx);

            if (Get <Relation_GraphX_QueryX>().TryGetChildren(gx, out IList<QueryX> pathQuery))
                ValidateQueryHierarchy(pathQuery, clearError);

            if (Get <Relation_GraphX_SymbolQueryX>().TryGetChildren(gx, out IList<QueryX> list))
            {
                ValidateQueryHierarchy(list, clearError);
            }
        }
        private void RevalidateUnresolved()
        {
            for (int i = 0; i < 10; i++)
            {
                var anyUnresolved = false;

                foreach (var cx in Get<ComputeXRoot>().Items)
                {
                    if (cx.Value.ValType == ValType.IsUnresolved)
                    {
                        anyUnresolved = true;
                        ValidateComputeQuery(cx);
                    }
                }
                if (!anyUnresolved) break;
            }
        }

        private void ValidateQueryHierarchy(IList<QueryX> rootChildren, bool clearError = false)
        {
            var queryQueue = new Queue<QueryX>();
            foreach (var qc in rootChildren) { queryQueue.Enqueue(qc); }
                
            while (queryQueue.Count > 0)
            {
                var qx = queryQueue.Dequeue();
                qx.ModelDelta++;
                if (clearError)
                {
                    ClearError(qx, Get<Property_QueryX_Where>());
                    ClearError(qx, Get<Property_QueryX_Select>());
                }

                if (Get<Relation_QueryX_QueryX>().TryGetChildren(qx, out IList<QueryX> children))
                {
                    qx.IsTail = false;
                    foreach (var qc in children) { queryQueue.Enqueue(qc); }
                }
                else
                    qx.IsTail = true;

                ValidateWhereSelect(qx);
            }

            void ValidateWhereSelect(QueryX qx)
            {
                var sto = GetQueryXTarget(qx);
                if (qx.Select != null)
                {
                    if (qx.Select.TryValidate(sto))
                    {
                        qx.Select.TryResolve();
                        if (qx.Select.AnyUnresolved)
                        {
                            var error = TryAddErrorMany(qx, Get<Property_QueryX_Select>(), IdKey.QueryUnresolvedSelectError);
                            if (error != null) qx.Select.GetTree(error.List);
                        }
                    }
                    else
                    {
                        var error = TryAddErrorMany(qx, Get<Property_QueryX_Select>(), IdKey.QueryInvalidSelectError);
                        if (error != null) qx.Select.GetTree(error.List);
                    }
                }
                if (qx.Where != null)
                {
                    if (qx.Where.TryValidate(sto))
                    {
                        qx.Where.TryResolve();
                        if (qx.Where.AnyUnresolved)
                        {
                            var error = TryAddErrorMany(qx, Get<Property_QueryX_Where>(), IdKey.QueryUnresolvedWhereError);
                            if (error != null) qx.Where.GetTree(error.List);
                        }
                    }
                    else
                    {
                        var error = TryAddErrorMany(qx, Get<Property_QueryX_Where>(), IdKey.QueryInvalidWhereError);
                        if (error != null) qx.Where.GetTree(error.List);
                    }
                }
            }
        }
        bool ValidateWhere(QueryX qx, Item item, Property prop, bool clearError)
        {
            if (clearError) ClearError(item, prop);

            var sto = GetQueryXTarget(qx);
            if (qx.Where != null)
            {
                qx.ModelDelta++;
                if (qx.Where.TryValidate(sto))
                {
                    qx.Where.TryResolve();
                    if (qx.Where.AnyUnresolved)
                    {
                        var error = TryAddErrorMany(item, prop, IdKey.QueryUnresolvedWhereError);
                        if (error != null) qx.Where.GetTree(error.List);
                        return false;
                    }
                }
                else
                {
                    var error = TryAddErrorMany(item, prop, IdKey.QueryInvalidWhereError);
                    if (error != null) qx.Where.GetTree(error.List);
                    return false;
                }
            }
            return true;
        }
        bool ValidateSelect(QueryX qx, Item item, Property prop, bool clearError)
        {
            if (clearError) ClearError(item, prop);

            var sto = GetQueryXTarget(qx);
            if (qx.Select != null)
            {
                qx.ModelDelta++;
                if (qx.Select.TryValidate(sto))
                {
                    qx.Select.TryResolve();
                    if (qx.Select.AnyUnresolved)
                    {
                        var error = TryAddErrorMany(item, prop, IdKey.QueryUnresolvedSelectError);
                        if (error != null) qx.Select.GetTree(error.List);
                        return false;
                    }
                }
                else
                {
                    var error = TryAddErrorMany(item, prop, IdKey.QueryInvalidSelectError);
                    if (error != null) qx.Select.GetTree(error.List);
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region CanDropQueryXRelation  ========================================
        private bool CanDropQueryXRelation(QueryX qx, Relation re)
        {
            GetHeadTail(qx, out Store p1, out Store c1);
            GetHeadTail(re, out Store p2, out Store c2);

            return (p1 == p2 || c1 == c2 || p1 == c2 || c1 == p2);
        }
        #endregion

        #region GetQueryXTarget  ==============================================
        internal Store  GetQueryXTarget(QueryX qx)
        {
            Store target = null;
            if (Get<Relation_Relation_QueryX>().TryGetParent(qx, out Relation re))
            {
                GetHeadTail(re, out Store head, out target);
                if (qx.IsReversed) { target = head; }
            }
            else
                Get<Relation_Store_QueryX>().TryGetParent(qx, out target);

            return target;
        }
        #endregion

        #region GetHeadTail  ==================================================
        internal void GetHeadTail(QueryX sx, out Store head, out Store tail)
        {
            if (Get<Relation_Relation_QueryX>().TryGetParent(sx, out Relation re))
            {
                GetHeadTail(re, out head, out tail);
                if (sx.IsReversed) { var temp = head; head = tail; tail = temp; }
            }
            else
            {
                Get < Relation_Store_QueryX>().TryGetParent(sx, out head);
                tail = head;
            }
        }
        internal string GetSelectName(QueryX vx)
        {
            GetHeadTail(vx, out Store head, out Store tail);
            return tail.GetNameId();
        }
        internal string GetWhereName(QueryX sx)
        {
            GetHeadTail(sx, out Store head, out Store tail);
            return tail.GetNameId();
        }
        #endregion

        #region GetSymbolXQueryX  =============================================
        int GetSymbolQueryXCount(GraphX gx, Store nodeOwner)
        {
            var N = 0;
            if (Get<Relation_GraphX_SymbolQueryX>().TryGetChildren(gx, out IList<QueryX> qxList))
            {
                foreach (var qx in qxList) { if (Get<Relation_Store_QueryX>().TryGetParent(qx, out Store parent) && nodeOwner == parent) N++; }
            }
            return N;
        }
        (List<SymbolX> symbols, List<QueryX> querys) GetSymbolXQueryX(GraphX gx, Store nodeOwner)
        {
            if (Get<Relation_GraphX_SymbolQueryX>().TryGetChildren(gx, out IList<QueryX> sqxList))
            {
                var sxList = new List<SymbolX>(sqxList.Count);
                var qxList = new List<QueryX>(sqxList.Count);

                var Store_QueryX = Get<Relation_Store_QueryX>();
                var SymbolX_QueryX = Get<Relation_SymbolX_QueryX>();

                foreach (var qx in sqxList)
                {
                    if (Store_QueryX.TryGetParent(qx, out Store store) && store == nodeOwner)
                    {
                        if (SymbolX_QueryX.TryGetParent(qx, out SymbolX sx))
                        {
                            sxList.Add(sx);
                            qxList.Add(qx);
                        }
                    }
                }
                if (sxList.Count > 0) return (sxList, qxList);
            }
            return (null, null);
        }
        #endregion

        #region CreateQueryX  =================================================
        private QueryX CreateQueryX(ViewX vx, Store st)
        {
            var qxNew = new QueryX(Get<QueryXRoot>(), QueryType.View, true);
            ItemCreated.Record(this, qxNew);
            ItemLinked.Record(this, Get<Relation_ViewX_QueryX>(), vx, qxNew);
            ItemLinked.Record(this, Get<Relation_Store_QueryX>(), st, qxNew);
            return qxNew;
        }
        private QueryX CreateQueryX(GraphX gx, Store st)
        {
            var qxNew = new QueryX(Get<QueryXRoot>(), QueryType.Graph, true);
            ItemCreated.Record(this, qxNew);
            ItemLinked.Record(this, Get<Relation_GraphX_QueryX>(), gx, qxNew);
            ItemLinked.Record(this, Get<Relation_Store_QueryX>(), st, qxNew);
            return qxNew;
        }
        private QueryX CreateQueryX(ComputeX cx, Store st)
        {
            var qxNew = new QueryX(Get<QueryXRoot>(), QueryType.Value, true);
            ItemCreated.Record(this, qxNew);
            ItemLinked.Record(this, Get<Relation_ComputeX_QueryX>(), cx, qxNew);
            ItemLinked.Record(this, Get<Relation_Store_QueryX>(), st, qxNew);
            return qxNew;
        }

        private QueryX CreateQueryX(GraphX gx, SymbolX sx, Store st)
        {
            var qxNew = new QueryX(Get<QueryXRoot>(), QueryType.Symbol, true);
            ItemCreated.Record(this, qxNew);
            ItemLinked.Record(this, Get<Relation_GraphX_SymbolQueryX>(), gx, qxNew);
            ItemLinked.Record(this, Get<Relation_SymbolX_QueryX>(), sx, qxNew);
            ItemLinked.Record(this, Get<Relation_Store_QueryX>(), st, qxNew);
            return qxNew;
        }

        private QueryX CreateQueryX(ViewX vx, Relation re)
        {
            var qxNew = new QueryX(Get<QueryXRoot>(), QueryType.View);
            ItemCreated.Record(this, qxNew);
            ItemLinked.Record(this, Get<Relation_ViewX_QueryX>(), vx, qxNew);
            ItemLinked.Record(this, Get<Relation_Relation_QueryX>(), re, qxNew);
            ClearParentTailFlags(qxNew);
            return qxNew;
        }
        private QueryX CreateQueryX(QueryX qx, Relation re, QueryType kind)
        {
            qx.IsTail = false;
            var qxNew = new QueryX(Get<QueryXRoot>(), kind);
            ItemCreated.Record(this, qx);
            ItemLinked.Record(this, Get<Relation_QueryX_QueryX>(), qx, qxNew);
            ItemLinked.Record(this, Get<Relation_Relation_QueryX>(), re, qxNew);
            ClearParentTailFlags(qxNew);
            return qxNew;
        }
        private void ClearParentTailFlags(QueryX qx)
        {
            if (Get<Relation_QueryX_QueryX>().TryGetParents(qx, out IList<QueryX> list))
            {
                foreach (var qp in list) { qp.IsTail = false; }
            }
        }
        #endregion

        #region GeTarget<String, Value>  ======================================
        internal string GetTargetString(Target targ)
        {
            if (targ == Target.Any) return "any";
            if (targ == Target.None) return string.Empty;

            var sb = new StringBuilder();

            if ((targ & Target.N) != 0) Add("N");
            if ((targ & Target.NW) != 0) Add("NW");
            if ((targ & Target.NE) != 0) Add("NE");

            if ((targ & Target.S) != 0) Add("S");
            if ((targ & Target.SW) != 0) Add("SW");
            if ((targ & Target.SE) != 0) Add("SE");

            if ((targ & Target.E) != 0) Add("E");
            if ((targ & Target.EN) != 0) Add("EN");
            if ((targ & Target.ES) != 0) Add("ES");

            if ((targ & Target.W) != 0) Add("W");
            if ((targ & Target.WN) != 0) Add("WN");
            if ((targ & Target.WS) != 0) Add("WS");

            if ((targ & Target.NWC) != 0) Add("NWC");
            if ((targ & Target.NEC) != 0) Add("NEC");
            if ((targ & Target.SWC) != 0) Add("SWC");
            if ((targ & Target.SEC) != 0) Add("SEC");

            return sb.ToString();

            void Add(string v)
            {
                if (sb.Length > 0) sb.Append(" - ");
                sb.Append(v);
            }
        }
        internal Target GetTargetValue(string val)
        {
            var targ = Target.None;
            var v = " " + val.ToUpper().Replace(",", " ").Replace("-", " ").Replace("_", " ") + " ";

            if (v.Contains(" ANY ")) targ = Target.Any;
            if (v.Contains(" N ")) targ |= Target.N;
            if (v.Contains(" NW ")) targ |= Target.NW;
            if (v.Contains(" NE ")) targ |= Target.NE;

            if (v.Contains(" W ")) targ |= Target.W;
            if (v.Contains(" WN ")) targ |= Target.WN;
            if (v.Contains(" WS ")) targ |= Target.WS;

            if (v.Contains(" E ")) targ |= Target.E;
            if (v.Contains(" EN ")) targ |= Target.EN;
            if (v.Contains(" ES ")) targ |= Target.ES;

            if (v.Contains(" S ")) targ |= Target.S;
            if (v.Contains(" SW ")) targ |= Target.SW;
            if (v.Contains(" SE ")) targ |= Target.SE;

            if (v.Contains(" NWC ")) targ |= Target.NWC;
            if (v.Contains(" NEC ")) targ |= Target.NEC;
            if (v.Contains(" SWC ")) targ |= Target.SWC;
            if (v.Contains(" SEC ")) targ |= Target.SEC;

            return targ;
        }
        #endregion

        #region Legacy  =======================================================
        internal bool TrySetWhereProperty(QueryX qx, string val)
        {
            qx.WhereString = val;
            ValidateQueryDependants(qx);
            return IsValid(qx);
        }

        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        internal bool TrySetSelectProperty(QueryX qx, string val)
        {
            qx.SelectString = val;
            ValidateQueryDependants(qx);
            return IsValid(qx);
        }

        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        internal string GetQueryXRelationName(QueryX qx)
        {
            if (Get<Relation_Relation_QueryX>().TryGetParent(qx, out Relation parent))
            {
                return GetRelationName(parent);
            }
            return null;
        }

        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        internal string QueryXLinkName(LineModel model)
        {
            return QueryXFilterName(model.Item as QueryX);
        }

        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        internal string QueryXFilterName(QueryX sd)
        {
            GetHeadTail(sd, out Store head, out Store tail);
            if (head == null || tail == null) return InvalidItem;

            var headName = head.GetNameId();
            var tailName = tail.GetNameId();
            {
                if (sd.HasWhere)
                    return $"{headName}{parentNameSuffix}{tailName}  ({sd.WhereString})";
                else
                    return $"{headName}{parentNameSuffix}{tailName}";
            }
        }

        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        internal string QueryXHeadName(LineModel m)
        {
            var sd = m.Item as QueryX;
            GetHeadTail(sd, out Store head1, out Store tail1);
            var sd2 = GetQueryXTail(sd);
            GetHeadTail(sd2, out Store head2, out Store tail2);

            StringBuilder sb = new StringBuilder(132);
            sb.Append(head1.GetNameId());
            sb.Append(parentNameSuffix);
            sb.Append(tail2.GetNameId());
            return sb.ToString();
        }

        //= = = = = = = = = = = = = = = = = = = = = = = = = = = =

        QueryX GetQueryXTail(QueryX qx)
        {
            var QueryX_QueryX = Get<Relation_QueryX_QueryX>();

            while (QueryX_QueryX.TryGetChild(qx, out QueryX qx2)) { qx = qx2; }
            return qx;
        }
        #endregion
    }
}