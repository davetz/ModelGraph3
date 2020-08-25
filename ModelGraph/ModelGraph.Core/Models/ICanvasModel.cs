using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public interface ICanvasModel
    {
        Vector2 GridPoint1 { get; set; }
        Vector2 GridPoint2 { get; set; }

        Vector2 DrawPoint1 { get; set; }
        Vector2 DrawPoint2 { get; set; }

        Vector2 NodePoint1 { get; }
        Vector2 NodePoint2 { get; }

        Vector2 RegionPoint1 { get; set; }
        Vector2 RegionPoint2 { get; set; }

        Vector2 GridPointDelta(bool reset);

        bool IsAnyHit { get; }
        bool IsHitPin { get; }
        bool IsHitNode { get; }
        bool IsHitEdge { get; }
        bool IsHitRegion { get; }
        bool IsValidRegion();

        bool TapHitTest();
        bool EndHitTest();
        bool SkimHitTest();
        bool DragHitTest();

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

        Extent DrawingExtent { get; }

        List<((Vector2,  Vector2) centerRadius, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB)> DrawRects { get; }
        List<((Vector2, float) centerRadius, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB)> DrawCircles { get; }
        List<(Vector2[] Points, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB)> DrawLines { get; }
        List<(Vector2[] Points, (Stroke, byte) strokeWidth, (byte, byte, byte, byte) ARGB)> DrawSplines { get; }
        List<((Vector2, string) topLeftText, (byte, byte, byte, byte) ARGB)> DrawText { get; }
    }
}
