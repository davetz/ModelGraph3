using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public interface ICanvasModel
    {
        Extent EditorExtent { get; }
        int Picker1Width { get; }
        int Picker2Width { get; }
        void Picker1Select(int YCord);
        void Picker2Select(int YCord);
        void Picker2Paste();

        Vector2 GridPoint1 { get; set; }
        Vector2 GridPoint2 { get; set; }

        Vector2 DrawPoint1 { get; set; }
        Vector2 DrawPoint2 { get; set; }

        Vector2 NodePoint1 { get; }
        Vector2 NodePoint2 { get; }

        Vector2 RegionPoint1 { get; set; }
        Vector2 RegionPoint2 { get; set; }

        Vector2 GridPointDelta(bool reset);

        bool AnyHit { get; }
        bool PinHit { get; }
        bool NodeHit { get; }
        bool EdgeHit { get; }
        bool RegionHit { get; }
        bool IsValidRegion();
        void ClearRegion();

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

        IDrawData EditorData { get; }
        IDrawData Picker1Data { get; }
        IDrawData Picker2Data { get; }
    }
}
