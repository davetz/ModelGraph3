using ModelGraph.Core;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ModelGraph.Controls
{
    public sealed partial class ModelDrawControl
    {
        private void ConfigControlBar()
        {
            ApplyButton.Visibility = RevertButton.Visibility = (DrawModel.HasApplyRevert) ? Visibility.Visible : Visibility.Collapsed;
            UndoButton.Visibility = RedoButton.Visibility = UndoCount.Visibility = RedoCount.Visibility = (DrawModel.HasUndoRedo) ? Visibility.Visible : Visibility.Collapsed;
            OverviewOnOffBorder.Visibility = (DrawModel.Picker1Data is IDrawData) ? Visibility.Collapsed : Visibility.Visible;

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
        static readonly Dictionary<string, VirtualKey> _virtualKeyDictionary = new Dictionary<string, VirtualKey>
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


        #region UndoRedo  =====================================================
        private void UndoButton_Click(object sender, RoutedEventArgs e) => TryUndo();
        private void RedoButton_Click(object sender, RoutedEventArgs e) => TryRedo();

        private void UpdateUndoRedoControls()
        {
            //var (canUndo, canRedo, undoCount, redoCount) = _graph.UndoRedoParms;

            //UndoButton.IsEnabled = canUndo;
            //RedoButton.IsEnabled = canRedo;
            //UndoCount.Text = undoCount.ToString();
            //RedoCount.Text = redoCount.ToString();
        }
        private void TryUndo()
        {
            //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { _graph.TryUndo(); _graph.AdjustGraph(); });
            //PostRefresh();
        }
        private void TryRedo()
        {
            //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { _graph.TryRedo(); _graph.AdjustGraph(); });
            //PostRefresh();
        }
        #endregion
    }
}
