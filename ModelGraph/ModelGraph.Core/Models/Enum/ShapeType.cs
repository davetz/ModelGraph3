namespace ModelGraph.Core
{
    public enum ShapeType : byte
    {
        //simple shapes 
        Line = 0,
        Pin2 = 1,
        Pin4 = 2,
        Grip2 = 3,
        Grip4 = 4,
        Circle = 5,
        Ellipse = 6,
        EqualRect = 7,
        CornerRect = 8,
        CenterRect = 9,
        RoundedRect = 10,
        SimpleShapeMask = 0x0F,

        //closed line segments
        ClosedLines = 0x20,
        PolySide = 0x21,
        PolyStar = 0x22,
        PolyGear = 0x23,

        //joined line segments
        JointedLines = 0x40,
        PolySpike = 0x44,
        PolyPulse = 0x45,

        //joined spline segments
        JoinedSplines = 0x80,
        PolyWave = 0x81,
        PolySpring = 0x82,

        Unknown = 0xFF,
    }
}
