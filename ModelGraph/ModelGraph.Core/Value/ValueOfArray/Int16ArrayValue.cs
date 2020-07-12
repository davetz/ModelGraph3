﻿using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    internal class Int16ArrayValue : ValueOfArray<short>
    {
        internal override ValType ValType => ValType.Int16Array;

        internal ValueDictionaryOf<short[]> ValueDictionary => _valueStore as ValueDictionaryOf<short[]>;
        internal override bool IsSpecific(Item key) => _valueStore.IsSpecific(key);

        #region Constructor, WriteData  =======================================
        internal Int16ArrayValue(IValueStore<short[]> store) { _valueStore = store; }

        internal Int16ArrayValue(DataReader r, int count, Item[] items)
        {
            var vs = new ValueDictionaryOf<short[]>(count, default);
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

                    var val = new short[len];
                    if (len > 0)
                    {
                        for (int j = 0; j < len; j++)
                        {
                            val[j] = r.ReadInt16();
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
                            w.WriteInt16(v);
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
                var qx = q.QueryX;
                if (!qx.HasSelect) continue;
                foreach (var k in q.Items) { if (k != null) N++; }
            }
            var v = new Int64[N];
            var i = 0;

            foreach (var q in qList)
            {
                if (q.Items == null) continue;
                var qx = q.QueryX;
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
            var b = (GetVal(key, out short[] v));
            value = ArrayFormat(v, (i) => ValueFormat(v[i], Format));
            return b;
        }
        internal override bool SetValue(Item key, string value)
        {
            (var ok, short[] v) = ArrayParse(value, (s) => Int16Parse(s));
            return ok ? SetVal(key, v) : false;
        }
        #endregion

        #region GetValueAt  ===================================================
        internal override bool GetValueAt(Item key, out bool value, int index)
        {
            var b = GetValAt(key, out short v, index);
            value = (v != 0);
            return b;
        }

        internal override bool GetValueAt(Item key, out int value, int index)
        {
            var b = GetValAt(key, out short v, index);
            value = v;
            return b;
        }

        internal override bool GetValueAt(Item key, out Int64 value, int index)
        {
            var b = GetValAt(key, out short v, index);
            value = v;
            return b;
        }

        internal override bool GetValueAt(Item key, out double value, int index)
        {
            var b = GetValAt(key, out short v, index);
            value = v;
            return b;
        }

        internal override bool GetValueAt(Item key, out string value, int index)
        {
            var b = GetValAt(key, out short v, index);
            value = ValueFormat(v, Format);
            return b;
        }
        #endregion

        #region GetLength  ====================================================
        internal override bool GetLength(Item key, out int value)
        {
            if (GetVal(key, out short[] v))
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
            var b = GetVal(key, out short[] v);
            var c = ValueArray(v, out value, (i) => (true, v[i] != 0));
            return b && c;
        }

        internal override bool GetValue(Item key, out int[] value)
        {
            var b = GetVal(key, out short[] v);
            var c = ValueArray(v, out value, (i) => (true, v[i]));
            return b && c;
        }

        internal override bool GetValue(Item key, out Int64[] value)
        {
            var b = GetVal(key, out short[] v);
            var c = ValueArray(v, out value, (i) => (true, v[i]));
            return b && c;
        }

        internal override bool GetValue(Item key, out double[] value)
        {
            var b = GetVal(key, out short[] v);
            var c = ValueArray(v, out value, (i) => (true, v[i]));
            return b && c;
        }

        internal override bool GetValue(Item key, out string[] value)
        {
            var b = GetVal(key, out short[] v);
            var c = ValueArray(v, out value, (i) => ValueFormat(v[i], Format));
            return b && c;
        }

        #endregion

        #region SetValue (array) ==============================================
        internal override bool SetValue(Item key, bool[] value)
        {
            var c = ValueArray(value, out short[] v, (i) => (true, (short)(value[i] ? 1 : 0)));
            var b = SetVal(key, v);
            return b && c;
        }

        internal override bool SetValue(Item key, int[] value)
        {
            var c = ValueArray(value, out short[] v, (i) => (!(value[i] < short.MinValue || value[i] > short.MaxValue), (short)value[i]));
            var b = SetVal(key, v);
            return b && c;
        }

        internal override bool SetValue(Item key, Int64[] value)
        {
            var c = ValueArray(value, out short[] v, (i) => (!(value[i] < short.MinValue || value[i] > short.MaxValue), (short)value[i]));
            var b = SetVal(key, v);
            return b && c;
        }

        internal override bool SetValue(Item key, double[] value)
        {
            var c = ValueArray(value, out short[] v, (i) => (!(value[i] < short.MinValue || value[i] > short.MaxValue), (short)value[i]));
            var b = SetVal(key, v);
            return b && c;
        }

        internal override bool SetValue(Item key, string[] value)
        {
            var c = ValueArray(value, out short[] v, (i) => Int16Parse(value[i]));
            var b = SetVal(key, v);
            return b && c;
        }
        #endregion
    }
}
