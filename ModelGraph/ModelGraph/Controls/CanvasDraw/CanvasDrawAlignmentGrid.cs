using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Controls
{
    public sealed partial class CanvasDrawControl
    {
        void ShowAlignmentGrid()
        {
            PositionAlignmentGrid();
            AlignmentGrid.Visibility = Visibility.Visible;
        }

        void HideAlignmentGrid()
        {
            //Model.ClearRegion();
            AlignmentGrid.Visibility = Visibility.Collapsed;
        }

        void PositionAlignmentGrid()
        {
            var min = Vector2.Min(Model.GridPoint1, Model.GridPoint2);
            var size = Vector2.Abs(Model.GridPoint1 - Model.GridPoint2);

            AlignmentGrid.Width = size.X + (float)SelectorBorder.Margin.Right;
            AlignmentGrid.Height = size.Y;

            Canvas.SetTop(AlignmentGrid, min.Y);
            Canvas.SetLeft(AlignmentGrid, min.X);

            //Model.RegionPoint1 = Vector2.Min(Model.DrawPoint1, Model.DrawPoint2);
            //Model.RegionPoint2 = Vector2.Max(Model.DrawPoint1, Model.DrawPoint2);
        }
    }
}
