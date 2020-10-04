
namespace ModelGraph.Core
{
    public enum DrawEvent
    { 
        Idle, 
        Tap,
        Context,
        DoubleTap, 
        TapEnd, 
        Skim, 
        Drag, 
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
        Picker1Drag,
        Cut,
        Copy,
        Paste,
        RotateLeft,
        RotateRight,
        SetDegree22,
        SetDegree30,
        VerticalFlip,
        HorizontalFlip,
        Center,
        ShowPins,
    };
}
