using System;

namespace ModelGraph.Core
{
    [Flags]
    internal enum ShapeProperty
    {
        None = 0,
        EndCap = 0x1,
        DashCap = 0x2,
        StartCap = 0x4,
        StrokeStyle = 0x8,
        StrokeWidth = 0x10,
        //=================
        SizeX = 0x20,
        SizeY = 0x40,
        //=================
        Radius1 = 0x80,
        Radius2 = 0x100,
        Factor1 = 0x200,
        CenterX = 0x400,
        CenterY = 0x800,
        Rotation1 = 0x1000,
        Rotation2 = 0x2000,
        Dimension = 0x4000,
        IsImpaired = 0x8000,
        //=================
        ExtentEast = 0x10000,
        ExtentWest = 0x20000,
        ExtentNorth = 0x40000,
        ExtentSouth = 0x80000,
    }
}
