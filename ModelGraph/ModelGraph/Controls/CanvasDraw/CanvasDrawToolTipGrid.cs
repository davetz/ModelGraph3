using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Controls
{
    public sealed partial class CanvasDrawControl
    {
        private void ShowToolTip()
        {
            if (string.IsNullOrWhiteSpace(Model.ToolTip_Text1)) HideToolTip();
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
            var (x, y) = GetToolTipGridPoint();
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
