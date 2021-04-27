using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Controls
{
    public sealed partial class CanvasDrawControl
    {
        private void ShowFlyTree()
        {
            var (x, y) = GetFlyoutPoint();
            Canvas.SetTop(FlyTreeGrid, y);
            Canvas.SetLeft(FlyTreeGrid, x);
            var sz = Model.FlyOutSize;
            FlyTreeCanvas.SetSize(sz.X, sz.Y);
            FlyTreeCanvas.IsEnabled = true;
            FlyTreeGrid.Visibility = Visibility.Visible;
        }
        private void HideFlyTree()
        {
            FlyTreeGrid.Visibility = Visibility.Collapsed;
            FlyTreeCanvas.IsEnabled = false;
        }
        private bool UpdateFlyTree()
        {
            if (FlyTreeCanvas.IsEnabled && FlyTreeGrid.Visibility == Visibility.Visible)
            {
                if (Model.FlyTreeDelta != _flyTreeDelta)
                {
                    _flyTreeDelta = Model.FlyTreeDelta;
                    return true;
                }
            }
            return false;
        }
        uint _flyTreeDelta = 9;
    }
}
