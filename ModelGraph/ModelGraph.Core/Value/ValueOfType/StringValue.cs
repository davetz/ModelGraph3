using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    internal class StringValue : ValueOfType<string>
    {
        internal override ValType ValType => ValType.String;

        internal ValueDictionaryOf<string> ValueDictionary => _valueStore as ValueDictionaryOf<string>;
        internal override bool IsSpecific(Item key) => _valueStore.IsSpecific(key);

        #region Constructor, WriteData  =======================================
        internal StringValue(IValueStore<string> store) { _valueStore = store; }

        internal StringValue(DataReader r, int count, Item[] items)
        {
            if (count == 0)
            {
                _valueStore = new ValueDictionaryOf<string>(count, default);
            }
            else
            {
                var vs = new ValueDictionaryOf<string>(count, ReadString(r));
                _valueStore = vs;

                for (int i = 0; i < count; i++)
                {
                    var inx = r.ReadInt32();
                    if (inx < 0 || inx >= items.Length) throw new Exception($"Invalid row index {inx}");

                    var rx = items[inx];
                    if (rx == null) throw new Exception($"Column row is null, index {inx}");

                    vs.LoadValue(rx, ReadString(r));
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
                WriteString(w, vd.DefaultValue);

                var keys = vd.GetKeys();
                var vals = vd.GetValues();

                for (int i = 0; i < N; i++)
                {
                    var key = keys[i];
                    var val = vals[i];

                    w.WriteInt32(itemIndex[key]);
                    WriteString(w, val);
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

            return (qx.Select.GetValue(k, out string v)) ? SetValue(key, v) : false;
        }
        #endregion

        #region GetValue  =====================================================
        internal override bool GetValue(Item key, out bool value) => (GetVal(key, out string v) && bool.TryParse(v, out value)) ? true : NoValue(out value);

        internal override bool GetValue(Item key, out int value) => (GetVal(key, out string v) && int.TryParse(v, out value)) ? true : NoValue(out value);

        internal override bool GetValue(Item key, out Int64 value) => (GetVal(key, out string v) && Int64.TryParse(v, out value)) ? true : NoValue(out value);

        internal override bool GetValue(Item key, out double value) => (GetVal(key, out string v) && double.TryParse(v, out value)) ? true : NoValue(out value);

        internal override bool GetValue(Item key, out DateTime value) => (GetVal(key, out string v) && DateTime.TryParse(v, out value)) ? true : NoValue(out value);

        internal override bool GetValue(Item key, out string value) => GetVal(key, out value);
        #endregion

        #region GetLength  ====================================================
        internal override bool GetLength(Item key, out int value)
        {
            if (GetVal(key, out string v))
            {
                value = v.Length;
                return true;
            }
            value = 0;
            return false;
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
        internal override bool SetValue(Item key, bool value) => SetVal(key, value.ToString());

        internal override bool SetValue(Item key, int value) => SetVal(key, value.ToString());

        internal override bool SetValue(Item key, Int64 value) => SetVal(key, value.ToString());

        internal override bool SetValue(Item key, double value) => SetVal(key, value.ToString());

        internal override bool SetValue(Item key, DateTime value) => SetVal(key, value.ToString());

        internal override bool SetValue(Item key, string value) => SetVal(key, value);
        #endregion
    }
}
