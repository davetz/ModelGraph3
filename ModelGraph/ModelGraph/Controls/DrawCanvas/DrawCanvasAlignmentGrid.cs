using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Controls
{
    public sealed partial class DrawCanvasControl
    {
        void ShowAlignmentGrid()
        {
            SelectorSelectorGrid();
            AlignmentGrid.Visibility = Visibility.Visible;
        }

        void HideAlignmentGrid() => AlignmentGrid.Visibility = Visibility.Collapsed;

        void SelectorSelectorGrid()
        {
            var min = Vector2.Min(_selector.GridPoint1, _selector.GridPoint2);
            var size = Vector2.Abs(_selector.GridPoint1 - _selector.GridPoint2);

            AlignmentGrid.Width = size.X + (float)SelectorBorder.Margin.Right;
            AlignmentGrid.Height = size.Y;

            Canvas.SetTop(AlignmentGrid, min.Y);
            Canvas.SetLeft(AlignmentGrid, min.X);
        }
    }
}
