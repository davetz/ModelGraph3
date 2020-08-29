namespace ModelGraph.Core
{
    public enum Shape : byte
    {
        //simple shapes 
        Line = 0,
        Circle = 1,
        Ellipse = 2,
        Rectangle = 4,
        RoundedRectangle = 5,

        //more complex geometry
        JointedLines = 6,
        JoinedSplines = 7,

        IsMultipleInstance = 0x10,
        MultipleInstanceMask = 0x0F,
    }
}
