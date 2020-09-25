using ModelGraph.Core;
using System;
using System.Collections.Generic;
using System.Numerics;
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

        private void SizerTop_PointerPressed(object sender, PointerRoutedEventArgs e) { if (Event_Action.TryGetValue(DrawEvent.TopHit, out Func<DrawState> action)) ExecuteSizeHit(e, action); }
        private void SizerLeft_PointerPressed(object sender, PointerRoutedEventArgs e) { if (Event_Action.TryGetValue(DrawEvent.LeftHit, out Func<DrawState> action)) ExecuteSizeHit(e, action); }
        private void SizerRight_PointerPressed(object sender, PointerRoutedEventArgs e) { if (Event_Action.TryGetValue(DrawEvent.RightHit, out Func<DrawState> action)) ExecuteSizeHit(e, action); }
        private void SizerBottom_PointerPressed(object sender, PointerRoutedEventArgs e) { if (Event_Action.TryGetValue(DrawEvent.BottomHit, out Func<DrawState> action)) ExecuteSizeHit(e, action); }
        private void SizerTopLeft_PointerPressed(object sender, PointerRoutedEventArgs e) { if (Event_Action.TryGetValue(DrawEvent.TopLeftHit, out Func<DrawState> action)) ExecuteSizeHit(e, action); }
        private void SizerTopRight_PointerPressed(object sender, PointerRoutedEventArgs e) { if (Event_Action.TryGetValue(DrawEvent.TopRightHit, out Func<DrawState> action)) ExecuteSizeHit(e, action); }
        private void SizerBottomLeft_PointerPressed(object sender, PointerRoutedEventArgs e) { if (Event_Action.TryGetValue(DrawEvent.BottomLeftHit, out Func<DrawState> action)) ExecuteSizeHit(e, action); }
        private void SizerBottomRight_PointerPressed(object sender, PointerRoutedEventArgs e) { if (Event_Action.TryGetValue(DrawEvent.BottomRightHit, out Func<DrawState> action)) ExecuteSizeHit(e, action); }

        private void ExecuteSizeHit(PointerRoutedEventArgs e, Func<DrawState> action)
        {
            e.Handled = true;
            _pointerIsPressed = true;
            HideTootlip();
            var newState = action();
            TrySetState(newState);
        }
        #endregion

        #region PointerCursor  ================================================
        private void RestorePointerCursor() => TrySetNewCursor(CoreCursorType.Arrow);
        private void TrySetNewCursor(CoreCursorType cursorType)
        {
            if (_currentCusorType == cursorType) return;
            if (_cursors.TryGetValue(cursorType, out CoreCursor newCursor))
            {
                _currentCusorType = cursorType;
                Window.Current.CoreWindow.PointerCursor = newCursor;
            }
        }
        private CoreCursorType _currentCusorType;
        readonly Dictionary<CoreCursorType, CoreCursor> _cursors = new Dictionary<CoreCursorType, CoreCursor>()
        {
            [CoreCursorType.Pin] = new CoreCursor(CoreCursorType.Pin, 0),
            [CoreCursorType.Hand] = new CoreCursor(CoreCursorType.Hand, 0),
            [CoreCursorType.Wait] = new CoreCursor(CoreCursorType.Wait, 0),
            [CoreCursorType.Help] = new CoreCursor(CoreCursorType.Help, 0),
            [CoreCursorType.Arrow] = new CoreCursor(CoreCursorType.Arrow, 0),
            [CoreCursorType.IBeam] = new CoreCursor(CoreCursorType.IBeam, 0),
            [CoreCursorType.Cross] = new CoreCursor(CoreCursorType.Cross, 0),
            [CoreCursorType.Person] = new CoreCursor(CoreCursorType.Person, 0),
            [CoreCursorType.UpArrow] = new CoreCursor(CoreCursorType.UpArrow, 0),
            [CoreCursorType.SizeAll] = new CoreCursor(CoreCursorType.SizeAll, 0),
            [CoreCursorType.UniversalNo] = new CoreCursor(CoreCursorType.UniversalNo, 0),
            [CoreCursorType.SizeWestEast] = new CoreCursor(CoreCursorType.SizeWestEast, 0),
            [CoreCursorType.SizeNorthSouth] = new CoreCursor(CoreCursorType.SizeNorthSouth, 0),
            [CoreCursorType.SizeNortheastSouthwest] = new CoreCursor(CoreCursorType.SizeNortheastSouthwest, 0),
            [CoreCursorType.SizeNorthwestSoutheast] = new CoreCursor(CoreCursorType.SizeNorthwestSoutheast, 0),
        };
        #endregion
    }
}
