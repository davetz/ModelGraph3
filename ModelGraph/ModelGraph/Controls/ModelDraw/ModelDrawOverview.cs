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
        private void RestoreOverview()
        {
            if (DrawModel.OverviewData is IDrawData)
            {
                OverviewBorder.Visibility = (DrawModel.VisibleDrawItems & DrawItem.Overview) == 0 ? Visibility.Collapsed : Visibility.Visible;
                OverviewResize.Visibility = (DrawModel.VisibleDrawItems & DrawItem.Overview) == 0 || DrawModel.Picker1Data is IDrawData ? Visibility.Collapsed : Visibility.Visible;
                OverCanvas.IsEnabled = true;  //enable CanvasDraw
                SetScaleOffset(OverCanvas, DrawModel.OverviewData);
            }
            else
                HideOverview();
        }
        internal void HideOverview()
        {
            OverCanvas.IsEnabled = false;  //disable CanvasDraw
            OverviewResize.Visibility = OverviewBorder.Visibility = Visibility.Collapsed;
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

        #region PointerEventOverride  =========================================
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
        #endregion
    }
}
