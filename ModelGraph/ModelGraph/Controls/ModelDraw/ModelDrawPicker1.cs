using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using ModelGraph.Core;

namespace ModelGraph.Controls
{
    public sealed partial class ModelDrawControl
    {
        private void ConfigPicker1()
        {
            if (DrawModel.Picker1Data is IDrawData)
            {
                Pick1Canvas.Width = DrawModel.Picker1Width < 16 ? 16 : DrawModel.Picker1Width;
                Picker1Grid.Margin = (DrawModel.OverviewData is IDrawData) ? new Thickness(0, DrawModel.OverviewWidth, Picker1Grid.Margin.Right, 0) : new Thickness(0, 0, Picker1Grid.Margin.Right, 0);
                RestorePicker1();
            }
            else
                HidePicker1();
        }
        private void RestorePicker1()
        {
            if (DrawModel.Picker1Data is IDrawData)
            {
                Pick1Canvas.IsEnabled = true;  //enable CanvasDraw
                Picker1Grid.Visibility = Visibility.Visible;
                Picker1GridColumn.Width = new GridLength(Pick1Canvas.Width + Picker1Grid.Margin.Right);
            }
            else
                HidePicker1();
        }
        internal void HidePicker1()
        {
            Pick1Canvas.IsEnabled = false;  //disable CanvasDraw
            Picker1GridColumn.Width = new GridLength(0);
            Picker1Grid.Visibility = Visibility.Collapsed;
        }

        private void Pick1Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _pickPointerIsPressed = true;
            SetDrawPoint1(Pick1Canvas, DrawModel.Picker1Data, e);
            e.Handled = true;

            if (e.KeyModifiers == Windows.System.VirtualKeyModifiers.Control)
                PostEvent(DrawEvent.Picker1CtrlTap);
            else
                PostEvent(DrawEvent.Picker1Tap);
        }
        private void Pick1Canvas_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _pickPointerIsPressed = false;
            e.Handled = true;
        }
        private void Pick1Canvas_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            e.Handled = true;
            if (_pickPointerIsPressed)
            {
                SetDrawPoint2(Pick1Canvas, DrawModel.Picker1Data, e);
                PostEvent(DrawEvent.Picker1Drag);
            }
        }
        private bool _pickPointerIsPressed;

    }
}
