using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Controls
{
    public sealed partial class DrawCanvasControl
    {
        void ShowAlignmentGrid()
        {
            PositionAlignmentGrid();
            AlignmentGrid.Visibility = Visibility.Visible;
        }

        void HideAlignmentGrid()
        {
            _model.ClearRegion();
            AlignmentGrid.Visibility = Visibility.Collapsed;
        }

        void PositionAlignmentGrid()
        {
            var min = Vector2.Min(_model.GridPoint1, _model.GridPoint2);
            var size = Vector2.Abs(_model.GridPoint1 - _model.GridPoint2);

            AlignmentGrid.Width = size.X + (float)SelectorBorder.Margin.Right;
            AlignmentGrid.Height = size.Y;

            Canvas.SetTop(AlignmentGrid, min.Y);
            Canvas.SetLeft(AlignmentGrid, min.X);

            _model.RegionPoint1 = Vector2.Min(_model.DrawPoint1, _model.DrawPoint2);
            _model.RegionPoint2 = Vector2.Max(_model.DrawPoint1, _model.DrawPoint2);
        }
    }
}
