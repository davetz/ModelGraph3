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
        bool TrySetState(DrawState state, bool reset = false);
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

        Extent ResizerExtent { get; } //in drawPoint coordinates

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
