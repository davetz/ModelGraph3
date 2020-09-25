
namespace ModelGraph.Core
{
    public enum DrawState
    {
        Unknown, 
        NoChange, //returned from DrawEventAction call to indicate there is no change to the current draw state 

        ViewIdle, ViewOnVoid,
        ViewOnPin, ViewOnNode, ViewOnEdge, ViewOnRegion,                //show tooltips
        ViewOnVoidTap, ViewOnVoidDrag,                                  //trace a new region
        ViewOnPinTap, ViewOnNodeTap, ViewOnEdgeTap, ViewOnRegionTap,    //show property sheet

        MoveIdle, MoveOnVoid,
        MoveOnPinTap, MovenOnNodeTap, MoveOnRegionTap,
        MoveOnPinDrag, MoveOnNodeDrag, MoveOnRegionDrag,
        MoveOnPinSkim, MoveOnNodeSkim, MoveOnRegionSkim,

        LinkIdle, LinkOnVoid,
        LinkOnPinTap, LinkOnNodeTap, LinkOnRegionTap,
        LinkOnPinDrag, LinkOnPinDragOnTarget,           //switches between these two modes
        LinkOnNodeDrag, LinkOnNodeDragOnTarget,         //switches between these two modes
        LinkOnRegionDrag, LinkOnRegionDragOnTarget,     //switches between these two modes

        CopyIdle, CopyOnVoid,
        CopyOnNodeTap, CopyOnRegionTap,
        CopyOnNodeDrag, CopyNodeDraggingOnVoid,     //switches between these two modes
        CopyOnRegionDrag, CopyRegionDragginOnVoid,  //switches between these two modes

        CreateIdle, CreateOnVoid, CreateOnVoidTap,

        OpertateIdel, OperateOnVoid, OperateOnNode, OperateOnNodeTap,

        ResizeOverview,

        ResizeTop, ResizeLeft, ResizeRight, ResizeBottom,
        ResizeTopLeft, ResizeTopRight, ResizeBottomLeft, ResizeBottomRight,
    };
}
