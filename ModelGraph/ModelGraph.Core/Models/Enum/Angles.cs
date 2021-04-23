using System;

namespace ModelGraph.Core
{
    internal enum Angles
    {
        L90 = 0x1,
        R90 = 0x2,
        L45 = 0x4,
        R45 = 0x8,
        L30 = 0x10,
        R30 = 0x20,
        L22 = 0x40,
        R22 = 0x80,
        LR9045 = L90 | R90 | L45 | R45,
        LR3022 = L30 | R30 | L22 | R22,
        ALL = LR9045 | LR3022,
    }
}
