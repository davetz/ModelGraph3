using System;
using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public interface IDrawModel
    {
        DrawState DrawState { get; }
        DrawCursor DrawCursor { get; }
        Dictionary<DrawEvent, Action> DrawEvent_Action { get; }
        bool TrySetState(DrawState state);
        void SetEventAction(DrawEvent evt, Action act);
        bool IsToolTipVisible { get; }
        bool IsResizerGridVisible { get; }
        bool IsFlyTreeVisible { get; }
        bool IsSideTreeVisible { get; }
        bool IsOverviewVisible { get; }
        bool IsPicker1Visible { get; }
        bool IsPicker2Visible { get; }
        bool IsColorPickerEnabled { get; }


        (byte,byte,byte,byte) ColorARGB { get; set; }
        void ColorARGBChanged();
        void Apply();
        void Reload();

        Extent EditorExtent { get; }
        Extent ResizerExtent { get; } //in drawPoint coordinates

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

        Vector2 GridPointDelta(bool reset);

        Vector2 ToolTipTarget { get; } //in drawPoint coordinates
        string ToolTip_Text1 { get; }
        string ToolTip_Text2 { get; }

        IDrawData HelperData { get; } // layer1 - static background data - (editor canvas only)
        IDrawData EditorData { get; } // layer2 - editor & overview data
        IDrawData Picker1Data { get; }
        IDrawData Picker2Data { get; }

        ITreeModel FlyTreeModel { get; }
        ITreeModel SideTreeModel { get; }
    }
}
