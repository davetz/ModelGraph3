using ModelGraph.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace ModelGraph.Controls
{
    public sealed partial class CanvasDrawControl
    {
        internal void ShowPicker2(int width) // this is only way to show Picker2
        {
            _picker2Width = width;
            RestorePicker2();
        }
        private int _picker2Width;
        private void RestorePicker2()
        {
            Pick2Canvas.Width = _picker2Width;
            if (_picker2Width < 4)
                HidePicker2();
            else
            {
                Pick2Canvas.IsEnabled = true;  //enable CanvasDraw
                Picker2Grid.Visibility = Visibility.Visible;
                Picker2GridColumn.Width = new GridLength(_picker2Width + Picker2Grid.Margin.Left);
            }
        }
        internal void HidePicker2()
        {
            Pick2Canvas.IsEnabled = false; //disable CanvasDraw
            Picker2GridColumn.Width = new GridLength(0);
            Picker2Grid.Visibility = Visibility.Collapsed;
        }

        private void Pick2Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SetDrawPoint1(Pick2Canvas, Model.Picker2Data, e);
            e.Handled = true;
            PostEvent(DrawEvent.Picker2Tap);
        }
    }
}
