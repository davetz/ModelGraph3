﻿
namespace ModelGraph.Core
{
    public enum StrokeType : byte
    {
        Simple = 0,         // simple solid line with flat start/end cap style

        EC_Round = 0x01,    // end cap
        EC_Square = 0x02,
        EC_Triangle = 0x03,

        DC_Round = 0x04,    // dash cap
        DC_Square = 0x06,
        DC_Triangle = 0x07,

        SC_Round = 0x10,    // start cap
        SC_Square = 0x20,
        SC_Triangle = 0x30,

        Dotted = 0x40, // shape's outline or line is drawn with dotted segments
        Dashed = 0x80, // shape's outline or line is drawn with dashed segments
        DashMask = 0xC0,

        Filled = 0xFF, // shape or polygon is drawn as a filled solid 
    }
}
