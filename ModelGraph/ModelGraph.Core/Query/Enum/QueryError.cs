using System;

namespace ModelGraph.Core
{
    /// <summary>QueryX validation error</summary>
    [Flags]
    public enum QueryError : byte //validation state
    {
        None = 0,

        IsInvalid = 0x1,  // can't even determine what the queryState is
        ParseError = 0x2,  // didn't get past the expression string parse phase
        InvalidRef = 0x4,   // encountered a computeX with ValueType.IsInvalid
        CircularRef = 0x8,   // encountered a computeX with ValueType.IsCircular
        UnresolvedRef = 0x10,  // encountered a computeX with ValueType.IsUnresolved

        ErrorsMask = 0x7F,
        IsValidQuery = 0x80, 
    }
}
