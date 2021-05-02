using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace ModelGraph.Controls
{
    public sealed partial class ModelDrawControl
    {
        private void ColorPickerControl_ColorChanged(ColorPicker sender, ColorChangedEventArgs args) => SetNewColor(args.NewColor);
        private void ColorSampleBorder_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var (A, R, G, B) = DrawModel.ColorARGB;
            var currentColor = Color.FromArgb(A, R, G, B);
            if (ColorPickerControl.Visibility == Visibility.Visible)
            {
                HideColorPicker(currentColor);
            }
            else
            {
                ShowColorPicker(currentColor);
            }
        }
        private void UndoColorBoarder_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _pickerColor = _originalColor;
            SetNewColor(_originalColor);
            HideColorPicker(_originalColor);
        }
        private void SetNewColor(Color color)
        {
            _pickerColor = color;
            SetSampleColor(color);
            DrawModel.ColorARGB = (color.A, color.R, color.G, color.B);
            if (EditCanvas.IsEnabled) EditCanvas.Invalidate();
            if (OverCanvas.IsEnabled) OverCanvas.Invalidate();
            if (Pick1Canvas.IsEnabled) Pick1Canvas.Invalidate();
            if (Pick2Canvas.IsEnabled) Pick2Canvas.Invalidate();
        }
        private void SetSampleColor(Color color)
        {
            var (_, R, G, B) = (color.A, color.R, color.G, color.B);
            ColorSampleBoarder.Background = new SolidColorBrush(color);
            ColorSampleTextBox.Foreground = (G > R + B) ? new SolidColorBrush(Colors.Black) : (R + G + B > 400) ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White); ;
        }
        private void HideColorPicker(Color color)
        {
            UndoColorBoarder.Visibility = Visibility.Collapsed;
            ColorPickerControl.Visibility = Visibility.Collapsed;

            ColorSampleTextBox.Text = "\uF0AE";
        }
        private void ShowColorPicker(Color color)
        {
            _originalColor = _pickerColor = color;
            var brush = new SolidColorBrush(color);

            UndoColorBoarder.Background = brush;
            UndoColorBoarder.Visibility = Visibility.Visible;

            ColorSampleTextBox.Text = "\uF0AD";
            ColorSampleBoarder.Background = brush;

            ColorPickerControl.Visibility = Visibility.Visible;
            ColorPickerControl.Color = color;
        }
        private void CheckColorChange()
        {
            var (A, R, G, B) = DrawModel.ColorARGB;
            var color = Color.FromArgb(A, R, G, B);
            if (color != _pickerColor)
            {
                _pickerColor = color;
                if (ColorPickerControl.Visibility == Visibility.Visible)
                    ColorPickerControl.Color = _pickerColor;
                else
                    SetSampleColor(color);
            }
        }
        private Color _pickerColor;
        private Color _originalColor;
    }
}
