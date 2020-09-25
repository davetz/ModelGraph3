using System;
using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public interface IDrawModel
    {
        (DrawState, Dictionary<DrawEvent,Func<DrawState>>) DrawStateChanged(DrawState newState);
        DrawState CurrentDrawState { get; set; }
        DrawMode CurrentDrawMode { get; set; }

        (byte,byte,byte,byte) ColorARGB { get; set; }
        void ColorARGBChanged();
        void Apply();
        void Reload();

        Extent EditorExtent { get; }
        void RefreshEditorData();
        int Picker1Width { get; }
        int Picker2Width { get; }
        void Picker1Select(int YCord, bool add = false);
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

        bool MoveNode();
        bool MoveRegion();

        bool CreateNode();

        IDrawData HelperData { get; } // layer1 - static background data - (editor canvas only)
        IDrawData EditorData { get; } // layer2 - editor & overview data
        IDrawData Picker1Data { get; }
        IDrawData Picker2Data { get; }

        ITreeModel FlyTreeModel { get; }
        ITreeModel SideTreeModel { get; }
    }
}
