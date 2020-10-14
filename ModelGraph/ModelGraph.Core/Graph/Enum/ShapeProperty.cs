using System;

namespace ModelGraph.Core
{
    [Flags]
    internal enum ShapeProperty
    {
        None = 0,
        Dim = 0x1,
        Aux = 0x2,
        Cent = 0x4,
        Horz = 0x8,
        Vert = 0x10,
        Rad1 = 0x20,
        Rad2 = 0x40,
        EndCap = 0x80,
        DashCap = 0x100,
        StartCap = 0x200,
        LineWidth = 0x400,
        LineStyle = 0x800,
        PolyLocked = 0x1000,
        MultiSizerMask = Dim | Aux | Cent | Rad1 | Rad2 | PolyLocked,
    }
}
