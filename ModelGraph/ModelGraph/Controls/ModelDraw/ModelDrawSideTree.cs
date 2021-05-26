using ModelGraph.Core;
using System.Diagnostics;
using Windows.UI.Xaml;

namespace ModelGraph.Controls
{
    public sealed partial class ModelDrawControl
    {
        private void ConfigSideTree()
        {
            if (DrawModel.SideTreeModel is ITreeModel)
            {
                SideTreeCanvas.IsEnabled = true;
                SideTreeGridColumn.Width = new GridLength(DrawModel.SideTreeWidth, GridUnitType.Star);
                EditorGridColumn.Width = new GridLength(DrawModel.EditorWidth);
                SideTreeGrid.Visibility = Visibility.Visible;
                SideTreeCanvas.Initialize(DrawModel.SideTreeModel);
                SideTreeCanvas.SetSize(DrawModel.SideTreeWidth, DrawModel.SideTreeWidth);
                PanZoomReset();
            }
            else
            {
                SideTreeCanvas.IsEnabled = false;
                SideTreeGridColumn.Width = new GridLength(0);
                EditorGridColumn.Width = new GridLength(DrawModel.EditorWidth, GridUnitType.Star);
                SideTreeGrid.Visibility = Visibility.Collapsed;
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
        byte _sideTreeDelta = 9;
        private void SideTree_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (SideTreeCanvas.IsEnabled && SideTreeGrid.Visibility == Visibility.Visible)
            {
                var width = SideTreeGrid.ActualWidth;
                var height = ActualHeight - MainGridHeaderRow.Height.Value;
                SideTreeCanvas.SetSize(width, height);
                Debug.WriteLine($"SizeChanged W,H {width},{height}");
            }
        }
    }
}
