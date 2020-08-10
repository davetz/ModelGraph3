using System;

namespace ModelGraph.Core
{
    internal abstract class ValueOfInt32 : ValueOfStep<int>
    {
        internal override ValType ValType => ValType.Int32;

        internal override bool AsBool() => (GetVal() != 0);
        internal override long AsInt64() => GetVal();
        internal override double AsDouble() => GetVal();
        internal override string AsString() => GetVal().ToString();
        internal override DateTime AsDateTime() => throw new NotImplementedException(); // failed type check
    }
}
