using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Controls
{
    public sealed partial class DrawCanvasControl
    {

        void ShowSelectorGrid()
        {
            UpdateSelectorGrid();
            SelectorGrid.Visibility = Visibility.Visible;           
        }

        void HideSelectorGrid() => SelectorGrid.Visibility = Visibility.Collapsed;

        void UpdateSelectorGrid()
        {
            var min = Vector2.Min(_model.GridPoint1, _model.GridPoint2);
            var size = Vector2.Abs(_model.GridPoint1 - _model.GridPoint2);

            SelectorGrid.Width = size.X;
            SelectorGrid.Height = size.Y;

            Canvas.SetTop(SelectorGrid, min.Y);
            Canvas.SetLeft(SelectorGrid, min.X);
        }
    }
}
