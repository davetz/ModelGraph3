using System;
using System.Numerics;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using ModelGraph.Core;
using Windows.UI.Xaml;

namespace ModelGraph.Controls
{
    /// <summary>UI shel for CanvasDrawControl</summary>
    public sealed partial class ModelDrawControl : Page, IPageControl, IModelPageControl
    {
        public IPageModel PageModel { get; }
        IDrawModel DrawModel => PageModel.LeadModel as IDrawModel;
        private readonly CoreDispatcher _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

        public (int Width, int Height) PreferredSize => throw new System.NotImplementedException();

        #region Constructor  ==================================================
        public ModelDrawControl(IPageModel model)
        {
            PageModel = model;
            InitializeComponent();
            InitializeModeSelector();
            ConfigPicker1();
            ConfigOverview();
            if (DrawModel.DrawConfig.TryGetValue(DrawItem.Editor, out (int, SizeType) edr))
            {
                EditorGridColumn.Width = new GridLength(edr.Item1, (GridUnitType)edr.Item2);
            }
            ConfigPicker2();
            ConfigSideTree();

            if (DrawModel.FlyTreeModel is ITreeModel)
                FlyTreeCanvas.Initialize(DrawModel.FlyTreeModel);
            else
                FlyTreeCanvas.IsEnabled = false;

            if (DrawModel.SideTreeModel is ITreeModel)
            {

                SideTreeCanvas.Initialize(DrawModel.SideTreeModel);
            }
            else
                SideTreeCanvas.IsEnabled = false;

            CanvasScaleOffset[EditCanvas] = (0.5f, new Vector2());
            CanvasScaleOffset[OverCanvas] = (0.5f, new Vector2());
            CanvasScaleOffset[Pick1Canvas] = (0.5f, new Vector2());
            CanvasScaleOffset[Pick2Canvas] = (0.5f, new Vector2());
            ChedkDrawItems();
        }
        #endregion

        #region IPageControl  =================================================
        public void Reload() { }
        public void SaveAs() { }
        public void Save() { }
        public void Close() { }
        public void NewView(IPageModel model) { }
        public async void RefreshAsync()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, SetSelectedMode);

            Refresh();
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
        void SetSelectedMode()
        {
            if (ModeComboBox.SelectedIndex != DrawModel.ModeIndex)
                ModeComboBox.SelectedIndex = DrawModel.ModeIndex;
            EditCanvas.Focus(Windows.UI.Xaml.FocusState.Programmatic);
        }
        #endregion

