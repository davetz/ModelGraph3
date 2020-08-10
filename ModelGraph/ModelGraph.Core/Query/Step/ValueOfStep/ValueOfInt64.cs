﻿using System;

namespace ModelGraph.Core
{
    internal abstract class ValueOfInt64 : ValueOfStep<long>
    {
        internal override ValType ValType => ValType.Int64;

        internal override bool AsBool() => (GetVal() != 0);
        internal override long AsInt64() => GetVal();
        internal override double AsDouble() => GetVal();
        internal override string AsString() => GetVal().ToString();
        internal override DateTime AsDateTime() => throw new NotImplementedException(); // failed type check
    }
}
