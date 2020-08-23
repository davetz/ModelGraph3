using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public interface IDrawCanvas
    {
        Vector2 GridPoint1 { get; set; }
        Vector2 GridPoint2 { get; set; }

        Vector2 DrawPoint1 { get; set; }
        Vector2 DrawPoint2 { get; set; }

        Vector2 NodePoint1 { get; }
        Vector2 NodePoint2 { get; }

        Vector2 RegionPoint1 { get; }
        Vector2 RegionPoint2 { get; }

        Vector2 GridPointDelta(bool reset);

        bool IsAnyHit { get; }
        bool IsHitPin { get; }
        bool IsHitNode { get; }
        bool IsHitRegion { get; }

        bool TapHitTest();
        bool EndHitTest();
        bool SkimHitTest();
        bool DragHitTest();

        bool RegionNodeHitTest();

        void ShowPropertyPanel();
        void HidePropertyPanel();

        string ToolTip_Text1 { get; }
        string ToolTip_Text2 { get; }

        void ResizeTop();
        void ResizeLeft();
        void ResizeRight();
        void ResizeBottom();
        void ResizeTopLeft();
        void ResizeTopRight();
        void ResizeBottomLeft();
        void ResizeBottomRight();
        void ResizePropagate();

        void RefreshDrawData();

        bool MoveNode();
        bool MoveRegion();

        bool CreateNode();

        void Pan(Vector2 adder); // update the coordinate point values so the drawing pans left/right
        void Zoom(float changeFactor); // update the coordinate point values so the drawing chages size
        void ZoomToExtent();     // update the coordinate point values so the drawing moves and chages its size
        void PanZoomReset(float width, float height); // recalculate the coordinate point values so the drawing is centered and fits on the screen

        float Scale { get; }    //referance scale factor used when populating the coordinate point values
        Vector2 Offset { get; } //reference offset  used when populating the coordinate point values

        IList<((float, float, float, float) XYXY, Stroke style, byte Width, (byte A, byte R, byte G, byte B) Color)> DrawRects { get; }
        IList<((float, float, float) XYR, Stroke style, byte Width, (byte A, byte R, byte G, byte B) Color)> DrawCircles { get; }
        IList<(Vector2[] Points, Stroke style, byte Width, (byte A, byte R, byte G, byte B) Color)> DrawLines { get; }
        IList<(Vector2[] Points, Stroke style, byte Width, (byte A, byte R, byte G, byte B) Color)> DrawSplines { get; }
        IList<(Vector2 TopLeft, string Text, (byte A, byte R, byte G, byte B) Color)> DrawText { get; }
    }
}
