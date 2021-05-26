
namespace ModelGraph.Core
{
    public enum DrawEvent : byte
    {
        Idle,
        Undo,
        Redo,
        Apply,
        Revert,

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
        OverviewTap,
        Picker2Tap,
        Picker1Tap,
        Picker1CtrlTap,
        Picker1ShiftTap,
        Picker1Drag,

        TabKey,
        EnterKey,
        EscapeKey,
        UpArrowKey,
        DownArrowKey,
        LeftArrowKey,
        RightArrowKey,

        Pseudo, //pseudo event initiated by the model to envoke an action
    };
}
