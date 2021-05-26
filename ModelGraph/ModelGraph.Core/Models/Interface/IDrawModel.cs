using System;
using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public interface IDrawModel
    {
        byte ModeIndex { get; set; }
        DrawItem VisibleDrawItems { get; set; }

        void PostRefresh(); //UI controls are loaded and ready for data 
        void Release(); //user has closed this standalone view
        List<IdKey> GetModeIdKeys();
        DrawCursor GetModeStateCursor();
        bool TryGetEventAction(DrawEvent evt, out Action act);

        (byte,byte,byte,byte) ColorARGB { get; set; }


        Vector2 FlyOutSize { get; } // flyoutTree size widht, height
        Vector2 FlyOutPoint { get; } //flyoutTree drawPoint coordinates
        string ToolTip_Text1 { get; }
        string ToolTip_Text2 { get; }

        // a null pointer indicates a usage choice
        IDrawData BackData { get; } // editor background layer
        IDrawData EditData { get; } // editor interactive layer
        IDrawData Picker1Data { get; }
        IDrawData Picker2Data { get; }
        IDrawData OverviewData { get; }
        ITreeModel FlyTreeModel { get; }
        ITreeModel SideTreeModel { get; }

        //Optional ui controls
        bool HasUndoRedo { get; }
        bool HasApplyRevert { get; }
        ushort UndoCount { get; }
        ushort RedoCount { get; }

        // initial ui layout grid widths
        ushort EditorWidth { get; }
        ushort Picker1Width { get; }
        ushort Picker2Width { get; }
        ushort OverviewWidth { get; }
        ushort SideTreeWidth { get; }

        // increment the delta after updating the data
        byte EditorDelta { get; }
        byte Picker1Delta { get; }
        byte Picker2Delta { get; }
        byte OverviewDelta { get; }
        byte ToolTipDelta { get; }
        byte FlyTreeDelta { get; }
        byte SideTreeDelta { get; }
    }
}
