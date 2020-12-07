using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class ComputeXRoot : ExternalRoot<Root, ComputeX>, ISerializer, IRootDomain
    {
        static Guid _serializerGuid = new Guid("35522B27-A925-4CE0-8D65-EDEF451097F2");
        static byte _formatVersion = 1;
        internal override IdKey IdKey => IdKey.ComputeXRoot;
        internal ComputeXRoot(Root root)
        {
            Owner = root;
            root.RegisterItemSerializer((_serializerGuid, this));
        }

        #region IPrimeRoot  ===================================================
        public void CreateSecondaryHierarchy(Root root)
        {
            var sto = root.Get<PropertyRoot>();

            root.RegisterReferenceItem(new Property_ComputeX_CompuType(sto));
            root.RegisterReferenceItem(new Property_ComputeX_Where(sto));
            root.RegisterReferenceItem(new Property_ComputeX_Select(sto));
            root.RegisterReferenceItem(new Property_ComputeX_Separator(sto));
            root.RegisterReferenceItem(new Property_ComputeX_ValueType(sto));

            root.RegisterInternalProperties(typeof(ComputeX), GetProps(root)); //used by property name lookup
        }

        public void RegisterRelationalReferences(Root root)
        {
            root.RegisterChildRelation(this, root.Get<Relation_ComputeX_QueryX>());

            root.RegisterParentRelation(this, root.Get<Relation_Store_ComputeX>());
            root.RegisterParentRelation(this, root.Get<Relation_Store_NameProperty>());
            root.RegisterParentRelation(this, root.Get<Relation_Store_SummaryProperty>());
            root.RegisterParentRelation(this, root.Get<Relation_QueryX_Property>());

            InitializeLocalReferences(root);
        }
        public void ValidateDomain(Root root) { }

        private Property[] GetProps(Root root) => new Property[]
        {
            root.Get<Property_Item_Name>(),
            root.Get<Property_Item_Summary>(),
            root.Get<Property_Item_Description>(),

            root.Get<Property_ComputeX_CompuType>(),
            root.Get<Property_ComputeX_Where>(),
            root.Get<Property_ComputeX_Select>(),
            root.Get<Property_ComputeX_Separator>(),
            root.Get<Property_ColumnX_ValueType>(),
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

                    var cx = new ComputeX(this);
                    items[index] = cx;

                    var b = r.ReadByte();
                    if ((b & B1) != 0) cx.SetState(r.ReadUInt16());
                    if ((b & B2) != 0) cx.Name = Value.ReadString(r);
                    if ((b & B3) != 0) cx.Summary = Value.ReadString(r);
                    if ((b & B4) != 0) cx.Description = Value.ReadString(r);
                    if ((b & B5) != 0) cx.Separator = Value.ReadString(r);
                    if ((b & B6) != 0) cx.CompuType = (CompuType)r.ReadByte();
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

                foreach (var cx in Items)
                {
                    w.WriteInt32(itemIndex[cx]);

                    var b = BZ;
                    if (cx.HasState()) b |= B1;
                    if (!string.IsNullOrWhiteSpace(cx.Name)) b |= B2;
                    if (!string.IsNullOrWhiteSpace(cx.Summary)) b |= B3;
                    if (!string.IsNullOrWhiteSpace(cx.Description)) b |= B4;
                    if (cx.Separator != ComputeX.DefaultSeparator) b |= B5;
                    if (cx.CompuType != CompuType.RowValue) b |= B6;

                    w.WriteByte(b);
                    if ((b & B1) != 0) w.WriteUInt16(cx.GetState());
                    if ((b & B2) != 0) Value.WriteString(w, cx.Name);
                    if ((b & B3) != 0) Value.WriteString(w, cx.Summary);
                    if ((b & B4) != 0) Value.WriteString(w, cx.Description);
                    if ((b & B5) != 0) Value.WriteString(w, (cx.Separator ?? string.Empty));
                    if ((b & B6) != 0) w.WriteByte((byte)cx.CompuType);
                }
            }
        }
        #endregion

        #region DomainMethods  ================================================
        //========================================== frequently used references
        private QueryXRoot _queryXManager;
        private ChangeManager _changeManager;

        private Relation_QueryX_QueryX _relation_QueryX_QueryX;
        private Relation_Store_ComputeX _relation_Store_ComputeX;
        private Relation_ComputeX_QueryX _relation_ComputeX_QueryX;
        private Relation_Relation_QueryX _relation_Relation_QueryX;

        #region InitializeLocalReferences  ====================================
        private void InitializeLocalReferences(Root root)
        {
            _queryXManager = root.Get<QueryXRoot>();
            _changeManager = root.Get<ChangeManager>();

            _relation_QueryX_QueryX = root.Get<Relation_QueryX_QueryX>();
            _relation_Store_ComputeX = root.Get<Relation_Store_ComputeX>();
            _relation_ComputeX_QueryX = root.Get<Relation_ComputeX_QueryX>();
            _relation_Relation_QueryX = root.Get<Relation_Relation_QueryX>();
        }
        #endregion

        internal bool TryGetParent(ComputeX cx, out Store p) => _relation_Store_ComputeX.TryGetParent(cx, out p);

        internal void ValidateComputeX(ComputeX cx) =>  _queryXManager.RevalidateComputeX(cx);

        #region ResetCacheValues  =============================================
        internal void ResetCacheValues()
        {
            foreach (var cx in Items) { cx.Value.Clear(); }
        }
        #endregion

        #region <Get/Set><Where/Select>String  ================================
        internal string GetWhereString(ComputeX cx) => _relation_ComputeX_QueryX.TryGetChild(cx, out QueryX qx) ? (qx.HasWhere ? qx.WhereString : null) : InvalidItem;
        internal string GetSelectString(ComputeX cx) => _relation_ComputeX_QueryX.TryGetChild(cx, out QueryX qx) ? (qx.HasSelect ? qx.SelectString : null) : InvalidItem;

        internal void SetWhereString(ComputeX cx, string value) { if (_relation_ComputeX_QueryX.TryGetChild(cx, out QueryX qx)) qx.WhereString = value; }
        internal void SetSelectString(ComputeX cx, string value) { if (_relation_ComputeX_QueryX.TryGetChild(cx, out QueryX qx)) qx.SelectString = value; _queryXManager.ValidateQueryDependants(qx); }
        #endregion

        #region Model_658_Compute  ============================================
        internal int GetQueryXCount(ComputeX cx) => _relation_ComputeX_QueryX.TryGetChild(cx, out QueryX q1) ? _relation_QueryX_QueryX.ChildCount(q1) : 0;
        internal IList<QueryX> GetQueryXList(ComputeX cx) => _relation_ComputeX_QueryX.TryGetChild(cx, out QueryX q1) && _relation_QueryX_QueryX.TryGetChildren(q1, out IList<QueryX> list) ? list : new QueryX[0];
        internal bool ComputeXRelationDrop(ComputeX cx, Relation rx, bool doDrop)
        {
            if (_relation_ComputeX_QueryX.TryGetChild(cx, out QueryX qx1) && _relation_Store_ComputeX.TryGetParent(cx, out Store st) && !_relation_QueryX_QueryX.HasChildLink(qx1))
            {
                var (head, tail) = rx.GetHeadTail();
                if (st == head || st == tail)
                {
                    if (doDrop) _queryXManager.AddComputeXQueryX(cx, qx1, rx, st == tail);
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region SetComputeType  ===============================================
        internal void SetComputeTypeProperty(ComputeX cx, CompuType type)
        {
            QueryX qh;
            if (!_relation_ComputeX_QueryX.TryGetChild(cx, out qh))
            {
                qh = new QueryX(_queryXManager, QueryType.Value);
                _relation_ComputeX_QueryX.SetLink(cx, qh);
            }
            if (cx.CompuType != type)
            {
                cx.CompuType = type;
                cx.Value.Clear();

                switch (type)
                {
                    case CompuType.RowValue:
                        if (_relation_QueryX_QueryX.TryGetChildren(qh, out IList<QueryX> qxList))
                        {
                            foreach (var qx in qxList.ToArray())
                            {
                                _changeManager.RemoveItem(qx);
                            }
                            cx.ChildDelta++;
                            cx.AutoExpandRight = true;
                        }
                        break;
                    case CompuType.RelatedValue:
                        break;
                    case CompuType.CompositeString:
                        break;
                    case CompuType.CompositeReversed:
                        break;
                }

                AllocateValueCache(cx);
            }
        }
        #endregion

        #region TryGetComputedValue  ==========================================
        internal bool TryGetComputedValue(ComputeX cx, Item key)
        {/*
            This method is called by valueDictionary when a key-value-pair does
            not exist for the callers key.
         */
            if (!_relation_ComputeX_QueryX.TryGetChild(cx, out QueryX qx) || cx.Value.IsEmpty)
                return false;

            switch (cx.CompuType)
            {
                case CompuType.RowValue: return TryGetRowValue();

                case CompuType.RelatedValue: return TryGetRelated();

                case CompuType.CompositeString: return TryGetCompositeString();

                case CompuType.CompositeReversed: return TryGetCompositeReversed();
            }
            return false;

            //= = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =

            bool TryGetRowValue()
            {
                if (!qx.HasValidSelect) return false;
                if (!qx.Select.GetValue(key, out string val)) return false;

                cx.Value.SetValue(key, val);
                return true;
            }

            bool TryGetRelated()
            {
                var selectors = new List<Query>();
                if (!_queryXManager.TryGetForest(cx, key, selectors, out _) || selectors.Count == 0) return false;

                return cx.Value.LoadCache(cx, key, selectors);
            }

            bool TryGetCompositeString(bool reverse = false)
            {
                var selectors = new List<Query>();
                if (!_queryXManager.TryGetForest(cx, key, selectors, out _) || selectors.Count == 0) return false;

                var sb = new StringBuilder(128);

                if (reverse) selectors.Reverse();

                var seperator = cx.Separator;
                if (string.IsNullOrEmpty(seperator)) seperator = null;

                foreach (var q in selectors)
                {
                    if (q.Items == null) continue;
                    var qt = q.Owner;

                    foreach (var k in q.Items)
                    {
                        if (k == null) continue;
                        if (!qt.Select.GetValue(k, out string text)) continue;
                        if (string.IsNullOrEmpty(text)) text = " ? ";
                        if (sb.Length > 0 && seperator != null)
                            sb.Append(seperator);
                        sb.Append(text);
                    }
                }

                return cx.Value.SetValue(key, sb.ToString());
            }

            bool TryGetCompositeReversed()
            {
                return TryGetCompositeString(true);
            }

        }
        #endregion

        #region AllocateValueCache  ===========================================
        // called when the computeX needs to produce a value, but its ValueCache is null
        internal ValType AllocateValueCache(ComputeX cx)
        {
            switch (cx.CompuType)
            {
                case CompuType.RowValue:

                    if (!_relation_ComputeX_QueryX.TryGetChild(cx, out QueryX vx) || vx.Select == null || vx.Select.ValueType == ValType.IsInvalid)
                        cx.Value = Value.ValuesInvalid;
                    else
                        AllocateCache(vx);
                    break;

                case CompuType.RelatedValue:

                    cx.Value = Value.Create(GetRelatedValueType(cx));
                    break;

                case CompuType.CompositeString:

                    cx.Value = Value.Create(ValType.String);
                    break;

                case CompuType.CompositeReversed:

                    cx.Value = Value.Create(ValType.String);
                    break;
            }
            cx.Value.SetOwner(cx);
            return cx.Value.ValType;

            void AllocateCache(QueryX vx)
            {
                var type = vx.Select.ValueType;
                if (type < ValType.MaximumType)
                    cx.Value = Value.Create(type);
                else if (type == ValType.IsUnknown)
                    cx.Value = Value.ValuesUnknown;
                else if (type == ValType.IsCircular)
                    cx.Value = Value.ValuesCircular;
                else if (type == ValType.IsUnresolved)
                    cx.Value = Value.ValuesUnresolved;
                else
                    cx.Value = Value.ValuesInvalid;
            }
        }
        #endregion

        #region GetRelatedValueType  ==========================================
        ValType GetRelatedValueType(ComputeX cx)
        {
            if (!_relation_ComputeX_QueryX.TryGetChild(cx, out QueryX qx))
                return ValType.IsInvalid; //computeX must have a root queryX reference

            if (!_relation_QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> list1))
                return ValType.IsInvalid; //computeX must have atleast one queryX reference

            var workQueue = new Queue<QueryX>(list1);
            var isMultiple = list1.Count > 1;

            var vTypes = new HashSet<ValType>();

            while (workQueue.Count > 0)
            {/*
                deapth first traversal of queryX true
             */
                var qt = workQueue.Dequeue();

                if (_relation_Relation_QueryX.TryGetParent(qt, out Relation r) && (r.Pairing == Pairing.ManyToMany || (!qt.IsReversed && r.Pairing == Pairing.OneToMany)))
                    isMultiple = true;

                if (qt.HasValidSelect && qt.Select.ValueType < ValType.MaximumType)
                {
                    vTypes.Add(qt.Select.ValueType);
                }

                if (_relation_QueryX_QueryX.TryGetChildren(qt, out IList<QueryX> list2))
                {
                    isMultiple |= list2.Count > 1;
                    foreach (var child in list2) { workQueue.Enqueue(child); }
                }
            }

            if (vTypes.Count == 0)
                return ValType.IsInvalid; //computeX must have atleast one valid related value

            var vType = ValType.IsInvalid;
            var vGroup = ValGroup.None;
            foreach (var vt in vTypes)
            {
                vGroup |= Value.GetValGroup(vt); // compose an aggregate of value group bit flags
                if (vType == ValType.IsInvalid) vType = vt; // get the first valType
            }
            if (vGroup == ValGroup.None)
                return ValType.IsInvalid; //computeX must have atleast one valid related value

            if (vGroup.HasFlag(ValGroup.ArrayGroup))
                isMultiple = true;

            if (vTypes.Count == 1)
            {
                if (isMultiple)
                    vType |= ValType.IsArray;
                else
                    vType &= ~ValType.IsArray;
            }
            else
            {
                if (vGroup == ValGroup.DateTime)
                {
                    vType = (isMultiple) ? ValType.DateTimeArray : ValType.DateTime;
                }
                else if (vGroup == ValGroup.DateTimeArray)
                {
                    vType = ValType.DateTimeArray;
                }
                else if (vGroup.HasFlag(ValGroup.DateTime) || vGroup.HasFlag(ValGroup.DateTimeArray))
                {
                    vType = ValType.StringArray;
                }
                else if (vGroup.HasFlag(ValGroup.ScalarGroup) && !vGroup.HasFlag(ValGroup.ArrayGroup))
                {
                    if (vGroup.HasFlag(ValGroup.String))
                        vType = ValType.StringArray;
                    else if (vGroup.HasFlag(ValGroup.Double))
                        vType = ValType.DoubleArray;
                    else if (vGroup.HasFlag(ValGroup.Long))
                        vType = ValType.Int64Array;
                    else if (vGroup.HasFlag(ValGroup.Int))
                        vType = ValType.Int32Array;
                    else if (vGroup.HasFlag(ValGroup.Bool))
                        vType = ValType.BoolArray;
                }
                else
                {
                    vType = ValType.StringArray;
                }
            }

            return vType;
        }
        #endregion

        #region GetSelectorName  ==============================================
        internal string GetSelectClauseTargetName(ComputeX cx)
        {
            return _relation_Store_ComputeX.TryGetParent(cx, out Store tbl) ? tbl.GetNameId() : "Select";
        }

        internal int GetValueType(QueryX qx) => qx.Select is null ? (int)ValType.IsUnknown : (int)qx.Select.ValueType;
        #endregion

        #endregion

        #region ModelHelpers  =================================================


        #endregion
    }
}
