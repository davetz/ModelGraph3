using Windows.UI.Xaml;

namespace ModelGraph.Controls
{
    public sealed partial class CanvasDrawControl
    {
        private void ConfigSideTree()
        {
            if (Model.DrawConfig.TryGetValue(Core.DrawItem.SideTree, out (int, Core.SizeType) sdtr))
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
                if (Model.SideTreeDelta != _sideTreeDelta)
                {
                    _sideTreeDelta = Model.SideTreeDelta;
                    return true;
                }
            }
            return false;
        }
        uint _sideTreeDelta = 9;
    }
}
