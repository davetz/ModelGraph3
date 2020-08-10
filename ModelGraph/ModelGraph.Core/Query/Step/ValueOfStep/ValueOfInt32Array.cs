using System;

namespace ModelGraph.Core
{
    internal abstract class ValueOfInt32Array : ValueOfStep<int[]>
    {
        internal override ValType ValType => ValType.Int32Array;

        internal override int[] AsInt32Array() => GetVal();
        internal override int AsLength() => AsInt32Array().Length;
        internal override string AsString()
        {
            var v = GetVal();
            return Value.ArrayFormat(v, (i) => Value.ValueFormat(v[i], FormatType.IsInt));
        }
    }
}
