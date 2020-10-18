
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
        NowOnRegion = 0x5,  //currently pointer is on a region

        NowMask = 0x7,      //issolate current location type

        IsTarget = 0x80,    //currently pointer is on a valid target

        StartOnVoid = 0x10,     //starting from a void location
        StartOnPin = 0x20,      //starting from a pin
        StartOnNode = 0x30,     //starting from a node
        StartOnEdge = 0x40,     //starting from an edge
        StartOnRegion = 0x50,   //starting from a region

        StartOnMask = 0x70,     //issolate starting location type

        ViewMode = 0x100,       //enable viewing
        EditMode = 0x200,       //enable editing
        MoveMode = 0x300,       //enable moving
        LinkMode = 0x400,       //enable linking
        CutMode = 0x500,        //enable cutting
        CopyMode = 0x600,       //enable copying
        PasteMode = 0x700,      //enable pasting
        CreateMode = 0x800,     //enable createing
        OperateMode = 0x900,    //enable operating
        ContextMenu = 0xA00,    //context menu visible
        PropertySheet = 0xB00,  //property sheet visible
        Apply = 0xC00,          //apply changes to model
        Revert = 0xD00,         //revert to previous version

        ModeMask = 0xF00,       //issolage current mode

        Tapped = 0x1000,         //a pointer tap has occured
        Ending = 0x2000,         //a pointer released has occured
        Draging = 0x3000,        //currently dragging something
        Skimming = 0x4000,       //currently skimming over the surface

        EventMask = 0x7000,     //issolate current event

        ViewOnVoid = ViewMode | NowOnVoid,      //hide tooltips
        ViewOnPin = ViewMode | NowOnPin,        //show tooltips
        ViewOnNode = ViewMode | NowOnNode,      //show tooltips
        ViewOnEdge = ViewMode | NowOnEdge,      //show tooltips
        ViewOnRegion = ViewMode | NowOnRegion,


        ViewOnVoidTapped = ViewMode | NowOnVoid | Tapped,       //hide property sheet, enable region tracing
        ViewOnVoidDragging = ViewMode | StartOnVoid | Draging,  //tracing a new region
        ViewOnPinTapped = ViewMode | StartOnPin | Tapped,       //show property sheet
        ViewOnNodeTapped = ViewMode | StartOnNode | Tapped,     //show property sheet,
        ViewOnEdgeTapped = ViewMode | StartOnEdge | Tapped,     //show property sheet,
        ViewOnRegionTapped = ViewMode | StartOnRegion | Tapped, //show property sheet,

        EditOnVoid = EditMode | NowOnVoid,
        EditOnPin = EditMode | NowOnPin,
        EditOnNode = EditMode | NowOnNode,
        EditOnEdge = EditMode | NowOnEdge,
        EditOnRegion = EditMode | NowOnRegion,

        EditOnVoidTapped = EditMode | NowOnVoid | Tapped,
        EditOnPinTapped = EditMode | StartOnPin | Tapped,
        EditOnNodeTapped = EditMode | StartOnNode | Tapped,
        EditOnEdgeTapped = EditMode | StartOnEdge | Tapped,
        EditOnRegionTapped = EditMode | StartOnRegion | Tapped,

        MoveOnVoid = MoveMode | NowOnVoid, 
        MoveOnPin = MoveMode | NowOnPin,                        //moving a pin state-1     
        MoveOnPinTapped = MoveMode | NowOnPin | Tapped,         //moving a pin state-2
        MoveOnPinDragging = MoveMode | StartOnPin | Draging,    //moving a pin state-3

        MovenOnNode = MoveMode | NowOnNode,                     //moving a node state-1
        MoveOnNodeTapped = MoveMode | NowOnNode | Tapped,       //moving a node state-2
        MoveOnNodeDraggging = MoveMode | StartOnNode | Draging, //moving a node state-3

        MoveOnRegion = MoveMode | NowOnRegion,                      //moving a region state-1
        MoveOnRegionTapped = MoveMode | NowOnRegion | Tapped,       //moving a region state-2
        MoveOnRegionDragging = MoveMode | StartOnRegion | Draging,  //moving a region state-3


        LinkOnVoid = LinkMode | NowOnVoid,

        LinkOnPin = LinkMode | NowOnPin,                        //linking a pin state-1     
        LinkOnPinTapped = LinkMode | NowOnPin | Tapped,         //linking a pin state-2
        LinkOnPinDragging = LinkMode | StartOnPin | Draging,    //linking a pin state-3

        LinkPinDraggingNowOnVoid = LinkMode | StartOnPin | NowOnVoid | Draging,        //linking a pin state-4
        LinkPinDraggingNowOnPin = LinkMode | StartOnPin | NowOnPin | Draging,          //linking a pin state-5
        LinkPinDraggingNowOnNode = LinkMode | StartOnPin | NowOnNode | Draging,        //linking a pin state-5
        LinkPinDraggingNowOnEdge = LinkMode | StartOnPin | NowOnEdge | Draging,        //linking a pin state-5
        LinkPinDraggingNoOnRegion = LinkMode | StartOnPin | NowOnRegion | Draging,     //linking a pin state-5

        LinkPinDraggingNowOnTargetPin = LinkMode | StartOnPin | NowOnPin | Draging | IsTarget,         //linking a pin state-6
        LinkPinDraggingNowOnTargetNode = LinkMode | StartOnPin | NowOnNode | Draging | IsTarget,       //linking a pin state-6
        LinkPinDraggingNowOnTargetEdge = LinkMode | StartOnPin | NowOnEdge | Draging | IsTarget,       //linking a pin state-6
        LinkPinDraggingNowOnTargetRegion = LinkMode | StartOnPin | NowOnRegion | Draging | IsTarget,   //linking a pin state-6

        LinkPinEndingNowOnTargetPin = LinkMode | StartOnPin | NowOnPin | Ending | IsTarget,         //linking a pin state-7
        LinkPinEndingNowOnTargetNode = LinkMode | StartOnPin | NowOnNode | Ending | IsTarget,       //linking a pin state-7
        LinkPinEndingNowOnTargetEdge = LinkMode | StartOnPin | NowOnEdge | Ending | IsTarget,       //linking a pin state-7
        LinkPinEndingNowOnTargetRegion = LinkMode | StartOnPin | NowOnRegion | Ending | IsTarget,   //linking a pin state-7

        LinkOnNode = LinkMode | NowOnNode,                      //linking a node state-1
        LinkOnNodeTapped = LinkMode | NowOnNode | Tapped,       //linking a node state-2
        LinkOnNodeDraggging = LinkMode | StartOnNode | Draging, //linking a node state-3

        LinkNodeDraggingOnVoid = LinkMode | StartOnNode | NowOnVoid | Draging,        //linking a node state-4
        LinkNodeDraggingOnPin = LinkMode | StartOnNode | NowOnPin | Draging,          //linking a node state-5
        LinkNodeDraggingOnNode = LinkMode | StartOnNode | NowOnNode | Draging,        //linking a node state-5
        LinkNodeDraggingOnEdge = LinkMode | StartOnNode | NowOnEdge | Draging,        //linking a node state-5
        LinkNodeDraggingOnRegion = LinkMode | StartOnNode | NowOnRegion | Draging,    //linking a node state-5

        LinkNodeDraggingNowOnTargetPin = LinkMode | StartOnNode | NowOnPin | Draging | IsTarget,         //linking a node state-6
        LinkNodeDraggingNowOnTargetNode = LinkMode | StartOnNode | NowOnNode | Draging | IsTarget,       //linking a node state-6
        LinkNodeDraggingNowOnTargetEdge = LinkMode | StartOnNode | NowOnEdge | Draging | IsTarget,       //linking a node state-6
        LinkNodeDraggingNowOnTargetRegion = LinkMode | StartOnNode | NowOnRegion | Draging | IsTarget,   //linking a node state-6

        LinkNodeEndingNowOnTargetPin = LinkMode | StartOnNode | NowOnPin | Ending | IsTarget,         //linking a node state-7
        LinkNodeEndingNowOnTargetNode = LinkMode | StartOnNode | NowOnNode | Ending | IsTarget,       //linking a node state-7
        LinkNodeEndingNowOnTargetEdge = LinkMode | StartOnNode | NowOnEdge | Ending | IsTarget,       //linking a node state-7
        LinkNodeEndingNowOnTargetRegion = LinkMode | StartOnNode | NowOnRegion | Ending | IsTarget,   //linking a node state-7

        LinkOnRegion = LinkMode | NowOnRegion,                      //linking a region state-1
        LinkOnRegionTapped = LinkMode | NowOnRegion | Tapped,       //linking a region state-2
        LinkOnRegionDragging = LinkMode | StartOnRegion | Draging,  //linking a region state-3

        LinkRegionDraggingOnVoid = LinkMode | StartOnRegion | NowOnVoid | Draging,        //linking a region state-4
        LinkRegionDraggingOnPin = LinkMode | StartOnRegion | NowOnPin | Draging,          //linking a region state-5
        LinkRegionDraggingOnNode = LinkMode | StartOnRegion | NowOnNode | Draging,        //linking a region state-5
        LinkRegionDraggingOnEdge = LinkMode | StartOnRegion | NowOnEdge | Draging,        //linking a region state-5
        LinkRegionDraggingOnRegion = LinkMode | StartOnRegion | NowOnRegion | Draging,    //linking a region state-5

        LinkRegionDraggingNowOnTargetPin = LinkMode | StartOnNode | NowOnPin | Draging | IsTarget,         //linking a region state-6
        LinkRegionDraggingNowOnTargetNode = LinkMode | StartOnNode | NowOnNode | Draging | IsTarget,       //linking a region state-6
        LinkRegionDraggingNowOnTargetEdge = LinkMode | StartOnNode | NowOnEdge | Draging | IsTarget,       //linking a region state-6
        LinkRegionDraggingNowOnTargetRegion = LinkMode | StartOnNode | NowOnRegion | Draging | IsTarget,   //linking a region state-6

        LinkRegionEndingNowOnTargetPin = LinkMode | StartOnNode | NowOnPin | Ending | IsTarget,         //linking a region state-7
        LinkRegionEndingNowOnTargetNode = LinkMode | StartOnNode | NowOnNode | Ending | IsTarget,       //linking a region state-7
        LinkRegionEndingNowOnTargetEdge = LinkMode | StartOnNode | NowOnEdge | Ending | IsTarget,       //linking a region state-7
        LinkRegionEndingNowOnTargetRegion = LinkMode | StartOnNode | NowOnRegion | Ending | IsTarget,   //linking a region state-7

        CutOnVoid = CutMode | NowOnVoid,
        CutOnPin = CutMode | NowOnPin,
        CutOnNode = CutMode | NowOnNode,
        CutOnEdge = CutMode | NowOnEdge,
        CutOnRegion = CutMode | NowOnRegion,

        CutOnPinTapped = CutMode | StartOnPin | Tapped,
        CutOnNodeTapped = CutMode | StartOnNode | Tapped,
        CutOnEdgeTapped = CutMode | StartOnEdge | Tapped,
        CutOnRegionTapped = CutMode | StartOnRegion | Tapped,

        CopyOnVoid = CopyMode | NowOnVoid,
        CopyOnNode = CopyMode | NowOnNode,                      //copy node state-1
        CopyOnNodeTapped = CopyMode | NowOnNode | Tapped,       //copy node state-2
        CopyNodeDragging = CopyMode | StartOnNode | Draging,    //copy node state-3
        CopyNodeEnding = CopyMode | StartOnNode | Ending,       //copy node state-4

        CopyOnRegion = CopyMode | NowOnRegion,                      //copy region state-1
        CopyOnRegionTapped = CopyMode | NowOnRegion | Tapped,       //copy region state-2
        CopyRegionDragging = CopyMode | StartOnRegion | Draging,    //copy region state-3
        CopyRegionEnding = CopyMode | StartOnRegion | Ending,       //copy region state-4

        PasteOnVoid = PasteMode | NowOnVoid,
        PasteOnPin = PasteMode | NowOnPin,
        PasteOnNode = PasteMode | NowOnNode,
        PasteOnEdge = PasteMode | NowOnEdge,
        PasteOnRegion = PasteMode | NowOnRegion,

        PasteOnVoidTapped = PasteMode | NowOnVoid | Tapped,
        PasteOnPinTapped = PasteMode | StartOnPin | Tapped,
        PasteOnNodeTapped = PasteMode | StartOnNode | Tapped,
        PasteOnEdgeTapped = PasteMode | StartOnEdge | Tapped,
        PasteOnRegionTapped = PasteMode | StartOnRegion | Tapped,

        CreateOnVoid = CreateMode | NowOnVoid, 
        CreateOnVoidTapped = CreateMode | NowOnVoid | Tapped,

        OperateOnVoid = OperateMode | NowOnVoid,

        OperateOnPin = OperateMode | NowOnPin,
        OperatePinTapped = OperateMode | NowOnPin | Tapped,

        OperateOnNode = OperateMode | NowOnNode,
        OperateNodeTapped = OperateMode | NowOnNode | Tapped,

        ContextMenueOnVoidTapped = ContextMenu | NowOnVoid | Tapped,
        PropertySheetOnVoidTapped = PropertySheet | NowOnVoid | Tapped,

        ResizeOverview,

        ResizeTop, ResizeLeft, ResizeRight, ResizeBottom,
        ResizeTopLeft, ResizeTopRight, ResizeBottomLeft, ResizeBottomRight,

        NoChange = 0xFFFF,
    };
}
