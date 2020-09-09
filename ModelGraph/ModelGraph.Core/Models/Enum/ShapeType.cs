namespace ModelGraph.Core
{
    public enum ShapeType : byte
    {
        //simple shapes 
        Line = 0,
        Circle = 1,
        Ellipse = 2,
        Rectangle = 3,
        RoundedRectangle = 4,
        SimpleShapeMask = 0x07,

        //multiple simple shapes 
        MultipleLines = 8,
        MultipleCircles = 9,
        MultipleEllipses = 10,
        MultipleRectangles = 11,
        MultipleRoundedRectangles = 12,
        MultipleSimpleShapesLimit = 0x0F,

        //connected line segments
        JointedLines = 0x40,
        PolySide = 0x41,
        PolyStar = 0x42,
        PolyGear = 0x43,
        PolySpike = 0x44,
        PolyPulse = 0x45,

        //connected sline segments
        JoinedSplines = 0x80,
        PolyWave = 0x81,
        PolySpring = 0x82,
    }
}
