using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    internal abstract partial class Value
    {
        internal virtual void WriteData(DataWriter w, Dictionary<Item, int> itemIndex) { }
        internal virtual ValType ValType => ValType.IsUnknown;
        internal virtual void SetOwner(ComputeX cx) { }
        internal bool IsEmpty => ValType > ValType.MaximumType;

        internal abstract int Count { get; }
        internal abstract bool IsSpecific(Item key);
        internal abstract void Clear();
        internal abstract void Remove(Item key);

        //= = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        // the following used by the UI to get/set property values
        //= = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        internal bool SetBool(Item key, bool value) => SetValue(key, value);
        internal bool SetString(Item key, string value) => SetValue(key, value);

        internal bool GetBool(Item key) { GetValue(key, out bool v); return v; }
        internal string GetString(Item key) { GetValue(key, out string v); return v; }

        //= = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        // the following used to transfer query results to the computeX's cache value
        //= = = = = = = = = = = = = = = = = = = = = = = = = = = = = =

        internal virtual bool LoadCache(ComputeX cx, Item key, List<Query> q) => false;

        //= = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        // the following used to get the inputs for computed values
        //= = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        // set-1
        internal virtual bool GetLength(Item key, out int value) => NoValue(out value);
        internal virtual bool GetValue(Item key, out bool value) => NoValue(out value);

        internal virtual bool GetValue(Item key, out int value) => NoValue(out value);
        internal virtual bool GetValue(Item key, out long value) => NoValue(out value);
        internal virtual bool GetValue(Item key, out double value) => NoValue(out value);

        internal virtual bool GetValue(Item key, out DateTime value) => NoValue(out value);
        internal abstract bool GetValue(Item key, out string value); // <===================== required method !!

        //= = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        // set-2
        internal virtual bool GetValueAt(Item key, out bool value, int index) => NoValue(out value);

        internal virtual bool GetValueAt(Item key, out int value, int index) => NoValue(out value);
        internal virtual bool GetValueAt(Item key, out long value, int index) => NoValue(out value);
        internal virtual bool GetValueAt(Item key, out double value, int index) => NoValue(out value);

        internal virtual bool GetValueAt(Item key, out DateTime value, int index) => NoValue(out value);
        internal virtual bool GetValueAt(Item key, out string value, int index) => NoValue(out value);

        //= = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        // set-3
        internal virtual bool GetValue(Item key, out bool[] value) => NoValue(out value);

        internal virtual bool GetValue(Item key, out int[] value) => NoValue(out value);
        internal virtual bool GetValue(Item key, out long[] value) => NoValue(out value);
        internal virtual bool GetValue(Item key, out double[] value) => NoValue(out value);

        internal virtual bool GetValue(Item key, out DateTime[] value) => NoValue(out value);
        internal virtual bool GetValue(Item key, out string[] value) => NoValue(out value);

        //= = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        // the following used to store the results of computed values
        //= = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        // set-1
        internal virtual bool SetValue(Item key, bool value) => false;

        internal virtual bool SetValue(Item key, int value) => false;
        internal virtual bool SetValue(Item key, long value) => false;
        internal virtual bool SetValue(Item key, double value) => false;

        internal virtual bool SetValue(Item key, DateTime value) => false;
        internal abstract bool SetValue(Item key, string value); // <===================== required method !!

        //= = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        // set-2
        internal virtual bool SetValue(Item key, bool[] value) => false;

        internal virtual bool SetValue(Item key, int[] value) => false;
        internal virtual bool SetValue(Item key, long[] value) => false;
        internal virtual bool SetValue(Item key, double[] value) => false;

        internal virtual bool SetValue(Item key, DateTime[] value) => false;
        internal virtual bool SetValue(Item key, string[] value) => false;

        internal bool NoValue<T>(out T value) { value = default; return false; }

        #region Read String/Bytes  ============================================
        internal static string ReadString(DataReader r)
        {
            var len = (UInt32)r.ReadUInt16();
            var str = r.ReadString(len);
            return (str == "^") ? string.Empty : str;
        }
        internal static byte[] ReadBytes(DataReader r)
        {
            var len = r.ReadInt32();
            var data = new byte[len];
            for (int i = 0; i < len; i++)
            {
                data[i] = r.ReadByte();
            }
            return data;
        }
        #endregion

        #region Write String/Bytes  ===========================================
        internal static void WriteString(DataWriter w, string str)
        {
            var txt = str ?? string.Empty;
            if (txt.Length == 0) txt = "^";


            var len = w.MeasureString(txt);
            if (len > UInt16.MaxValue)
            {
                var r = (double)len / (double)UInt16.MaxValue;
                var n = (UInt16)((txt.Length / r) - 2);
                var trucated = txt.Substring(0, n);
                w.WriteUInt16((UInt16)w.MeasureString(trucated));
                w.WriteString(trucated);
            }
            else
            {
                w.WriteUInt16((UInt16)len);
                w.WriteString(txt);
            }
        }
        internal static void WriteBytes(DataWriter w, byte[] data)
        {
            var len = data.Length;
            w.WriteInt32(len);
            foreach (var b in data)
            {
                w.WriteByte(b);
            }
        }

        #endregion
    }
}
