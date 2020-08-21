using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    internal class StringArrayValue : ValueOfArray<string>
    {
        internal override ValType ValType => ValType.StringArray;

        internal ValueDictionaryOf<string[]> ValueDictionary => _valueStore as ValueDictionaryOf<string[]>;
        internal override bool IsSpecific(Item key) => _valueStore.IsSpecific(key);

        #region Constructor, WriteData  =======================================
        internal StringArrayValue(IValueStore<string[]> store) { _valueStore = store; }

        internal StringArrayValue(DataReader r, int count, Item[] items)
        {
            var vs = new ValueDictionaryOf<string[]>(count, default);
            _valueStore = vs;

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var inx = r.ReadInt32();
                    if (inx < 0 || inx >= items.Length) throw new Exception($"Invalid row index {inx}");

                    var rx = items[inx];
                    if (rx == null) throw new Exception($"Column row is null, index {inx}");

                    var len = r.ReadUInt16();

                    var val = new string[len];
                    if (len > 0)
                    {
                        for (int j = 0; j < len; j++)
                        {
                            val[j] = ReadString(r);
                        }
                    }
                    vs.LoadValue(rx, val);
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
                var keys = vd.GetKeys();
                var vals = vd.GetValues();

                for (int i = 0; i < N; i++)
                {
                    var key = keys[i];
                    w.WriteInt32(itemIndex[key]);

                    var val = vals[i];
                    var len = (val is null) ? 0 : val.Length > ushort.MaxValue ? ushort.MaxValue : val.Length;
                    w.WriteUInt16((ushort)len);

                    if (len > 0)
                    {
                        foreach (var v in val)
                        {
                            WriteString(w, v);
                        }
                    }
                }
            }
        }
        #endregion

        #region LoadCache  ====================================================
        internal override bool LoadCache(ComputeX cx, Item key, List<Query> qList)
        {
            if (cx == null || qList == null || qList.Count == 0) return false;

            var N = 0;
            foreach (var q in qList)
            {
                if (q.Items == null) continue;
                var qx = q.Owner;
                if (!qx.HasSelect) continue;
                foreach (var k in q.Items) { if (k != null) N++; }
            }
            var v = new string[N];
            var i = 0;

            foreach (var q in qList)
            {
                if (q.Items == null) continue;
                var qx = q.Owner;
                if (!qx.HasSelect) continue;
                foreach (var k in q.Items)
                {
                    if (k != null)
                    {
                        qx.Select.GetValue(k, out v[i++]);
                    }
                }
            }
            return SetValue(key, v);
        }
        #endregion

        #region Required  =====================================================
        internal override bool GetValue(Item key, out string value)
        {
            var b = (GetVal(key, out string[] v));
            value = ArrayFormat(v, (i) => v[i]);
            return b;
        }
        internal override bool SetValue(Item key, string value)
        {
            (var ok, string[] v) = ArrayParse(value, (s) => (true, s));
            return ok ? SetVal(key, v) : false;
        }
        #endregion

        #region GetValueAt  ===================================================
        internal override bool GetValueAt(Item key, out bool value, int index)
        {
            var b = GetValAt(key, out string v, index);
            (bool c, bool val) = BoolParse(v);
            value = val;
            return (b && c);
        }

        internal override bool GetValueAt(Item key, out int value, int index)
        {
            var b = GetValAt(key, out string v, index);
            (bool c, int val) = Int32Parse(v);
            value = val;
            return (b && c);
        }

        internal override bool GetValueAt(Item key, out long value, int index)
        {
            var b = GetValAt(key, out string v, index);
            (bool c, long val) = Int64Parse(v);
            value = val;
            return (b && c);
        }

        internal override bool GetValueAt(Item key, out double value, int index)
        {
            var b = GetValAt(key, out string v, index);
            (bool c, double val) = DoubleParse(v);
            value = val;
            return (b && c);
        }

        internal override bool GetValueAt(Item key, out DateTime value, int index)
        {
            var b = GetValAt(key, out string v, index);
            (bool c, DateTime val) = DateTimeParse(v);
            value = val;
            return (b && c);
        }

        internal override bool GetValueAt(Item key, out string value, int index) => GetValAt(key, out value, index);
        #endregion

        #region GetLength  ====================================================
        internal override bool GetLength(Item key, out int value)
        {
            if (GetVal(key, out string[] v))
            {
                value = v.Length;
                return true;
            }
            value = 0;
            return false;
        }
        #endregion

        #region GetValue (array)  =============================================
        internal override bool GetValue(Item key, out bool[] value)
        {
            var b = GetVal(key, out string[] v);
            var c = ValueArray(v, out value, (i) => BoolParse(v[i]));
            return (b && c);
        }

        internal override bool GetValue(Item key, out int[] value)
        {
            var b = GetVal(key, out string[] v);
            var c = ValueArray(v, out value, (i) => Int32Parse(v[i]));
            return (b && c);
        }

        internal override bool GetValue(Item key, out long[] value)
        {
            var b = GetVal(key, out string[] v);
            var c = ValueArray(v, out value, (i) => Int64Parse(v[i]));
            return (b && c);
        }

        internal override bool GetValue(Item key, out double[] value)
        {
            var b = GetVal(key, out string[] v);
            var c = ValueArray(v, out value, (i) => DoubleParse(v[i]));
            return (b && c);
        }

        internal override bool GetValue(Item key, out DateTime[] value)
        {
            var b = GetVal(key, out string[] v);
            var c = ValueArray(v, out value, (i) => DateTimeParse(v[i]));
            return (b && c);
        }

        internal override bool GetValue(Item key, out string[] value) => GetVal(key, out value);
        #endregion

        #region SetValue (array) ==============================================
        internal override bool SetValue(Item key, bool[] value)
        {
            var c = ValueArray(value, out string[] v, (i) => value[i].ToString());
            var b = SetVal(key, v);
            return b && c;
        }
        internal override bool SetValue(Item key, int[] value)
        {
            var c = ValueArray(value, out string[] v, (i) => value[i].ToString());
            var b = SetVal(key, v);
            return b && c;
        }
        internal override bool SetValue(Item key, long[] value)
        {
            var c = ValueArray(value, out string[] v, (i) => value[i].ToString());
            var b = SetVal(key, v);
            return b && c;
        }

        internal override bool SetValue(Item key, double[] value)
        {
            var c = ValueArray(value, out string[] v, (i) => value[i].ToString());
            var b = SetVal(key, v);
            return b && c;
        }

        internal override bool SetValue(Item key, DateTime[] value)
        {
            var c = ValueArray(value, out string[] v, (i) => value[i].ToString());
            var b = SetVal(key, v);
            return b && c;
        }

        internal override bool SetValue(Item key, string[] value) => SetVal(key, value);
        #endregion
    }
}
