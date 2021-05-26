using ModelGraph.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace ModelGraph.Controls
{
    public sealed partial class ModelDrawControl
    {
        private void ConfigPicker2()
        {
            if (DrawModel.Picker2Data is null)
                HidePicker2();
            else
            {
                Picker2Grid.Width = Pick2Canvas.Width = DrawModel.Picker2Width < 16 ? 16 : DrawModel.Picker2Width;
                RestorePicker2();
            }
        }
        private void RestorePicker2()
        {
            Pick2Canvas.IsEnabled = true;  //enable CanvasDraw
            Picker2Grid.Visibility = Visibility.Visible;
            Picker2GridColumn.Width = new GridLength(Picker2Grid.Width + Picker2Grid.Margin.Left);
        }
        internal void HidePicker2()
        {
            Pick2Canvas.IsEnabled = false; //disable CanvasDraw
            Picker2GridColumn.Width = new GridLength(0);
            Picker2Grid.Visibility = Visibility.Collapsed;
        }

        private void Pick2Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SetDrawPoint1(Pick2Canvas, DrawModel.Picker2Data, e);
            e.Handled = true;
            PostEvent(DrawEvent.Picker2Tap);
        }
    }
}
