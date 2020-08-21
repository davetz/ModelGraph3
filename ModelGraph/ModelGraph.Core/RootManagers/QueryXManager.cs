using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class QueryXManager : ExternalManager<Root, QueryX>, ISerializer, IManager
    {
        static Guid _serializerGuid = new Guid("33B9B8A4-9332-4902-A3C1-37C5F971B6FF");
        static byte _formatVersion = 1;
        internal override IdKey IdKey => IdKey.QueryXManager;

        internal QueryXManager(Root root)
        {
            Owner = root;
            root.RegisterItemSerializer((_serializerGuid, this));
        }

        #region IPrimeRoot  ===================================================
        public void CreateSecondaryHierarchy(Root root)
        {
            root.RegisterReferenceItem(new DummyQueryX(this));

            var sto = root.Get<PropertyManager>();

            root.RegisterReferenceItem(new Property_QueryX_Where(sto));
            root.RegisterReferenceItem(new Property_QueryX_Select(sto));
            root.RegisterReferenceItem(new Property_QueryX_Facet1(sto));
            root.RegisterReferenceItem(new Property_QueryX_Facet2(sto));
            root.RegisterReferenceItem(new Property_QueryX_Connect1(sto));
            root.RegisterReferenceItem(new Property_QueryX_Connect2(sto));
            root.RegisterReferenceItem(new Property_QueryX_LineStyle(sto));
            root.RegisterReferenceItem(new Property_QueryX_DashStyle(sto));
            root.RegisterReferenceItem(new Property_QueryX_LineColor(sto));
            root.RegisterReferenceItem(new Property_QueryX_Relation(sto));
            root.RegisterReferenceItem(new Property_QueryX_IsReversed(sto));
            root.RegisterReferenceItem(new Property_QueryX_IsBreakPoint(sto));
            root.RegisterReferenceItem(new Property_QueryX_ExclusiveKey(sto));
            root.RegisterReferenceItem(new Property_QueryX_ValueType(sto));

            root.RegisterInternalProperties(typeof(QueryX), GetProps(root)); //used by property name lookup
        }

        public void RegisterRelationalReferences(Root root)
        {
            root.RegisterChildRelation(this, root.Get<Relation_QueryX_ViewX>());
            root.RegisterChildRelation(this, root.Get<Relation_QueryX_QueryX>());
            root.RegisterChildRelation(this, root.Get<Relation_QueryX_Property>());

            root.RegisterParentRelation(this, root.Get<Relation_ViewX_QueryX>());
            root.RegisterParentRelation(this, root.Get<Relation_QueryX_QueryX>());
            root.RegisterParentRelation(this, root.Get<Relation_Store_QueryX>());
            root.RegisterParentRelation(this, root.Get<Relation_GraphX_QueryX>());
            root.RegisterParentRelation(this, root.Get<Relation_SymbolX_QueryX>());
            root.RegisterParentRelation(this, root.Get<Relation_ComputeX_QueryX>());
            root.RegisterParentRelation(this, root.Get<Relation_Relation_QueryX>());
            root.RegisterParentRelation(this, root.Get<Relation_GraphX_SymbolQueryX>());

            InitializeLocalReferences(root);
        }
        public void ValidateDomain(Root root) 
        {
            ValidateQueryXStore();
            ResetItemTriggeredRefresh();
        }

        private Property[] GetProps(Root root) => new Property[]
        {
            root.Get<Property_QueryX_Where>(),
            root.Get<Property_QueryX_Select>(),
            root.Get<Property_QueryX_Facet1>(),
            root.Get<Property_QueryX_Facet2>(),
            root.Get<Property_QueryX_Connect1>(),
            root.Get<Property_QueryX_Connect2>(),
            root.Get<Property_QueryX_LineStyle>(),
            root.Get<Property_QueryX_DashStyle>(),
            root.Get<Property_QueryX_LineColor>(),
            root.Get<Property_QueryX_Relation>(),
            root.Get<Property_QueryX_IsReversed>(),
            root.Get<Property_QueryX_IsBreakPoint>(),
            root.Get<Property_QueryX_ExclusiveKey>(),
            root.Get<Property_QueryX_ValueType>(),
        };
        #endregion

        #region ISerializer  ==================================================
        public void ReadData(DataReader r, Item[] items)
        {
            var N = r.ReadInt32();
            if (N < 1) throw new Exception($"Invalid count {N}");
            SetCapacity(N);

            var fv = r.ReadByte();
            if (fv == 1)
            {
                for (int i = 0; i < N; i++)
                {
                    var index = r.ReadInt32();
                    if (index < 0 || index >= items.Length) throw new Exception($"Invalid index {index}");

                    var qx = new QueryX(this);
                    items[index] = qx;

                    var b = r.ReadUInt16();
                    if ((b & S1) != 0) qx.SetState(r.ReadUInt16());
                    if ((b & S2) != 0) qx.WhereString = Value.ReadString(r);
                    if ((b & S3) != 0) qx.SelectString = Value.ReadString(r);
                    if ((b & S4) != 0) qx.ExclusiveKey = r.ReadByte();

                    if (qx.QueryKind == QueryType.Path && qx.IsHead) qx.PathParm = new PathParm();

                    if ((b & S5) != 0) qx.PathParm.Facet1 = (Facet)r.ReadByte();
                    if ((b & S6) != 0) qx.PathParm.Target1 = (Target)r.ReadUInt16();

                    if ((b & S7) != 0) qx.PathParm.Facet2 = (Facet)r.ReadByte();
                    if ((b & S8) != 0) qx.PathParm.Target2 = (Target)r.ReadUInt16();

                    if ((b & S9) != 0) qx.PathParm.DashStyle = (DashStyle)r.ReadByte();
                    if ((b & S10) != 0) qx.PathParm.LineStyle = (LineStyle)r.ReadByte();
                    if ((b & S11) != 0) qx.PathParm.LineColor = Value.ReadString(r);
                }
            }
            else
                throw new Exception($"ColumnXDomain ReadData, unknown format version: {fv}");
        }

        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            if (Count > 0)
            {
                w.WriteInt32(Count);
                w.WriteByte(_formatVersion);

                foreach (var qx in Items)
                {
                    w.WriteInt32(itemIndex[qx]);

                    var b = SZ;
                    if (qx.HasState()) b |= S1;
                    if (!string.IsNullOrWhiteSpace(qx.WhereString)) b |= S2;
                    if (!string.IsNullOrWhiteSpace(qx.SelectString)) b |= S3;
                    if (qx.IsExclusive) b |= S4;
                    if (qx.QueryKind == QueryType.Path && qx.IsHead == true && qx.PathParm != null)
                    {
                        if (qx.PathParm.Facet1 != Facet.None) b |= S5;
                        if (qx.PathParm.Target1 != Target.Any) b |= S6;

                        if (qx.PathParm.Facet2 != Facet.None) b |= S7;
                        if (qx.PathParm.Target2 != Target.Any) b |= S8;

                        if (qx.PathParm.DashStyle != DashStyle.Solid) b |= S9;
                        if (qx.PathParm.LineStyle != LineStyle.PointToPoint) b |= S10;
                        if (!string.IsNullOrWhiteSpace(qx.PathParm.LineColor)) b |= S11;
                    }

                    w.WriteUInt16(b);
                    if ((b & S1) != 0) w.WriteUInt16(qx.GetState());
                    if ((b & S2) != 0) Value.WriteString(w, qx.WhereString);
                    if ((b & S3) != 0) Value.WriteString(w, qx.SelectString);
                    if ((b & S4) != 0) w.WriteByte(qx.ExclusiveKey);

                    if ((b & S5) != 0) w.WriteByte((byte)qx.PathParm.Facet1);
                    if ((b & S6) != 0) w.WriteUInt16((ushort)qx.PathParm.Target1);

                    if ((b & S7) != 0) w.WriteByte((byte)qx.PathParm.Facet2);
                    if ((b & S8) != 0) w.WriteUInt16((ushort)qx.PathParm.Target2);

                    if ((b & S9) != 0) w.WriteByte((byte)qx.PathParm.DashStyle);
                    if ((b & S10) != 0) w.WriteByte((byte)qx.PathParm.LineStyle);
                    if ((b & S11) != 0) Value.WriteString(w, qx.PathParm.LineColor);
                }
            }
        }
        #endregion

        #region QueryXHelpers  ================================================
        //========================================== frequently used references
        private ErrorManager _errorManager;
        private GraphXManager _graphXManager;
        private ComputeXManager _computeXManager;

        private Property_QueryX_Where _property_QueryX_Where;
        private Property_QueryX_Select _property_QueryX_Select;
        private Property_ComputeX_Select _property_ComputeX_Select;

        private Relation_Store_QueryX _relation_Store_QueryX;
        private Relation_ViewX_QueryX _relation_ViewX_QueryX;
        private Relation_QueryX_QueryX _relation_QueryX_QueryX;
        private Relation_GraphX_QueryX _relation_GraphX_QueryX;
        private Relation_SymbolX_QueryX _relation_SymbolX_QueryX;
        private Relation_Store_ComputeX _relation_Store_ComputeX;
        private Relation_ComputeX_QueryX _relation_ComputeX_QueryX;
        private Relation_Relation_QueryX _relation_Relation_QueryX;
        private Relation_GraphX_SymbolQueryX _relation_GraphX_SymbolQueryX;

        #region InitializeLocalReferences  ====================================
        private void InitializeLocalReferences(Root root)
        {
            _errorManager = root.Get<ErrorManager>();
            _graphXManager = root.Get<GraphXManager>();
            _computeXManager = root.Get<ComputeXManager>();

            _property_QueryX_Where = root.Get<Property_QueryX_Where>();
            _property_QueryX_Select = root.Get<Property_QueryX_Select>();
            _property_ComputeX_Select = root.Get<Property_ComputeX_Select>();

            _relation_Store_QueryX = root.Get<Relation_Store_QueryX>();
            _relation_ViewX_QueryX = root.Get<Relation_ViewX_QueryX>();
            _relation_QueryX_QueryX = root.Get<Relation_QueryX_QueryX>();
            _relation_GraphX_QueryX = root.Get<Relation_GraphX_QueryX>();
            _relation_SymbolX_QueryX = root.Get<Relation_SymbolX_QueryX>();
            _relation_Store_ComputeX = root.Get<Relation_Store_ComputeX>();
            _relation_ComputeX_QueryX = root.Get<Relation_ComputeX_QueryX>();
            _relation_Relation_QueryX = root.Get<Relation_Relation_QueryX>();
            _relation_GraphX_SymbolQueryX = root.Get<Relation_GraphX_SymbolQueryX>();
        }
        #endregion

        #region ResetItemTriggeredRefresh  ====================================
        private void ResetItemTriggeredRefresh()
        {
            var props = new HashSet<Property>();
            var workQueue = new Queue<QueryX>();
            Owner.ClearItemTriggeredRefresh();

            foreach (var cx in _computeXManager.Items)
            {
                if (_relation_ComputeX_QueryX.TryGetChild(cx, out QueryX qh)) SetTriggers(cx, qh);
            }

            foreach (var gx in _graphXManager.Items)
            {
                if (_relation_GraphX_QueryX.TryGetChildren(gx, out IList<QueryX> roots))
                    foreach (var qh in roots) { SetTriggers(gx, qh); }
            }

            void SetTriggers(Item ti, QueryX qh)
            {
                workQueue.Clear();
                workQueue.Enqueue(qh);
                while(workQueue.Count > 0)
                {
                    var qx = workQueue.Dequeue();
                    props.Clear();
                    qx.GetPropertyRefs(props);
                    foreach (var p in props)
                    {
                        p.IsRefreshTriggerItem = true;
                        Owner.AddItemTriggeredRefresh(p, ti);
                    }
                    if (_relation_Relation_QueryX.TryGetParent(qx, out Relation r))
                    {
                        r.IsRefreshTriggerItem = true;
                        Owner.AddItemTriggeredRefresh(r, ti);
                    }
                    if (_relation_QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> list))
                    {
                        foreach (var q in list) { workQueue.Enqueue(q); }
                    }
                }
            }

        }
        #endregion

        #region ValidateQueryXStore  ==========================================
        private void ValidateQueryXStore()
        {
            foreach (var cx in _computeXManager.Items)
            {
                ValidateComputeQuery(cx);
            }
            RevalidateUnresolved();
            foreach (var gx in _graphXManager.Items)
            {
                ValidateGraphQuery(gx);
            }
        }
        /// <summary>This should a manually initiated action</summary>
        /// <param name="cx"></param>
        internal void RevalidateComputeX(ComputeX cx)
        {
            ValidateComputeQuery(cx, true);
            ResetItemTriggeredRefresh();
        }

        internal void ValidateQueryDependants(QueryX qx)
        {
            while (_relation_QueryX_QueryX.TryGetParent(qx, out QueryX qp)) { qx = qp; }

            if (_relation_ComputeX_QueryX.TryGetParent(qx, out ComputeX cx))
                ValidateComputeQuery(cx, true);
            else if (_relation_GraphX_QueryX.TryGetParent(qx, out GraphX gx))
            {
                ValidateGraphQuery(gx, true);
            }
            else if (_relation_SymbolX_QueryX.TryGetParent(qx, out SymbolX sx))
            {
                ValidateWhere(qx, qx, _property_QueryX_Where, true);
            }
        }

        private void ValidateComputeQuery(ComputeX cx, bool clearError = false)
        {
            cx.Value.Clear();
            cx.Value = Value.ValuesUnknown;
            if (clearError)
            {
                _errorManager.ClearError(cx);
                _errorManager.ClearError(cx, _property_ComputeX_Select);
            }
            cx.ModelDelta++;
            if (!_relation_ComputeX_QueryX.TryGetChild(cx, out QueryX qx))
            {
                cx.Value = Value.ValuesInvalid;
                _errorManager.TryAddErrorNone(cx, IdKey.Error_216_ComputeMissingRootQuery);
            }
            else
            {
                if (qx.Select == null)
                {
                    if (cx.CompuType == CompuType.RowValue)
                    {
                        _errorManager.TryAddErrorNone(cx, _property_ComputeX_Select, IdKey.Error_215_ComputeMissingSelect);
                    }
                }
                else
                {
                    ValidateSelect(qx, cx, _property_ComputeX_Select, clearError);
                }
            }
            if (_relation_QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> children))
                ValidateQueryHierarchy(children, clearError);
            _computeXManager.AllocateValueCache(cx);
        }
        private void ValidateGraphQuery(GraphX gx, bool clearError = false)
        {
            if (clearError) _errorManager.ClearError(gx);

            if (_relation_GraphX_QueryX.TryGetChildren(gx, out IList<QueryX> pathQuery))
                ValidateQueryHierarchy(pathQuery, clearError);

            if (_relation_GraphX_SymbolQueryX.TryGetChildren(gx, out IList<QueryX> list))
            {
                ValidateQueryHierarchy(list, clearError);
            }
        }
        private void RevalidateUnresolved()
        {
            for (int i = 0; i < 10; i++)
            {
                var anyUnresolved = false;

                foreach (var cx in _computeXManager.Items)
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
                    _errorManager.ClearError(qx, _property_QueryX_Where);
                    _errorManager.ClearError(qx, _property_QueryX_Select);
                }

                if (_relation_QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> children))
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
                            var error = _errorManager.TryAddErrorMany(qx, _property_QueryX_Select, IdKey.Error_222_QueryUnresolvedSelect);
                            if (error != null) qx.Select.GetTree(error.List);
                        }
                    }
                    else
                    {
                        var error = _errorManager.TryAddErrorMany(qx, _property_QueryX_Select, IdKey.Error_223_QueryInvalidSelect);
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
                            var error = _errorManager.TryAddErrorMany(qx, _property_QueryX_Where, IdKey.Error_220_QueryUnresolvedWhere);
                            if (error != null) qx.Where.GetTree(error.List);
                        }
                    }
                    else
                    {
                        var error = _errorManager.TryAddErrorMany(qx, _property_QueryX_Where, IdKey.Error_221_QueryInvalidWhere);
                        if (error != null) qx.Where.GetTree(error.List);
                    }
                }
            }
        }
        bool ValidateWhere(QueryX qx, Item item, Property prop, bool clearError)
        {
            if (clearError) _errorManager.ClearError(item, prop);

            var sto = GetQueryXTarget(qx);
            if (qx.Where != null)
            {
                qx.ModelDelta++;
                if (qx.Where.TryValidate(sto))
                {
                    qx.Where.TryResolve();
                    if (qx.Where.AnyUnresolved)
                    {
                        var error = _errorManager.TryAddErrorMany(item, prop, IdKey.Error_220_QueryUnresolvedWhere);
                        if (error != null) qx.Where.GetTree(error.List);
                        return false;
                    }
                }
                else
                {
                    var error = _errorManager.TryAddErrorMany(item, prop, IdKey.Error_221_QueryInvalidWhere);
                    if (error != null) qx.Where.GetTree(error.List);
                    return false;
                }
            }
            return true;
        }
        bool ValidateSelect(QueryX qx, Item item, Property prop, bool clearError)
        {
            if (clearError) _errorManager.ClearError(item, prop);

            var sto = GetQueryXTarget(qx);
            if (qx.Select != null)
            {
                qx.ModelDelta++;
                if (qx.Select.TryValidate(sto))
                {
                    qx.Select.TryResolve();
                    if (qx.Select.AnyUnresolved)
                    {
                        var error = _errorManager.TryAddErrorMany(item, prop, IdKey.Error_222_QueryUnresolvedSelect);
                        if (error != null) qx.Select.GetTree(error.List);
                        return false;
                    }
                }
                else
                {
                    var error = _errorManager.TryAddErrorMany(item, prop, IdKey.Error_223_QueryInvalidSelect);
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
           var (p1, c1) = GetHeadTail(qx);
           var (p2, c2) = re.GetHeadTail();

            return (p1 == p2 || c1 == c2 || p1 == c2 || c1 == p2);
        }
        #endregion

        #region GetQueryXTarget  ==============================================
        internal Store GetQueryXTarget(QueryX qx)
        {
            var (_, tail) = GetHeadTail(qx);
            return tail;
        }
        #endregion

        #region GetHeadTail  ==================================================
        internal (Store, Store) GetHeadTail(QueryX qx)
        {
            if (_relation_Relation_QueryX.TryGetParent(qx, out Relation re))
            {
                var (head, tail) = re.GetHeadTail();
                return qx.IsReversed ? (tail, head) : (head, tail);
            }
            else
            {
                _relation_Store_QueryX.TryGetParent(qx, out Store head);
                return (head, head);
            }
        }
        #endregion

        #region GetSymbolXQueryX  =============================================
        int GetSymbolQueryXCount(GraphX gx, Store nodeOwner)
        {
            var N = 0;
            if (_relation_GraphX_SymbolQueryX.TryGetChildren(gx, out IList<QueryX> qxList))
            {
                foreach (var qx in qxList) { if (_relation_Store_QueryX.TryGetParent(qx, out Store parent) && nodeOwner == parent) N++; }
            }
            return N;
        }
        (List<SymbolX> symbols, List<QueryX> querys) GetSymbolXQueryX(GraphX gx, Store nodeOwner)
        {
            if (_relation_GraphX_SymbolQueryX.TryGetChildren(gx, out IList<QueryX> sqxList))
            {
                var sxList = new List<SymbolX>(sqxList.Count);
                var qxList = new List<QueryX>(sqxList.Count);

                foreach (var qx in sqxList)
                {
                    if (_relation_Store_QueryX.TryGetParent(qx, out Store store) && store == nodeOwner)
                    {
                        if (_relation_SymbolX_QueryX.TryGetParent(qx, out SymbolX sx))
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
            var qxNew = new QueryX(this, QueryType.View, true);
            ItemCreated.Record(Owner, qxNew);
            ItemLinked.Record(Owner, _relation_ViewX_QueryX, vx, qxNew);
            ItemLinked.Record(Owner, _relation_Store_QueryX, st, qxNew);
            return qxNew;
        }
        private QueryX CreateQueryX(GraphX gx, Store st)
        {
            var qxNew = new QueryX(this, QueryType.Graph, true);
            ItemCreated.Record(Owner, qxNew);
            ItemLinked.Record(Owner, _relation_GraphX_QueryX, gx, qxNew);
            ItemLinked.Record(Owner, _relation_Store_QueryX, st, qxNew);
            return qxNew;
        }
        private QueryX CreateQueryX(ComputeX cx, Store st)
        {
            var qxNew = new QueryX(this, QueryType.Value, true);
            ItemCreated.Record(Owner, qxNew);
            ItemLinked.Record(Owner, _relation_ComputeX_QueryX, cx, qxNew);
            ItemLinked.Record(Owner, _relation_Store_QueryX, st, qxNew);
            return qxNew;
        }

        private QueryX CreateQueryX(GraphX gx, SymbolX sx, Store st)
        {
            var qxNew = new QueryX(this, QueryType.Symbol, true);
            ItemCreated.Record(Owner, qxNew);
            ItemLinked.Record(Owner, _relation_GraphX_SymbolQueryX, gx, qxNew);
            ItemLinked.Record(Owner, _relation_SymbolX_QueryX, sx, qxNew);
            ItemLinked.Record(Owner, _relation_Store_QueryX, st, qxNew);
            return qxNew;
        }

        private QueryX CreateQueryX(ViewX vx, Relation re)
        {
            var qxNew = new QueryX(this, QueryType.View);
            ItemCreated.Record(Owner, qxNew);
            ItemLinked.Record(Owner, _relation_ViewX_QueryX, vx, qxNew);
            ItemLinked.Record(Owner, _relation_Relation_QueryX, re, qxNew);
            ClearParentTailFlags(qxNew);
            return qxNew;
        }
        private QueryX CreateQueryX(QueryX qx, Relation re, QueryType kind)
        {
            qx.IsTail = false;
            var qxNew = new QueryX(this, kind);
            ItemCreated.Record(Owner, qx);
            ItemLinked.Record(Owner, _relation_QueryX_QueryX, qx, qxNew);
            ItemLinked.Record(Owner, _relation_Relation_QueryX, re, qxNew);
            ClearParentTailFlags(qxNew);
            return qxNew;
        }
        private void ClearParentTailFlags(QueryX qx)
        {
            if (_relation_QueryX_QueryX.TryGetParents(qx, out IList<QueryX> list))
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

        #region PropertyModels  ===============================================
        internal string GetWhereSelectTargetName(QueryX qx)
        {
            var (_, tail) = GetHeadTail(qx);
            return tail is null ? InvalidItem : tail.GetNameId();
        }
        internal string GetRelationNameId(QueryX qx) => _relation_Relation_QueryX.TryGetParent(qx, out Relation re) ? re.GetNameId() : InvalidItem;
        #endregion

        #endregion

        #region ModelHelpers  =================================================
        internal Relation GetRelation(QueryX qx) => _relation_Relation_QueryX.TryGetParent(qx, out Relation r) ? r : null;
        internal Store GetStore(QueryX qx) => _relation_Store_QueryX.TryGetParent(qx, out Store s) ? s : null;

        internal int QueryQueryChildCount(QueryX qx) => _relation_QueryX_QueryX.ChildCount(qx);
        internal IList<QueryX> QueryQueryChildList(QueryX qx) => _relation_QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> list) ? list : new QueryX[0];

        internal bool QueryRelationDrop(QueryX qx1, Relation r2, bool doDrop)
        {
            if (_relation_Relation_QueryX.TryGetParent(qx1, out Relation r1))
            {
                var (_, t1) = r1.GetHeadTail();
                var (h2, t2) = r2.GetHeadTail();
                if (t1 == h2 || t1 == t2)
                {
                    if (doDrop)
                    {
                        qx1.IsTail = false;
                        var qx2 = new QueryX(this, QueryType.Value);
                        qx2.IsReversed = t1 == t2;
                        ItemCreated.Record(Owner, qx2);
                        ItemLinked.Record(Owner, _relation_QueryX_QueryX, qx1, qx2);
                        ItemLinked.Record(Owner, _relation_Relation_QueryX, r2, qx2);
                    }
                    return true;
                }
            }
            return false;
        }

        internal void AddComputeXQueryX(ComputeX cx, QueryX qx1, Relation rx, bool isReversed)
        {
            qx1.IsTail = false;
            var qx2 = new QueryX(this, QueryType.Value);
            qx2.IsReversed = isReversed;
            ItemCreated.Record(Owner, qx2);
            ItemLinked.Record(Owner, _relation_QueryX_QueryX, qx1, qx2);
            ItemLinked.Record(Owner, _relation_Relation_QueryX, rx, qx2);
        }
        #endregion

        #region QueryForest  ==================================================

        #region GetForest  ====================================================
        /// <summary>
        /// Return a GraphX forest of query trees
        /// </summary>
        internal bool TryGetForest(Graph g, Item seed, HashSet<Store> nodeOwners)
        {
            g.Forest = null;
            var gx = g.Owner;

            _graphXManager.RebuildGraphX_ARGBList_NodeOwners(gx);
            if (_relation_GraphX_QueryX.TryGetChildren(gx, out IList<QueryX> roots))
            {
                var workList = new List<Query>();
                if (TryGetForestRoots(roots, seed, workList, out Query[] forest))
                {
                    g.Forest = forest;

                    var keyPairs = new Dictionary<byte, List<(Item, Item)>>();
                    var workQueue = new Queue<Query>(forest);
                    while (workQueue.Count > 0)
                    {
                        var query = workQueue.Dequeue();

                        if (nodeOwners.Contains(query.Item.Store)) g.NodeItems.Add(query.Item);
                        if (query.Items == null) continue;
                        foreach (var itm in query.Items) { if (nodeOwners.Contains(itm.Store)) g.NodeItems.Add(itm); }

                        if (_relation_QueryX_QueryX.TryGetChildren(query.Owner, out IList<QueryX> qxChildren))
                        {
                            var N = query.Items.Length;
                            query.Children = new Query[N][];

                            for (int i = 0; i < N; i++)
                            {
                                var item = query.Items[i];
                                workList.Clear();

                                foreach (var qc in qxChildren)
                                {
                                    var child = GetChildQuery(g, qc, query, item, keyPairs);
                                    if (child == null) continue;

                                    workList.Add(child);
                                    workQueue.Enqueue(child);
                                }
                                if (workList.Count > 0) query.Children[i] = workList.ToArray();
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region GetForest  ====================================================
        /// <summary>
        /// Return a query forest for the callers computeX.
        /// Also return a list of query's who's parent queryX has a valid select clause. 
        /// </summary>
        internal bool TryGetForest(ComputeX cx, Item seed, List<Query> selectors, out Query[] forest)
        {
            forest = null;
            if (_relation_ComputeX_QueryX.TryGetChildren(cx, out IList<QueryX> qxRoots))
            {
                var workList = new List<Query>();
                if (TryGetForestRoots(qxRoots, seed, workList, out forest))
                {
                    var workQueue = new Queue<Query>(forest);
                    while (workQueue.Count > 0)
                    {
                        var q = workQueue.Dequeue();
                        if (q.Items != null)
                        {
                            if (_relation_QueryX_QueryX.TryGetChildren(q.Owner, out IList<QueryX> qxChildren))
                            {
                                var N = q.Items.Length;
                                q.Children = new Query[N][];

                                for (int i = 0; i < N; i++)
                                {
                                    var item = q.Items[i];

                                    workList.Clear();
                                    foreach (var qx in qxChildren)
                                    {
                                        var qc = GetChildQuery(qx, q, item);
                                        if (qc == null) continue;
                                        if (qc.IsTail && qx.HasValidSelect)
                                        {
                                            var p = qc.Parent;
                                            while (p.Parent != null) { p = p.Parent; }
                                            var qp = p.Owner;
                                            if (qp.HasValidSelect)
                                                selectors.Add(p);
                                            selectors.Add(qc);
                                        }
                                        workList.Add(qc);
                                        workQueue.Enqueue(qc);
                                    }
                                    if (workList.Count > 0) q.Children[i] = workList.ToArray();
                                }
                            }
                        }
                    }
                }
            }
            return (forest != null);
        }
        #endregion

        #region GetRoots  =====================================================
        bool TryGetForestRoots(IList<QueryX> qxRoots, Item seed, List<Query> workList, out Query[] forest)
        {
            workList.Clear();
            if (qxRoots != null && qxRoots.Count > 0)
            {
                foreach (var qx in qxRoots)
                {
                    if (_relation_Store_QueryX.TryGetParent(qx, out Store sto))
                    {
                        if (seed == null || seed.GetOwner() != sto && sto.Count > 0)
                        {
                            var items = sto.GetItems();
                            if (qx.HasWhere) items = ApplyFilter(qx, items);

                            if (items != null) workList.Add(new Query(qx, null, sto, items.ToArray()));
                        }
                        else if (seed != null && seed.GetOwner() == sto)
                        {
                            workList.Add(new Query(qx, null, sto, new Item[] { seed }));
                        }
                    }
                }
            }
            forest = (workList.Count > 0) ? workList.ToArray() : null;
            return (forest != null);
        }
        #endregion

        #region GetChildQuery  ================================================
        Query GetChildQuery(Graph g, QueryX qx, Query q, Item item, Dictionary<byte, List<(Item, Item)>> keyPairs)
        {
            if (!_relation_Relation_QueryX.TryGetParent(qx, out Relation r)) return null;

            List<Item> items = null;
            if (qx.IsReversed)
                r.TryGetParents(item, out items);
            else
                r.TryGetChildren(item, out items);

            if (qx.HasWhere && items != null)
            {
                items = ApplyFilter(qx, items);
                if (items == null) return null;
            }

            if (items == null)
            {
                if (qx.QueryKind == QueryType.Path)
                {
                    AddOpenQueryPair(g, new Query(qx, q, item, null));
                }
                return null;
            }

            if (qx.IsExclusive) items = RemoveDuplicates(qx, item, items, keyPairs);
            if (items == null) return null;

            if (_relation_QueryX_QueryX.HasNoChildren(qx)) { qx.IsTail = true; }

            var q2 = new Query(qx, q, item, items.ToArray());
            if (qx.IsTail)
            {
                var q1 = q2.GetHeadQuery();

                if (qx.QueryKind == QueryType.Path)
                {
                    g.PathQuerys.Add((q1, q2));
                }

                if (q2.ItemCount == 1 && q1.Item != q2.Items[0])
                {
                    switch (qx.QueryKind)
                    {
                        case QueryType.Group:
                            g.GroupQuerys.Add((q1, q2));
                            break;
                        case QueryType.Egress:
                            g.SegueQuerys.Add((q1, q2));
                            break;
                    }
                }
            }
            return q2;
        }
        private void AddOpenQueryPair(Graph g, Query q2)
        {
            var q1 = q2.GetHeadQuery();
            var N = g.OpenQuerys.Count;
            for (int i = 0; i < N; i++)
            {
                if (g.OpenQuerys[i].Item1.Item != q1.Item) continue;
                g.OpenQuerys.Insert(i, (q1, q2));
                return;
            }
            g.OpenQuerys.Add((q1, q2));
        }
        #endregion

        #region GetChildQuery  ================================================
        Query GetChildQuery(QueryX qx, Query q, Item item)
        {
            if (!_relation_Relation_QueryX.TryGetParent(qx, out Relation r)) return null;

            List<Item> items = null;
            if (qx.IsReversed)
                r.TryGetParents(item, out items);
            else
                r.TryGetChildren(item, out items);

            if (qx.HasWhere) items = ApplyFilter(qx, items);
            if (items == null) return null;

            return new Query(qx, q, item, items.ToArray());
        }
        #endregion

        #region ApplyFilter  ==================================================
        List<Item> ApplyFilter(QueryX sd, List<Item> input)
        {
            if (input == null) return null;

            var output = input;
            var M = input.Count;
            var N = M;
            var filter = sd.Where;
            for (int i = 0; i < M; i++)
            {
                if (filter.Matches(input[i])) continue;
                input[i] = null; N--;
            }
            return RemoveNulls(input, M, N);
        }
        #endregion

        #region RemoveDuplicates  =============================================
        // do not cross the same edge (item to input[n]) twice
        List<Item> RemoveDuplicates(QueryX sd, Item item, List<Item> input, Dictionary<byte, List<(Item, Item)>> keyPairs)
        {
            var M = input.Count;
            var N = M;

            if (!keyPairs.TryGetValue(sd.ExclusiveKey, out List<(Item, Item)> itemPairs))
            {
                itemPairs = new List<(Item, Item)>(M);
                keyPairs.Add(sd.ExclusiveKey, itemPairs);
            }

            for (var i = 0; i < M; i++)
            {
                var item2 = input[i];
                if (item2 == null) continue;

                for (var j = 0; j < itemPairs.Count; j++)
                {
                    if (itemPairs[j].Item1 != item) continue;
                    if (itemPairs[j].Item2 != item2) continue;
                    item2 = input[i] = null; N--;
                    break;
                }
                if (item2 != null) itemPairs.Add((item, item2));
            }
            return RemoveNulls(input, M, N);
        }
        #endregion

        #region RemoveNulls  ==================================================
        private List<Item> RemoveNulls(List<Item> input, int M, int N)
        {
            if (N == 0) return null;
            if (N == M) return input;

            var output = new List<Item>(N);
            foreach (var item in input)
            {
                if (item == null) continue;
                output.Add(item);
            }
            return output;
        }
        private List<QueryX> RemoveNulls(List<QueryX> input, int M, int N)
        {
            if (N == 0) return null;
            if (N == M) return input;

            var output = new List<QueryX>(N);
            foreach (var item in input)
            {
                if (item == null) continue;
                output.Add(item);
            }
            return output;
        }
        #endregion

        #endregion
    }
}
