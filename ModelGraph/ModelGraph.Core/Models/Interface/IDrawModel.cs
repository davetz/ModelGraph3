using System;
using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public interface IDrawModel
    {
        DrawState DrawState { get; }
        DrawItem VisibleDrawItems { get; set; }

        void Release(); //user has closed this standalone view
        bool TryGetDrawEventAction(DrawEvent evt, out Action act);
        DrawCursor GetDrawStateCursor();

        bool IsPasteActionEnabled { get; }

        (byte,byte,byte,byte) ColorARGB { get; set; }

        Extent ResizerExtent { get; } //in drawPoint coordinates

        Vector2 FlyOutSize { get; } // flyoutTree size widht, height
        Vector2 FlyOutPoint { get; } //flyoutTree drawPoint coordinates
        string ToolTip_Text1 { get; }
        string ToolTip_Text2 { get; }

        IDrawData HelperData { get; } // layer1 - static background data - (editor canvas only)
        IDrawData EditorData { get; } // layer2 - editor & overview data
        IDrawData Picker1Data { get; }
        IDrawData Picker2Data { get; }

        ITreeModel FlyTreeModel { get; }
        ITreeModel SideTreeModel { get; }
        uint FlyTreeDelta { get; }
        uint SideTreeDelta { get; }
    }
}
