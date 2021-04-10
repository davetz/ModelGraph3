using ModelGraph.Core;
using System;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ModelGraph.Controls
{
    /// <summary>UI shel for CanvasDrawControl</summary>
    public sealed partial class ModelDrawControl : Page, IPageControl, IModelPageControl
    {
        public IPageModel PageModel { get; }
        IDrawModel DrawModel => PageModel.LeadModel as IDrawModel;
        public (int Width, int Height) PreferredSize => throw new System.NotImplementedException();

        public ModelDrawControl(IPageModel model)
        {
            PageModel = model;
            InitializeComponent();
            InitializeModeSelector();
            DrawCanvas.Initialize(DrawModel);
        }

        #region IPageControl  =================================================
        public void Reload() { }
        public void SaveAs() { }
        public void Save() { }
        public void Close() { }
        public void NewView(IPageModel model) { }
        public async void RefreshAsync()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, SetSelectedMode);

            DrawCanvas.Refresh();
        }
        public void Release() => DrawModel.Release();
        public void SetSize(double width, double height) { }
        #endregion

        #region InitializeModeSelector  =======================================
        private void InitializeModeSelector()
        {
            var idKeys = DrawModel.GetModeIdKeys();
            for (byte modeIndex = 0; modeIndex < idKeys.Count; modeIndex++)
            {
                var key = idKeys[modeIndex];
                var accKey = Root.GetAcceleratorId(key);
                var name = Root.GetNameId(key);
                var summary = Root.GetSummaryId(key);
                var itm = new ComboBoxItem();
                itm.Content = TrySetKeyboardAccelerator(modeIndex, accKey) ? $"{accKey} = {name}" : $"{name}";
                ModeComboBox.Items.Add(itm);
                ToolTipService.SetToolTip(itm, summary);
            }
        }
        private void ControlGridKeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (_acceleratorKey_ModeIndex.TryGetValue(sender, out byte index) && DrawModel.ModeIndex != index)
                DrawModel.ModeIndex = index;
        }
        private readonly Dictionary<KeyboardAccelerator, byte> _acceleratorKey_ModeIndex = new Dictionary<KeyboardAccelerator, byte>();

        #region TrySetKeyboardAccelerator  ====================================
        bool TrySetKeyboardAccelerator(byte modeIndex, string key)
        {
            if (!string.IsNullOrEmpty(key) && _virtualKeyDictionary.TryGetValue(key, out VirtualKey vkey))
            {
                var acc = new KeyboardAccelerator();
                acc.Key = vkey;
                acc.Invoked += ControlGridKeyboardAccelerator_Invoked;
                _acceleratorKey_ModeIndex[acc] = modeIndex;
                ControlGrid.KeyboardAccelerators.Add(acc);
                return true;
            }
            return false;
        }
        static Dictionary<string, VirtualKey> _virtualKeyDictionary = new Dictionary<string, VirtualKey>
        {
            ["A"] = VirtualKey.A,
            ["B"] = VirtualKey.B,
            ["C"] = VirtualKey.C,
            ["D"] = VirtualKey.D,
            ["E"] = VirtualKey.E,
            ["F"] = VirtualKey.F,
            ["G"] = VirtualKey.G,
            ["H"] = VirtualKey.H,
            ["I"] = VirtualKey.I,
            ["J"] = VirtualKey.J,
            ["K"] = VirtualKey.K,
            ["L"] = VirtualKey.L,
            ["M"] = VirtualKey.M,
            ["N"] = VirtualKey.N,
            ["O"] = VirtualKey.O,
            ["P"] = VirtualKey.P,
            ["Q"] = VirtualKey.Q,
            ["R"] = VirtualKey.R,
            ["S"] = VirtualKey.S,
            ["T"] = VirtualKey.T,
            ["U"] = VirtualKey.U,
            ["V"] = VirtualKey.V,
            ["W"] = VirtualKey.W,
            ["X"] = VirtualKey.X,
            ["Y"] = VirtualKey.Y,
            ["Z"] = VirtualKey.Z,
        };
        #endregion
        #endregion

        #region ModeComboBox_SelectionChanged  ================================
        private void ModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DrawModel.ModeIndex != (byte)ModeComboBox.SelectedIndex)            
                DrawModel.ModeIndex = (byte)ModeComboBox.SelectedIndex;
        }
        internal void PostEvent(DrawEvent evt)
        {
            if (DrawModel.TryGetEventAction(evt, out Action action))
                _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { action(); });
        }
        void SetSelectedMode()
        {
            if (ModeComboBox.SelectedIndex != DrawModel.ModeIndex)
                ModeComboBox.SelectedIndex = DrawModel.ModeIndex;
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
