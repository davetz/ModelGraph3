﻿using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    internal class DoubleArrayValue : ValueOfArray<double>
    {
        internal override ValType ValType => ValType.DoubleArray;

        internal ValueDictionaryOf<double[]> ValueDictionary => _valueStore as ValueDictionaryOf<double[]>;
        internal override bool IsSpecific(Item key) => _valueStore.IsSpecific(key);

        #region Constructor, WriteData  =======================================
        internal DoubleArrayValue(IValueStore<double[]> store) { _valueStore = store; }

        internal DoubleArrayValue(DataReader r, int count, Item[] items)
        {
            var vs = new ValueDictionaryOf<double[]>(count, default);
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

                    var val = new double[len];
                    if (len > 0)
                    {
                        for (int j = 0; j < len; j++)
                        {
                            val[j] = r.ReadDouble();
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
                            w.WriteDouble(v);
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
            var v = new double[N];
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
            var b = (GetVal(key, out double[] v));
            value = ArrayFormat(v, (i) => ValueFormat(v[i], Format));
            return b;
        }
        internal override bool SetValue(Item key, string value)
        {
            (var ok, double[] v) = ArrayParse(value, (s) => DoubleParse(s));
            return ok ? SetVal(key, v) : false;
        }
        #endregion

        #region GetValueAt  ===================================================
        internal override bool GetValueAt(Item key, out bool value, int index)
        {
            var b = GetValAt(key, out double v, index);
            value = (v != 0);
            return b;
        }

        internal override bool GetValueAt(Item key, out int value, int index)
        {
            var b = (GetValAt(key, out double v, index) && !(v < int.MinValue || v > int.MaxValue));
            value = (int)v;
            return b;
        }

        internal override bool GetValueAt(Item key, out long value, int index)
        {
            var b = (GetValAt(key, out double v, index) && !(v < long.MinValue || v > long.MaxValue));
            value = (long)v;
            return b;
        }

        internal override bool GetValueAt(Item key, out double value, int index)
        {
            var b = GetValAt(key, out double v, index);
            value = v;
            return b;
        }

        internal override bool GetValueAt(Item key, out string value, int index)
        {
            var b = GetValAt(key, out double v, index);
            value = ValueFormat(v, Format);
            return b;
        }
        #endregion

        #region GetLength  ====================================================
        internal override bool GetLength(Item key, out int value)
        {
            if (GetVal(key, out double[] v))
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
            var b = GetVal(key, out double[] v);
            var c = ValueArray(v, out value, (i) => (true, v[i] != 0));
            return b && c;
        }

        internal override bool GetValue(Item key, out int[] value)
        {
            var b = GetVal(key, out double[] v);
            var c = ValueArray(v, out value, (i) => (!(v[i] < int.MinValue || v[i] > int.MaxValue), (int)v[i]));
            return b && c;
        }

        internal override bool GetValue(Item key, out long[] value)
        {
            var b = GetVal(key, out double[] v);
            var c = ValueArray(v, out value, (i) => (!(v[i] < long.MinValue || v[i] > long.MaxValue), (long)v[i]));
            return b && c;
        }

        internal override bool GetValue(Item key, out double[] value)
        {
            var b = GetVal(key, out double[] v);
            var c = ValueArray(v, out value, (i) => (true, (double)v[i]));
            return b && c;
        }

        internal override bool GetValue(Item key, out string[] value)
        {
            var b = GetVal(key, out double[] v);
            var c = ValueArray(v, out value, (i) => ValueFormat(v[i], Format));
            return b && c;
        }

        #endregion

        #region SetValue (array) ==============================================
        internal override bool SetValue(Item key, bool[] value)
        {
            var c = ValueArray(value, out double[] v, (i) => (true, value[i] ? 1 : 0));
            var b = SetVal(key, v);
            return b && c;
        }

        internal override bool SetValue(Item key, int[] value)
        {
            var c = ValueArray(value, out double[] v, (i) => (true, value[i]));
            var b = SetVal(key, v);
            return b && c;
        }

        internal override bool SetValue(Item key, long[] value)
        {
            var c = ValueArray(value, out double[] v, (i) => (true, value[i]));
            var b = SetVal(key, v);
            return b && c;
        }

        internal override bool SetValue(Item key, double[] value) => SetVal(key, value);

        internal override bool SetValue(Item key, string[] value)
        {
            var c = ValueArray(value, out double[] v, (i) => DoubleParse(value[i]));
            var b = SetVal(key, v);
            return b && c;
        }
        #endregion
    }
}
