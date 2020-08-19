using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;

using Windows.Foundation;
using Windows.UI.Xaml;

namespace ModelGraph.Core
{
    public interface ICanvasModel : IDataModel
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

        void RefreshCanvasDrawData();

        bool MoveNode();
        bool MoveRegion();

        bool CreateNode();

        IList<((float, float, float, float) Rect, (byte A, byte R, byte G, byte B) Color)> DrawRects { get; }
        IList<(Vector2[] Points, bool IsDotted, byte Width, (byte A, byte R, byte G, byte B) Color)> DrawLines { get; }
        IList<(Vector2[] Points, bool IsDotted, byte Width, (byte A, byte R, byte G, byte B) Color)> DrawSplines { get; }
        IList<(Vector2 TopLeft, string Text, (byte A, byte R, byte G, byte B) Color)> DrawText { get; }
    }
}
