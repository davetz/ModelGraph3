
namespace ModelGraph.Core
{
    /// <summary>Used for tracking and debugging program flow</summary>
    public enum DrawState
    {
        Unknown = 0x0,

        NowOnVoid = 0x1,    //currently pointer is on void
        NowOnPin = 0x2,     //currently pointer is on a pin
        NowOnNode = 0x3,    //currently pointer is on a node
        NowOnEdge = 0x4,    //currently pointer is on an edge

        NowMask = 0x7,      //issolate current location type

        IsTarget = 0x80,    //currently pointer is on a valid target

        StartOnVoid = 0x10,     //starting from a void location
        StartOnPin = 0x20,      //starting from a pin
        StartOnNode = 0x30,     //starting from a node
        StartOnEdge = 0x40,     //starting from an edge

        StartOnMask = 0x70,     //issolate starting location type

        ViewMode = 0x100,       //enable viewing
        EditMode = 0x200,       //enable editing
        MoveMode = 0x300,       //enable moving nodes
        LinkMode = 0x400,       //enable linking
        PinsMode = 0x500,       //enable movinge individual pins
        CopyMode = 0x700,       //enable copying
        UnlinkMode = 0x800,     //enable linking
        CreateMode = 0xA00,     //enable createing
        DeleteMode = 0xB00,     //enable operating
        GravityMode = 0xC00,    //enable operating
        OperateMode = 0xD00,    //enable operating

        ModeMask = 0xF00,       //issolage current mode

        Tapped       = 0x01000,   //a pointer tap has occured
        CtrlTapped   = 0x02000,   //a pointer ctrl tap has occured
        ShiftTapped  = 0x03000,   //a pointer shift tap has occured
        Ending       = 0x04000,   //a pointer released has occured

        Dragging     = 0x15000,   //currently dragging something
        CtrlDraging  = 0x16000,   //currently ctrl dragging something
        ShiftDraging = 0x17000,   //currently shift dragging something
        UpArrow      = 0x18000,
        DownArrow    = 0x19000,
        LeftArrow    = 0x1A000,
        RightArrow   = 0x1B000,
        ContextMenu  = 0x0C000,    //context menu visible

        MayRepeat = 0x10000,
        EventMask    = 0x1F000,   //issolate current event

        NoChange = 0xFFFFFFF,
    };
}
