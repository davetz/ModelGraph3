using ModelGraph.Core;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ModelGraph.Controls
{
    public sealed partial class CanvasDrawControl
    {
        void ShowResizerGrid()
        {
            UpdateResizerGrid();
            ResizerGrid.Visibility = Visibility.Visible;
        }

        void HideResizerGrid() => ResizerGrid.Visibility = Visibility.Collapsed;

        void UpdateResizerGrid()
        {
            var (top, left, width, height) = GetResizerParams();

            var margin = ResizerBorder.Margin;

            ResizerGrid.Width = width + margin.Left + margin.Right;
            ResizerGrid.Height = height + margin.Top + margin.Bottom;

            Canvas.SetTop(ResizerGrid, top - margin.Top);
            Canvas.SetLeft(ResizerGrid, left - margin.Left);
        }

        #region Resizer_PointerEvents  =========================================
        private void Sizer_PointerExited(object sender, PointerRoutedEventArgs e) => ConditionalySetNewCursor(CoreCursorType.Arrow);

        private void SizerTop_PointerEntered(object sender, PointerRoutedEventArgs e) => ConditionalySetNewCursor(CoreCursorType.SizeNorthSouth);
        private void SizerLeft_PointerEntered(object sender, PointerRoutedEventArgs e) => ConditionalySetNewCursor(CoreCursorType.SizeWestEast);
        private void SizerRight_PointerEntered(object sender, PointerRoutedEventArgs e) => ConditionalySetNewCursor(CoreCursorType.SizeWestEast);
        private void SizerBottom_PointerEntered(object sender, PointerRoutedEventArgs e) => ConditionalySetNewCursor(CoreCursorType.SizeNorthSouth);

        private void SizerTopLeft_PointerEntered(object sender, PointerRoutedEventArgs e) => ConditionalySetNewCursor(CoreCursorType.SizeNorthwestSoutheast);
        private void SizerTopRight_PointerEntered(object sender, PointerRoutedEventArgs e) => ConditionalySetNewCursor(CoreCursorType.SizeNortheastSouthwest);
        private void SizerBottomLeft_PointerEntered(object sender, PointerRoutedEventArgs e) => ConditionalySetNewCursor(CoreCursorType.SizeNortheastSouthwest);
        private void SizerBottomRight_PointerEntered(object sender, PointerRoutedEventArgs e) => ConditionalySetNewCursor(CoreCursorType.SizeNorthwestSoutheast);

        void ConditionalySetNewCursor(CoreCursorType cursorType) { if (_pointerIsPressed) return; TrySetNewCursor(cursorType); }

        private void SizerTop_PointerPressed(object sender, PointerRoutedEventArgs e) { if (Model.DrawEvent_Action.TryGetValue(DrawEvent.TopTap, out Action action)) ExecuteSizeHit(e, action); }
        private void SizerLeft_PointerPressed(object sender, PointerRoutedEventArgs e) { if (Model.DrawEvent_Action.TryGetValue(DrawEvent.LeftTap, out Action action)) ExecuteSizeHit(e, action); }
        private void SizerRight_PointerPressed(object sender, PointerRoutedEventArgs e) { if (Model.DrawEvent_Action.TryGetValue(DrawEvent.RightTap, out Action action)) ExecuteSizeHit(e, action); }
        private void SizerBottom_PointerPressed(object sender, PointerRoutedEventArgs e) { if (Model.DrawEvent_Action.TryGetValue(DrawEvent.BottomTap, out Action action)) ExecuteSizeHit(e, action); }
        private void SizerTopLeft_PointerPressed(object sender, PointerRoutedEventArgs e) { if (Model.DrawEvent_Action.TryGetValue(DrawEvent.TopLeftTap, out Action action)) ExecuteSizeHit(e, action); }
        private void SizerTopRight_PointerPressed(object sender, PointerRoutedEventArgs e) { if (Model.DrawEvent_Action.TryGetValue(DrawEvent.TopRightTap, out Action action)) ExecuteSizeHit(e, action); }
        private void SizerBottomLeft_PointerPressed(object sender, PointerRoutedEventArgs e) { if (Model.DrawEvent_Action.TryGetValue(DrawEvent.BottomLeftTap, out Action action)) ExecuteSizeHit(e, action); }
        private void SizerBottomRight_PointerPressed(object sender, PointerRoutedEventArgs e) { if (Model.DrawEvent_Action.TryGetValue(DrawEvent.BottomRightTap, out Action action)) ExecuteSizeHit(e, action); }

        private void ExecuteSizeHit(PointerRoutedEventArgs e, Action action)
        {
            e.Handled = true;
            _pointerIsPressed = true;
            HideToolTip();
            action();
        }
        #endregion
    }
}
