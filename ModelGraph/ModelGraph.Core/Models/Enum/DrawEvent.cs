
namespace ModelGraph.Core
{
    public enum DrawEvent
    { 
        Idle,
        Tap,
        Skim,
        Drag,
        TapEnd,
        CtrlTap,
        ShiftTap,
        CtrlDrag,
        ShiftDrag,
        DoubleTap,
        ContextMenu,

        TopTap, 
        LeftTap, 
        RightTap, 
        BottomTap, 
        TopLeftTap, 
        TopRightTap, 
        BottomLeftTap, 
        BottomRightTap,

        OverviewTap,
        Picker2Tap,
        Picker1Tap,
        Picker1CtrlTap,
        Picker1ShiftTap,
        Picker1Drag,

        KeyEnter,
        KeyEscape,
        KeyUpArrow,
        KeyLeftArrow,
        KeyRightArrow,
        KeyDownArrow,

        Cut,
        Copy,
        Paste,
        Apply,
        Revert,
        Angle22,
        Angle30,
        Recenter,
        RotateLeft,
        RotateRight,
        FlipVertical,
        FlipHorizontal,
    };
}
