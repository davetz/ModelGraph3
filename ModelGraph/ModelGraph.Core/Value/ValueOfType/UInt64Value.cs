using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    internal class UInt64Value : ValueOfType<ulong>
    {
        internal override ValType ValType => ValType.UInt64;

        internal ValueDictionaryOf<ulong> ValueDictionary => _valueStore as ValueDictionaryOf<ulong>;
        internal override bool IsSpecific(Item key) => _valueStore.IsSpecific(key);

        #region Constructor, WriteData  =======================================
        internal UInt64Value(IValueStore<ulong> store) { _valueStore = store; }

        internal UInt64Value(DataReader r, int count, Item[] items)
        {
            if (count == 0)
            {
                _valueStore = new ValueDictionaryOf<ulong>(count, default);
            }
            else
            {
                var vs = new ValueDictionaryOf<ulong>(count, r.ReadUInt64());
                _valueStore = vs;

                for (int i = 0; i < count; i++)
                {
                    var inx = r.ReadInt32();
                    if (inx < 0 || inx >= items.Length) throw new Exception($"Invalid row index {inx}");

                    var rx = items[inx];
                    if (rx == null) throw new Exception($"Column row is null, index {inx}");

                    vs.LoadValue(rx, r.ReadUInt64());
                }
            }
        }
        internal override void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            w.WriteByte((byte)ValType);

            var vd = ValueDictionary;
            var N = vd.Count;
            w.WriteInt32(N);

            if (N > 0)
            {
                w.WriteUInt64(vd.DefaultValue);

                var keys = vd.GetKeys();
                var vals = vd.GetValues();

                for (int i = 0; i < N; i++)
                {
                    var key = keys[i];
                    var val = vals[i];

                    w.WriteInt32(itemIndex[key]);
                    w.WriteUInt64(val);
                }
            }
        }
        #endregion

        #region LoadCache  ====================================================
        internal override bool LoadCache(ComputeX cx, Item key, List<Query> qList)
        {
            if (cx == null || qList == null || qList.Count == 0) return false;

            var q = qList[0];
            if (q.Items == null || q.Items.Length == 0) return false;

            var qx = q.QueryX;
            if (!qx.HasSelect) return false;

            var k = q.Items[0];
            if (k == null) return false;

            return (qx.Select.GetValue(k, out Int64 v)) ? SetValue(key, v) : false;
        }
        #endregion

        #region GetValue  =====================================================
        internal override bool GetValue(Item key, out bool value)
        {
            var b = GetVal(key, out ulong v);
            value = (v != 0);
            return b;
        }

        internal override bool GetValue(Item key, out int value)
        {
            var b = (GetVal(key, out ulong v) && !(v > int.MaxValue));
            value = (int)v;
            return b;
        }

        internal override bool GetValue(Item key, out Int64 value)
        {
            var b = GetVal(key, out ulong v);
            value = (Int64)v;
            return b;
        }
    internal override bool GetValue(Item key, out double value)
        {
            var b = GetVal(key, out ulong v);
            value = v;
            return b;
        }

        internal override bool GetValue(Item key, out string value)
        {
            var b = GetVal(key, out ulong v);
            value = ValueFormat(v, Format);
            return b;
        }
        #endregion

        #region PseudoArrayValue  =============================================
        internal override bool GetValue(Item key, out bool[] value)
        {
            var b = GetValue(key, out bool v);
            value = new bool[] { v };
            return b;
        }

        internal override bool GetValue(Item key, out int[] value)
        {
            var b = GetValue(key, out int v);
            value = new int[] { v };
            return b;
        }

        internal override bool GetValue(Item key, out Int64[] value)
        {
            var b = GetValue(key, out Int64 v);
            value = new Int64[] { v };
            return b;
        }

        internal override bool GetValue(Item key, out double[] value)
        {
            var b = GetValue(key, out double v);
            value = new double[] { v };
            return b;
        }

        internal override bool GetValue(Item key, out string[] value)
        {
            var b = GetValue(key, out string v);
            value = new string[] { v };
            return b;
        }
        internal override bool GetValue(Item key, out DateTime[] value)
        {
            var b = GetValue(key, out DateTime v);
            value = new DateTime[] { v };
            return b;
        }
        #endregion

        #region SetValue ======================================================
        internal override bool SetValue(Item key, bool value) => SetVal(key, (ulong)(value ? 1 : 0));

        internal override bool SetValue(Item key, int value) => SetVal(key, (ulong)value);

        internal override bool SetValue(Item key, Int64 value) => SetVal(key, (ulong)value);

        internal override bool SetValue(Item key, double value) => (value < ulong.MinValue || value > ulong.MaxValue) ? false : SetVal(key, (ulong)value);

        internal override bool SetValue(Item key, string value)
        {
            var (ok, val) = UInt64Parse(value);
            return (ok) ? SetVal(key, val) : false;
        }
        #endregion
    }
}