        #region HideShowOverview  =============================================
        private void OverviewOnOffTextBlock_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if ((DrawModel.VisibleDrawItems & DrawItem.Overview) == 0)
            {
                OverviewOnOffTextBlock.Text = "\uF0AD";
                DrawModel.VisibleDrawItems |= DrawItem.Overview;
                Refresh();
            }
            else
            {
                OverviewOnOffTextBlock.Text = "\uF0AE";
                DrawModel.VisibleDrawItems &= ~DrawItem.Overview;
                Refresh();
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
                Refresh();
            }
            else
            {
                PalletOnOffTextBlock.Text = "\uF0AE";
                DrawModel.VisibleDrawItems &= ~DrawItem.Picker2;
                Refresh();
            }
        }
        #endregion

        #region CheckDrawItems  ===============================================
        private void ChedkDrawItems()
        {
            var visibleItems = DrawModel.VisibleDrawItems;
            if (visibleItems != _prevVisible)
            {
                if ((visibleItems & DrawItem.Selector) == 0 && SelectorGrid.Visibility != Visibility.Collapsed)
                    HideSelectorGrid();
                if ((visibleItems & DrawItem.Selector) != 0 && SelectorGrid.Visibility != Visibility.Visible)
                    ShowSelectorGrid();

                if ((visibleItems & DrawItem.ToolTip) == 0 && ToolTipBorder.Visibility != Visibility.Collapsed)
                    HideToolTip();
                if ((visibleItems & DrawItem.ToolTip) != 0 && ToolTipBorder.Visibility != Visibility.Visible)
                    ShowToolTip();
                if ((visibleItems & DrawItem.ToolTip) != 0 && (visibleItems & DrawItem.ToolTipChange) != (_prevVisible & DrawItem.ToolTipChange))
                    ShowToolTip();

                if ((visibleItems & DrawItem.FlyTree) == 0 && FlyTreeGrid.Visibility != Visibility.Collapsed)
                    HideFlyTree();
                if ((visibleItems & DrawItem.FlyTree) != 0 && FlyTreeGrid.Visibility != Visibility.Visible)
                    ShowFlyTree();

                if ((visibleItems & DrawItem.Picker1) == 0 && Pick1Canvas.Visibility != Visibility.Collapsed)
                    HidePicker1();
                if ((visibleItems & DrawItem.Picker1) != 0 && Pick1Canvas.Visibility != Visibility.Visible)
                    RestorePicker1();

                if ((visibleItems & DrawItem.Picker2) == 0 && Picker2Grid.Visibility != Visibility.Collapsed)
                    HidePicker2();
                if ((visibleItems & DrawItem.Picker2) != 0 && Picker2Grid.Visibility != Visibility.Visible)
                    RestorePicker2();

                if ((visibleItems & DrawItem.SideTree) == 0 && SideTreeGrid.Visibility != Visibility.Collapsed)
                    SideTreeGrid.Visibility = Visibility.Collapsed;
                if ((visibleItems & DrawItem.SideTree) != 0 && SideTreeGrid.Visibility != Visibility.Visible)
                    SideTreeGrid.Visibility = Visibility.Visible;

                if ((visibleItems & DrawItem.Overview) == 0 && OverviewBorder.Visibility != Visibility.Collapsed)
                    HideOverview();
                if ((visibleItems & DrawItem.Overview) != 0 && OverviewBorder.Visibility != Visibility.Visible)
                    RestoreOverview();

                if ((visibleItems & DrawItem.ColorPicker) == 0 && ColorPickerBorder.Visibility != Visibility.Collapsed)
                    ColorPickerBorder.Visibility = Visibility.Collapsed;
                if ((visibleItems & DrawItem.ColorPicker) != 0 && ColorPickerBorder.Visibility != Visibility.Visible)
                    ColorPickerBorder.Visibility = Visibility.Visible;

                _prevVisible = visibleItems;
            }
        }
        private DrawItem _prevVisible;
        #endregion

        #region CheckCanvasDataDelta  =========================================
        private bool UpdateEditCanvas()
        {
            if (EditCanvas.IsEnabled)
            {
                if (DrawModel.EditorData.DataDelta != _editCanvasDelta)
                {
                    _editCanvasDelta = DrawModel.EditorData.DataDelta;
                    return true;
                }
            }
            return false;
        }
        private bool UpdateOverCanvas()
        {
            if (OverCanvas.IsEnabled && OverviewBorder.Visibility == Visibility.Visible)
            {
                if (DrawModel.EditorData.DataDelta != _overCanvasDelta)
                {
                    _overCanvasDelta = DrawModel.EditorData.DataDelta;
                    return true;
                }
            }
            return false;
        }
        private bool UpdatePick1Canvas()
        {
            if (Pick1Canvas.IsEnabled && Pick1Canvas.Visibility == Visibility.Visible)
            {
                if (DrawModel.Picker1Data.DataDelta != _pick1CanvasDelta)
                {
                    _pick1CanvasDelta = DrawModel.Picker1Data.DataDelta;
                    return true;
                }
            }
            return false;
        }
        private bool UpdatePick2Canvas()
        {
            if (Pick2Canvas.IsEnabled && Pick2Canvas.Visibility == Visibility.Visible)
            {
                if (DrawModel.Picker2Data.DataDelta != _pick2CanvasDelta)
                {
                    _pick2CanvasDelta = DrawModel.Picker2Data.DataDelta;
                    return true;
                }
            }
            return false;
        }
        private uint _editCanvasDelta;
        private uint _overCanvasDelta;
        private uint _pick1CanvasDelta;
        private uint _pick2CanvasDelta;
        #endregion

        #region Refresh  ======================================================
        internal async void Refresh()
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, RefreshAll);
        }
        private void RefreshAll()
        {
            var color = Picker1Grid.Background;

            ChedkDrawItems();
            CheckColorChange();

            if (UpdateFlyTree()) FlyTreeCanvas.Refresh();
            if (UpdateSideTree()) SideTreeCanvas.Refresh();

            if (UpdateEditCanvas()) EditCanvas.Invalidate();
            if (UpdateOverCanvas()) OverCanvas.Invalidate();
            if (UpdatePick1Canvas()) Pick1Canvas.Invalidate();
            if (UpdatePick2Canvas()) Pick2Canvas.Invalidate();

            CheckDrawCursor();
        }
        private void RestoreDrawCursor() => CheckDrawCursor(true);
        private void CheckDrawCursor(bool restore = false)
        {
            var cursor = DrawModel.GetModeStateCursor();
            if (restore || cursor != _drawCursor)
            {
                _drawCursor = cursor;
                if (cursor > DrawCursor.CustomCursorsBegin)
                {
                    var id = (int)_drawCursor;
                    TrySetNewCursor(CoreCursorType.Custom, id);
                }
                else
                    TrySetNewCursor((CoreCursorType)_drawCursor);
                RootFocusButton.Focus(FocusState.Programmatic);
            }
        }
        DrawCursor _drawCursor;
        internal async void PostEvent(DrawEvent evt)
        {
            if (DrawModel.TryGetEventAction(evt, out Action action))
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { action(); });
            RefreshAll();
        }
        internal void ExecuteAction(DrawEvent evt)
        {
            if (DrawModel.TryGetEventAction(evt, out Action action))
            {
                action(); //imediately execute the drag event
                EditCanvas.Invalidate(); //and then trigger editCanvas refresh
                //RefreshAll();
            }
        }
        #endregion

        #region DrawCanvas_Loaded  ============================================
        private void DrawCanvas_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _isDrawCanvasLoaded = true;
            EditCanvas.Loaded -= DrawCanvas_Loaded;
            if (_isRootCanvasLoaded)
            {
                PanZoomReset();
            }
            DrawModel.PostRefresh();
        }
        bool _isDrawCanvasLoaded;

        #endregion

        #region CanvasDraw_Unloaded  ==========================================
        private void CanvasDraw_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Unloaded -= CanvasDraw_Unloaded;

            if (EditCanvas != null)
            {
                EditCanvas.RemoveFromVisualTree();
                EditCanvas = null;
                OverCanvas.RemoveFromVisualTree();
                OverCanvas = null;
                Pick1Canvas.RemoveFromVisualTree();
                Pick1Canvas = null;
                Pick2Canvas.RemoveFromVisualTree();
                Pick2Canvas = null;
            }
        }
        #endregion

    }
}
