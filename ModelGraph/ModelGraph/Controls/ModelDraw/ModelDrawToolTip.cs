using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Controls
{
    public sealed partial class ModelDrawControl
    {
        private void ShowToolTip()
        {
            if (string.IsNullOrWhiteSpace(DrawModel.ToolTip_Text1)) HideToolTip();
            ItemName.Text = DrawModel.ToolTip_Text1;

            var offset = 60;
            if (string.IsNullOrWhiteSpace(DrawModel.ToolTip_Text2))
            {
                offset /= 2;
                ItemToolTip.Visibility = Visibility.Collapsed;
            }
            else
            {
                ItemToolTip.Visibility = Visibility.Collapsed;
                ItemToolTip.Text = DrawModel.ToolTip_Text2;
            }

            var ds = ItemToolTip.Text.Length * 4;
            var (x, y) = GetFlyoutPoint();
            x -= ds;
            y -= offset;

            Canvas.SetTop(ToolTipBorder, y);
            Canvas.SetLeft(ToolTipBorder, x);

            ToolTipBorder.Visibility = Visibility.Visible;
        }

        private void HideToolTip()
        {
            if (ToolTipBorder.Visibility == Visibility.Visible)
            {
                ToolTipBorder.Visibility = Visibility.Collapsed;
            }
        }
    }
}
