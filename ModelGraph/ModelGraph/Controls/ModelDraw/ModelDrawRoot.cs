using ModelGraph.Core;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace ModelGraph.Controls
{
    public sealed partial class ModelDrawControl
    {
        private Vector2 GridPoint1;
        private Vector2 GridPoint2;

        #region HelperMethods  ================================================
        private void SetGridPoint1(PointerRoutedEventArgs e) => GridPoint1 = GridPoint(e);
        private void SetGridPoint2(PointerRoutedEventArgs e)
        {
            GridPoint2 = GridPoint(e);
            if (SelectorGrid.Visibility == Visibility.Visible) UpdateSelectorGrid();
        }
        private void SetDrawPoint1(CanvasControl canvas, IDrawData data, PointerRoutedEventArgs e) => data.Point1 = DrawPoint(canvas, e);
        private void SetDrawPoint2(CanvasControl canvas, IDrawData data, PointerRoutedEventArgs e) => data.Point2 = DrawPoint(canvas, e);
        private Vector2 GridPoint(PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(RootGrid).Position;
            return new Vector2((float)p.X, (float)p.Y);
        }
        private Vector2 DrawPoint(CanvasControl canvas, PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(canvas).Position;
            var (scale, offset) = CanvasScaleOffset[canvas];
            var x = (p.X - offset.X) / scale;
            var y = (p.Y - offset.Y) / scale;
            return new Vector2((float)x, (float)y);
        }

        private (float, float) GetFlyoutPoint()
        {
            var (scale, offset) = CanvasScaleOffset[EditCanvas];
            var p = DrawModel.FlyOutPoint * scale + offset;
            return (p.X, p.Y);
        }
        #endregion

        private void RootCanvas_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _isRootCanvasLoaded = true;
            RootCanvas.Loaded -= RootCanvas_Loaded;
            if (_isDrawCanvasLoaded) PanZoomReset();
        }
        bool _isRootCanvasLoaded;

        private void RootCanas_ContextRuested(Windows.UI.Xaml.UIElement sender, ContextRequestedEventArgs args)
        {
            if (args.TryGetPosition(RootCanvas, out Point p))
            {
                args.Handled = true;
                PostEvent(DrawEvent.ContextMenu);
            }
        }
        private void RootCanvas_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) => PanZoomReset();

        private void RootCanvas_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _rootPointerIsPressed = true;
            _rootCtrlPointerPressed = e.KeyModifiers.HasFlag(Windows.System.VirtualKeyModifiers.Control);
            _rootShiftPointerPressed = e.KeyModifiers.HasFlag(Windows.System.VirtualKeyModifiers.Shift);
            SetGridPoint1(e);
            SetDrawPoint1(EditCanvas, DrawModel.EditorData, e);
            e.Handled = true;

            if (_overridePointerPressed is null)
            {
                if (_rootCtrlPointerPressed)
                    PostEvent(DrawEvent.CtrlTap);
                else if (_rootShiftPointerPressed)
                    PostEvent(DrawEvent.ShiftTap);
                else
                    PostEvent(DrawEvent.Tap);
            }
            else
                _overridePointerPressed();
        }
        private void RootCanvas_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _rootPointerIsPressed = false;
            SetGridPoint2(e);
            SetDrawPoint2(EditCanvas, DrawModel.EditorData, e);
            e.Handled = true;

            if (_overridePointerReleased is null)
                PostEvent(DrawEvent.TapEnd);
            else
                _overridePointerReleased();
        }
        private void RootCanvas_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_isRootCanvasLoaded)
            {
                var p = e.GetCurrentPoint(EditCanvas).Position;
                if (p.X < 0 || p.Y < 0) return;

                SetGridPoint2(e);
                SetDrawPoint2(EditCanvas, DrawModel.EditorData, e);

                e.Handled = true;

                if (_overridePointerMoved is null)
                {
                    if (_rootPointerIsPressed)
                    {
                        if (_rootCtrlPointerPressed)
                            ExecuteAction(DrawEvent.CtrlDrag); // we want a fast responce
                        else if (_rootShiftPointerPressed)
                            ExecuteAction(DrawEvent.ShiftDrag); // we want a fast responce
                        else
                            ExecuteAction(DrawEvent.Drag);      // we want a fast responce
                    }
                    else
                        PostEvent(DrawEvent.Skim);
                }
                else
                    _overridePointerMoved();
            }
        }
        private bool _rootPointerIsPressed;
        private bool _rootCtrlPointerPressed;
        private bool _rootShiftPointerPressed;

        private void KeyboardAccelerator_Enter_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.EnterKey);
        private void KeyboardAccelerator_Escape_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.EscapeKey);
        private void KeyboardAccelerator_UpArrow_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.UpArrowKey);
        private void KeyboardAccelerator_DownArrow_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.DownArrowKey);
        private void KeyboardAccelerator_LeftArrow_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.LeftArrowKey);
        private void KeyboardAccelerator_RightArrow_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.RightArrowKey);

        private void TestDrawEvent(DrawEvent evt)
        {
            if (DrawModel.TryGetEventAction(evt, out _))
            {
                PostEvent(evt);
            }
        }
        private void RootCanvas_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            RestorePointerCursor();
            SideTreeCanvas.Focus(FocusState.Programmatic);
        }
        private void RootCanvas_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            RestoreDrawCursor();
        }
    }
}
