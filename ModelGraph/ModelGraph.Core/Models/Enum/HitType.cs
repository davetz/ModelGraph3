using System;

namespace ModelGraph.Core
{
    [Flags]
    internal enum HitType
    {
        None = 0,
        Pin = 0x01,
        Node = 0x02,
        Region = 0x04,
    }
}
