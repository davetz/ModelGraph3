using Windows.UI.Xaml;

namespace ModelGraph.Controls
{
    public sealed partial class CanvasDrawControl
    {
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
