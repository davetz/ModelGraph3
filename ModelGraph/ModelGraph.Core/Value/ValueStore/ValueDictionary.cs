using System;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    internal class ValueDictionary
    {
        internal static void ReadData(DataReader r, ColumnX cx, Item[] items)
        {
            var t = r.ReadByte();
            if (t > _createValue.Length) throw new ArgumentException("Invalid Value Type");

            var count = r.ReadInt32();
            if (count < 0) throw new Exception($"Invalid row count {count}");

            cx.Value = _createValue[t](r, count, items);
        }

        #region _readValueDictionary  =========================================
        static Func<DataReader, int, Item[], Value>[] _createValue =
        {
            (r, c, i) => new BoolValue(r, c, i),           //  0
            (r, c, i) => new BoolArrayValue(r, c, i),      //  1
            (r, c, i) => new CharValue(r, c, i),           //  2
            (r, c, i) => new CharArrayValue(r, c, i),      //  3
            (r, c, i) => new ByteValue(r, c, i),           //  4
            (r, c, i) => new ByteArrayValue(r, c, i),      //  5
            (r, c, i) => new SByteValue(r, c, i),          //  6
            (r, c, i) => new SByteArrayValue(r, c, i),     //  7
            (r, c, i) => new Int16Value(r, c, i),          //  8
            (r, c, i) => new Int16ArrayValue(r, c, i),     //  9
            (r, c, i) => new UInt16Value(r, c, i),         // 10
            (r, c, i) => new UInt16ArrayValue(r, c, i),    // 11
            (r, c, i) => new Int32Value(r, c, i),          // 12
            (r, c, i) => new Int32ArrayValue(r, c, i),     // 13
            (r, c, i) => new UInt32Value(r, c, i),         // 14
            (r, c, i) => new UInt32ArrayValue(r, c, i),    // 15
            (r, c, i) => new Int64Value(r, c, i),          // 16
            (r, c, i) => new Int64ArrayValue(r, c, i),     // 17
            (r, c, i) => new UInt64Value(r, c, i),         // 18
            (r, c, i) => new UInt64ArrayValue(r, c, i),    // 19
            (r, c, i) => new SingleValue(r, c, i),         // 20
            (r, c, i) => new SingleArrayValue(r, c, i),    // 21
            (r, c, i) => new DoubleValue(r, c, i),         // 22
            (r, c, i) => new DoubleArrayValue(r, c, i),    // 23
            (r, c, i) => new DecimalValue(r, c, i),        // 24
            (r, c, i) => new DecimalArrayValue(r, c, i),   // 25
            (r, c, i) => new DateTimeValue(r, c, i),       // 26
            (r, c, i) => new DateTimeArrayValue(r, c, i),  // 27
            (r, c, i) => new StringValue(r, c, i),         // 28
            (r, c, i) => new StringArrayValue(r, c, i),    // 29
        };
        #endregion
    }
}
