using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Controls
{
    public sealed partial class CanvasDrawControl
    {

        void ShowSelectorGrid()
        {
            UpdateSelectorGrid();
            SelectorGrid.Visibility = Visibility.Visible;           
        }

        void HideSelectorGrid() => SelectorGrid.Visibility = Visibility.Collapsed;

        void UpdateSelectorGrid()
        {
            var min = Vector2.Min(Model.GridPoint1, Model.GridPoint2);
            var size = Vector2.Abs(Model.GridPoint1 - Model.GridPoint2);

            SelectorGrid.Width = size.X;
            SelectorGrid.Height = size.Y;

            Canvas.SetTop(SelectorGrid, min.Y);
            Canvas.SetLeft(SelectorGrid, min.X);
        }
    }
}
