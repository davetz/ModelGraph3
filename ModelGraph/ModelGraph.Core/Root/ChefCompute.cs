﻿using System.Collections.Generic;
using System.Text;

namespace ModelGraph.Core
{/*

 */
    public partial class Root
    {
        static internal ValueUnknown ValuesUnknown = new ValueUnknown();
        static internal ValueInvalid ValuesInvalid = new ValueInvalid();
        static internal ValueCircular ValuesCircular = new ValueCircular();
        static internal ValueUnresolved ValuesUnresolved = new ValueUnresolved();
        static internal LiteralUnresolved LiteralUnresolved = new LiteralUnresolved();

        #region ResetCacheValues  =============================================
        private void ResetCacheValues()
        {
            var items = Get<ComputeXRoot>().Items;
            foreach (var cx in items) { cx.Value.Clear(); }
        }
        #endregion 

        #region <Get/Set>SelectString  ========================================
        internal string GetWhereProperty(ComputeX cx)
        {
            if (!Get<Relation_ComputeX_QueryX>().TryGetChild(cx, out QueryX qx)) return InvalidItem;

            return (qx.HasWhere) ? qx.WhereString : null;
        }
        internal bool TrySetWhereProperty(ComputeX cx, string value)
        {
            if (!Get<Relation_ComputeX_QueryX>().TryGetChild(cx, out QueryX qx)) return false;

            return TrySetWhereProperty(qx, value);
        }
        internal string GetSelectProperty(ComputeX cx)
        {
            if (!Get<Relation_ComputeX_QueryX>().TryGetChild(cx, out QueryX qx)) return InvalidItem;

            return (qx.HasSelect) ? qx.SelectString : null;
        }
        internal bool TrySetSelectProperty(ComputeX cx, string value)
        {
            if (!Get<Relation_ComputeX_QueryX>().TryGetChild(cx, out QueryX qx)) return false;

            return TrySetSelectProperty(qx, value);
        }
        #endregion

        #region TrySet...Property  ============================================
        internal bool TrySetComputeTypeProperty(ComputeX cx, int val)
        {
            var type = (CompuType)val;
            if (cx.CompuType != type)
            {
                cx.CompuType = type;
                cx.Value.Clear();
                AllocateValueCache(cx);
            }
            return true;
        }
        #endregion

        #region TryGetComputedValue  ==========================================
        internal bool TryGetComputedValue(ComputeX cx, Item key)
        {/*
            This method is called by valueDictionary when a key-value-pair does
            not exist for the callers key.
         */
            if (!Get<Relation_ComputeX_QueryX>().TryGetChild(cx, out QueryX qx) || cx.Value.IsEmpty)
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
                if (!TryGetForest(cx, key, selectors, out Query[] forest) || selectors.Count == 0) return false;

                return cx.Value.LoadCache(cx, key, selectors);
            }

            bool TryGetCompositeString(bool reverse = false)
            {
                var selectors = new List<Query>();
                if (!TryGetForest(cx, key, selectors, out _) || selectors.Count == 0) return false;

                var sb = new StringBuilder(128);

                if (reverse) selectors.Reverse();

                var seperator = cx.Separator;
                if (string.IsNullOrEmpty(seperator)) seperator = null;

                foreach (var q in selectors)
                {
                    if (q.Items == null) continue;
                    var qt = q.QueryX;

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

                    if (!Get<Relation_ComputeX_QueryX>().TryGetChild(cx, out QueryX vx) || vx.Select == null || vx.Select.ValueType == ValType.IsInvalid)
                        cx.Value = ValuesInvalid;
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
                    cx.Value = ValuesUnknown;
                else if (type == ValType.IsCircular)
                    cx.Value = ValuesCircular;
                else if (type == ValType.IsUnresolved)
                    cx.Value = ValuesUnresolved;
                else
                    cx.Value = ValuesInvalid;
            }
        }
        #endregion

        #region GetRelatedValueType  ==========================================
        ValType GetRelatedValueType(ComputeX cx)
        {
            var relation_QueryX_QueryX = Get<Relation_QueryX_QueryX>();
            var relation_Relation_QueryX = Get<Relation_Relation_QueryX>();

            if (!Get<Relation_ComputeX_QueryX>().TryGetChild(cx, out QueryX qx))
                return ValType.IsInvalid; //computeX must have a root queryX reference

            if (!relation_QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> list1))
                return ValType.IsInvalid; //computeX must have atleast one queryX reference

            var workQueue = new Queue<QueryX>(list1);
            var isMultiple = list1.Count > 1;

            var vTypes = new HashSet<ValType>();

            while(workQueue.Count > 0)
            {/*
                deapth first traversal of queryX true
             */
                var qt = workQueue.Dequeue();

                if (relation_Relation_QueryX.TryGetParent(qt, out Relation r) && (r.Pairing == Pairing.ManyToMany || (!qt.IsReversed && r.Pairing == Pairing.OneToMany)))
                    isMultiple = true;

                if (qt.HasValidSelect && qt.Select.ValueType < ValType.MaximumType)
                {
                    vTypes.Add(qt.Select.ValueType);
                }

                if (relation_QueryX_QueryX.TryGetChildren(qt, out IList<QueryX> list2))
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
                if (vGroup == ValGroup.DateTime )
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
        internal string GetSelectorName(ComputeX item)
        {
            return Get<Relation_Store_ComputeX>().TryGetParent(item, out Store tbl) ? tbl.GetNameId() : "Select";
        }
        internal int GetValueType(QueryX qx)
        {
            if (qx.Select == null) return (int)ValType.IsUnknown;
            return (int)qx.Select.ValueType;
        }
        #endregion
    }
}
