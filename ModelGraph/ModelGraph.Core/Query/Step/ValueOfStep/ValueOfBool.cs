using System;

namespace ModelGraph.Core
{
    internal abstract class ValueOfBool : ValueOfStep<bool>
    {
        internal override ValType ValType => ValType.Bool;

        internal override bool AsBool() => GetVal();
        internal override int AsInt32() => GetVal() ? 1 : 0;
        internal override long AsInt64() => GetVal() ? 1 : 0;
        internal override double AsDouble() => GetVal() ? 1 : 0;
        internal override string AsString() => GetVal() ? "True" : "False";
        internal override DateTime AsDateTime() => throw new NotImplementedException(); // failed type check
    }
}
