using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Controls
{
    public sealed partial class CanvasDrawControl
    {
        private bool _isToolTipVisible;

        private void ShowTooltip()
        {
            if (string.IsNullOrWhiteSpace(Model.ToolTip_Text1)) HideTootlip();
            ItemName.Text = Model.ToolTip_Text1;

            var offset = 60;
            if (string.IsNullOrWhiteSpace(Model.ToolTip_Text2))
            {
                offset /= 2;
                ItemToolTip.Visibility = Visibility.Collapsed;
            }
            else
            {
                ItemToolTip.Visibility = Visibility.Collapsed;
                ItemToolTip.Text = Model.ToolTip_Text2;
            }

            var ds = ItemToolTip.Text.Length * 4;
            var x = Model.GridPoint2.X - ds;
            var y = Model.GridPoint2.Y - offset;

            Canvas.SetTop(ToolTipBorder, y);
            Canvas.SetLeft(ToolTipBorder, x);

            ToolTipBorder.Visibility = Visibility.Visible;
            _isToolTipVisible = true;
        }

        private void HideTootlip()
        {
            if (_isToolTipVisible)
            {
                _isToolTipVisible = false;
                ToolTipBorder.Visibility = Visibility.Collapsed;
            }
        }
    }
}
