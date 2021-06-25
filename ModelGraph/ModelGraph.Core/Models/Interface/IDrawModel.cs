using System;
using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public interface IDrawModel
    {
        byte ModeIndex { get; set; }

        void DrawControlReady(); //UI controls are loaded and ready for data 
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
        IDrawData BackData { get; }       // editor canvas background layer
        IDrawData EditData { get; }       // editor canvas interactive layer
        IDrawData Picker1Data { get; }    // picker1 canvas data
        IDrawData Picker2Data { get; }    // picker2 canvas data
        IDrawData OverviewData { get; }   // overview canvas data
        ITreeModel FlyTreeModel { get; }
        ITreeModel SideTreeModel { get; }

        //Optional ui controls
        int UndoCount { get; }
        int RedoCount { get; }
        bool HasUndoRedo { get; }
        bool HasApplyRevert { get; }
        bool HasColorPicker { get; }

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

        //ui control visibility
        bool ToolTipIsVisible { get; }
        bool Picker1IsVisible { get; }
        bool Picker2IsVisible { get; }
        bool SelectorIsVisible { get; }
        bool FlyTreeIsVisible { get; }
        bool SideTreeIsVisible { get; }
        bool OverviewIsVisible { get; set; }
    }
}
