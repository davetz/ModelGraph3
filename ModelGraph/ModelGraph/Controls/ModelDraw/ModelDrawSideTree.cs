using System.Diagnostics;
using Windows.UI.Xaml;

namespace ModelGraph.Controls
{
    public sealed partial class ModelDrawControl
    {
        private void SideTree_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(SideTreeCanvas.IsEnabled && SideTreeGrid.Visibility == Visibility.Visible)
            {
                var width = SideTreeGrid.ActualWidth;
                var height = ActualHeight - MainGridHeaderRow.Height.Value;
                Debug.WriteLine($"SizeChanged W,H {width},{height}");
            }
        }
        private void ConfigSideTree()
        {
            if (DrawModel.DrawConfig.TryGetValue(Core.DrawItem.SideTree, out (int, Core.SizeType) sdtr))
            {
                SideTreeGridColumn.Width = new GridLength(sdtr.Item1, (GridUnitType)sdtr.Item2);
                SideTreeGrid.Visibility = Visibility.Visible;
                SideTreeCanvas.SetSize(sdtr.Item1, sdtr.Item1);
                PanZoomReset();
            }
        }
        internal void SetSideTreeSize(int width, int height)
        {
            if (SideTreeCanvas.IsEnabled)
            {
                SideTreeGrid.Visibility = Visibility.Visible;
                SideTreeCanvas.SetSize(width, height);
                PanZoomReset();
            }
        }
        private bool UpdateSideTree()
        {
            if (SideTreeCanvas.IsEnabled && SideTreeGrid.Visibility == Visibility.Visible)
            {
                if (DrawModel.SideTreeDelta != _sideTreeDelta)
                {
                    _sideTreeDelta = DrawModel.SideTreeDelta;
                    return true;
                }
            }
            return false;
        }
        uint _sideTreeDelta = 9;
    }
}
