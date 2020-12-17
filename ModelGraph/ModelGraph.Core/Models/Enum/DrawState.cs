
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

        AddMode     = 0x100,                //enable adding
        ViewMode    = 0x200,                //enable viewing
        EditMode    = 0x300 | HasSelector,  //enable editing
        MoveMode    = 0x400 | HasSelector,  //enable moving nodes
        PinsMode    = 0x500 | HasSelector,  //enable movinge individual pins
        CopyMode    = 0x600 | HasSelector,  //enable copying
        LinkMode    = 0x700,                //enable linking
        UnlinkMode  = 0x800,                //enable linking
        CreateMode  = 0x900,                //enable createing
        DeleteMode  = 0xA00 | HasSelector,  //enable deleting
        GravityMode = 0xB00 | HasSelector,  //enable gravity
        OperateMode = 0xC00,                //enable operating

        ModeMask    = 0xF00 | HasSelector,  //issolate current mode

        Tapped      = 0x01000,   //a pointer pressed has occured
        CtrlTapped  = 0x02000,   //a pointer ctrl pressed has occured
        ShiftTapped = 0x03000,   //a pointer shift pressed has occured
        TapDragEnd  = 0x04000,   //a pointer released has occured

        Dragging     = 0x05000 | MayRepeat,  //currently dragging something
        CtrlDraging  = 0x06000 | MayRepeat,  //currently ctrl dragging something
        ShiftDraging = 0x07000 | MayRepeat,  //currently shift dragging something
        UpArrow      = 0x08000 | MayRepeat,
        DownArrow    = 0x09000 | MayRepeat,
        LeftArrow    = 0x0A000 | MayRepeat,
        RightArrow   = 0x0B000 | MayRepeat,

        ContextMenu  = 0x0C000,    //context menu visible

        EventMask    = 0x0F000 | MayRepeat,    //issolate current event

        MayRepeat   = 0x10000, //flag bit enable repeat action
        HasSelector = 0x20000, //flag bit enable select by range

        NoChange = 0xFFFFFFF,
    };
}
