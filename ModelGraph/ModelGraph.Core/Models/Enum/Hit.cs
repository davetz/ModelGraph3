﻿using System;

namespace ModelGraph.Core
{
    [Flags]
    internal enum Hit
    {
        ZZZ = 0,
        Pin = 0x01,
        Node = 0x02,
        Edge = 0x04,
        Region = 0x08,
    }
}