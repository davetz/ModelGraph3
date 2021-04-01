using ModelGraph.Core;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ModelGraph.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ModelDrawControl : Page, IPageControl
    {
        public IPageModel PageModel { get; }
        IDrawModel DrawModel => PageModel.LeadModel as IDrawModel;
        public (int Width, int Height) PreferredSize => throw new System.NotImplementedException();

        public ModelDrawControl()
        {
            this.InitializeComponent();
        }

        #region IPageControl  =================================================
        public void Reload()
        {
        }
        public void SaveAs()
        {
        }
        public void Save()
        {
        }
        public void Close()
        {
        }
        public void NewView(IPageModel model)
        {
        }
        public async void RefreshAsync()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, SetSelectedMode);

            DrawCanvas.Refresh();
        }
        #endregion


        #region ModeComboBox_SelectionChanged  ================================
        private object _forcedSelectedMode;
        private object _previousSelectedMode;
        private void ModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var itm = ModeComboBox.SelectedItem;
            if (itm != _forcedSelectedMode)
            {
            }
        }
        internal void PostEvent(DrawEvent evt)
        {
            if (DrawModel.TryGetEventAction(evt, out Action action))
                _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { action(); });
        }
        void SetSelectedMode()
        {
            if (_forcedSelectedMode != _previousSelectedMode)
                ModeComboBox.SelectedItem = _forcedSelectedMode;

            DrawCanvas.Focus(Windows.UI.Xaml.FocusState.Programmatic);
        }
        #endregion


        #region HideShowOverview  =============================================
        private void OverviewOnOffTextBlock_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if ((DrawModel.VisibleDrawItems & DrawItem.Overview) == 0)
            {
                OverviewOnOffTextBlock.Text = "\uF0AD";
                DrawModel.VisibleDrawItems |= DrawItem.Overview;
                DrawCanvas.Refresh();
            }
            else
            {
                OverviewOnOffTextBlock.Text = "\uF0AE";
                DrawModel.VisibleDrawItems &= ~DrawItem.Overview;
                DrawCanvas.Refresh();
            }
        }
        #endregion

        #region HideShowPallet  ===============================================
        private void PalletOnOffTextBlock_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if ((DrawModel.VisibleDrawItems & DrawItem.Picker2) == 0)
            {
                PalletOnOffTextBlock.Text = "\uF0AD";
                DrawModel.VisibleDrawItems |= DrawItem.Picker2;
                DrawCanvas.Refresh();
            }
            else
            {
                PalletOnOffTextBlock.Text = "\uF0AE";
                DrawModel.VisibleDrawItems &= ~DrawItem.Picker2;
                DrawCanvas.Refresh();
            }
        }
        #endregion
    }
}
