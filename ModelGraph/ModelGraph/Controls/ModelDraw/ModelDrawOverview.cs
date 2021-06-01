using System;
using System.Numerics;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using ModelGraph.Core;

namespace ModelGraph.Controls
{
    public sealed partial class ModelDrawControl
    {
        private void ConfigOverview()
        {
            if (DrawModel.OverviewData is IDrawData)
            {
                OverviewBorder.Width = DrawModel.OverviewWidth + OverviewBorder.BorderThickness.Right;
                OverviewBorder.Height = DrawModel.OverviewWidth;
                RestoreOverview();
            }
            else
                HideOverview();
        }
        private void OverviewOnOffTextBlock_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_overviewIsVisible) HideOverview(); else RestoreOverview();
        }

        private void RestoreOverview()
        {
            if (DrawModel.OverviewData is IDrawData)
            {
                DrawModel.OverviewIsVisible = _overviewIsVisible = true;
                OverviewBorder.Visibility = Visibility.Visible;
                OverviewOnOffTextBlock.Text = "\uF0AD";
                OverviewBorder.Visibility = Visibility.Visible;
                OverviewResize.Visibility = DrawModel.Picker1Data is null ? Visibility.Visible : Visibility.Collapsed;
                OverCanvas.IsEnabled = true;  //enable CanvasDraw
                SetScaleOffset(OverCanvas, DrawModel.OverviewData);
                OverCanvas.Invalidate();
            }
            else
                HideOverview();
        }
        internal void HideOverview()
        {
            DrawModel.OverviewIsVisible = _overviewIsVisible = false;
            OverviewBorder.Visibility = OverviewResize.Visibility = Visibility.Collapsed;
            OverviewOnOffTextBlock.Text = "\uF0AE";
            OverCanvas.IsEnabled = false;  //disable CanvasDraw
        }

        private void OverCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;
            PostEvent(DrawEvent.OverviewTap);
        }
        private void OverviewResize_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!_rootPointerIsPressed) TrySetNewCursor(CoreCursorType.SizeAll);
        }
        private void OverviewResize_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!_rootPointerIsPressed) RestorePointerCursor();
        }

        private void OverviewResize_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _rootPointerIsPressed = true;
            GridPoint1 = new Vector2(0, 0);
            GridPoint2 = GridPoint(e);
            OverridePointerMoved(ResizingOverview);
            OverridePointerReleased(ClearPointerOverrides);
        }
        private void ResizingOverview()
        {
            var size = Vector2.Abs(GridPoint1 - GridPoint2);
            if (size.X < OverviewBorder.MinWidth) return;
            if (size.Y < OverviewBorder.MinHeight) return;

            OverviewBorder.Width = size.X;
            OverviewBorder.Height = size.Y;
            SetScaleOffset(OverCanvas, DrawModel.EditData);
            OverCanvas.Invalidate();
        }

        private void OverridePointerMoved(Action action) => _overridePointerMoved = action;
        private void OverridePointerPressed(Action action) => _overridePointerPressed = action;
        private void OverridePointerReleased(Action action) => _overridePointerReleased = action;
        private void ClearPointerOverrides()
        {
            RestorePointerCursor();
            _overridePointerPressed = _overridePointerMoved = _overridePointerReleased = null;
        }

        private Action _overridePointerMoved;
        private Action _overridePointerPressed;
        private Action _overridePointerReleased;
    }
}
