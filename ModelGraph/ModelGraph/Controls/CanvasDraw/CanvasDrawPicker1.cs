using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using ModelGraph.Core;

namespace ModelGraph.Controls
{
    public sealed partial class CanvasDrawControl
    {
        private void ConfigPicker1()
        {
            if (Model.DrawConfig.TryGetValue(DrawItem.Picker1, out (int, SizeType) pic))
            {
                _picker1Width = pic.Item1;
                if (Model.DrawConfig.TryGetValue(DrawItem.Overview, out (int, SizeType) ovr) && ovr.Item2 == SizeType.Fixed)
                    _picker1TopMargin = ovr.Item1;
                if ((Model.VisibleDrawItems & DrawItem.Picker1) != 0) RestorePicker1();
            }
            else
                HidePicker1();
        }
        internal void ShowPicker1(int width, int topMargin) // this is only way to show Picker1
        {
            _picker1Width = width;
            _picker1TopMargin = topMargin;
            RestorePicker1();
        }
        private int _picker1Width;
        private int _picker1TopMargin;
        private void RestorePicker1()
        {

            Pick1Canvas.Width = _picker1Width;
            if (_picker1Width < 4)
                HidePicker1();
            else
            {
                Pick1Canvas.IsEnabled = true;  //enable CanvasDraw
                Picker1Grid.Visibility = Visibility.Visible;
                Picker1GridColumn.Width = new GridLength(_picker1Width + Picker1Grid.Margin.Right);
                Picker1Grid.Margin = new Thickness(0, _picker1TopMargin, Picker1Grid.Margin.Right, 0);
            }
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
            SetDrawPoint1(Pick1Canvas, Model.Picker1Data, e);
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
                SetDrawPoint2(Pick1Canvas, Model.Picker1Data, e);
                PostEvent(DrawEvent.Picker1Drag);
            }
        }
        private bool _pickPointerIsPressed;

    }
}
