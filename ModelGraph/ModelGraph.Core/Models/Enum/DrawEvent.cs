
namespace ModelGraph.Core
{
    public enum DrawEvent : byte
    {
        Idle,

        Btn1,
        Btn2,
        Btn3,
        Btn4,
        Btn5,
        Btn6,
        Btn7,
        Btn8,
        Btn9,
        BtnA,
        BtnB,
        BtnC,

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

        ExecuteAction, //pseudo event initiated by the model to envoke an action
    };
}
