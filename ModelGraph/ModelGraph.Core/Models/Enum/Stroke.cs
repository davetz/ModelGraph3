
namespace ModelGraph.Core
{
    public enum Stroke : byte
    {
        IsSimple = 0x00, 
        EC_Round = 0x01,    // end cap
        EC_Square = 0x02,
        EC_Triangle = 0x03,
        DC_Round = 0x04,    // dash cap
        DC_Square = 0x06,
        DC_Triangle = 0x07,
        SC_Round = 0x10,    // start cap
        SC_Square = 0x20,
        SC_Triangle = 0x30,
        DottedOutline = 0x40, // shape's outline or line is drawn with dotted segments
        DashedOutline = 0x80, // shape's outline or line is drawn with dashed segments
        DashStyleMask = 0xC0,
        ShapeIsFilled = 0xFF, // shape or polygon is drawn as a filled solid 
    }
}
