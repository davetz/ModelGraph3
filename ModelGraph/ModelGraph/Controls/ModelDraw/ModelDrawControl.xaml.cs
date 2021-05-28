using System;
using System.Numerics;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using ModelGraph.Core;
using Windows.UI.Xaml;
using System.Diagnostics;

namespace ModelGraph.Controls
{
    /// <summary>UI shel for CanvasDrawControl</summary>
    public sealed partial class ModelDrawControl : Page, IPageControl, IModelPageControl
    {
        public IPageModel PageModel { get; }
        IDrawModel DrawModel => PageModel.LeadModel as IDrawModel;
        private readonly CoreDispatcher _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
        public (int Width, int Height) PreferredSize => throw new System.NotImplementedException(); //STILL WANT THIS - BUT NEED TO FIGURE OUT HOW TO IMPLEMENT IT

        #region Constructor  ==================================================
        public ModelDrawControl(IPageModel model)
        {
            PageModel = model;
            InitializeComponent();

            ConfigControlBar();
            ConfigFlyTree();
            ConfigPicker1();
            ConfigOverview();
            ConfigPicker2();
            ConfigSideTree();

            CanvasScaleOffset[EditCanvas] = (0.5f, new Vector2());
            CanvasScaleOffset[OverCanvas] = (0.5f, new Vector2());
            CanvasScaleOffset[Pick1Canvas] = (0.5f, new Vector2());
            CanvasScaleOffset[Pick2Canvas] = (0.5f, new Vector2());
            ChedkDrawItems();
        }
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var height = ActualHeight - ControlGrid.ActualHeight;
            if (SideTreeCanvas.IsEnabled && SideTreeGrid.Visibility == Visibility.Visible)
            {
                var width = SideTreeGrid.ActualWidth;
                SideTreeCanvas.SetSize(width, height);
            }
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
                if (DrawModel.EditorDelta != _editCanvasDelta)
                {
                    _editCanvasDelta = DrawModel.EditorDelta;
                    return true;
                }
            }
            return false;
        }
        private bool UpdateOverCanvas()
        {
            if (OverCanvas.IsEnabled && OverviewBorder.Visibility == Visibility.Visible)
            {
                if (DrawModel.EditorDelta != _overCanvasDelta)
                {
                    _overCanvasDelta = DrawModel.EditorDelta;
                    return true;
                }
            }
            return false;
        }
        private bool UpdatePick1Canvas()
        {
            if (Pick1Canvas.IsEnabled && Pick1Canvas.Visibility == Visibility.Visible)
            {
                if (DrawModel.Picker1Delta != _pick1CanvasDelta)
                {
                    _pick1CanvasDelta = DrawModel.Picker1Delta;
                    return true;
                }
            }
            return false;
        }
        private bool UpdatePick2Canvas()
        {
            if (Pick2Canvas.IsEnabled && Pick2Canvas.Visibility == Visibility.Visible)
            {
                if (DrawModel.Picker2Delta != _pick2CanvasDelta)
                {
                    _pick2CanvasDelta = DrawModel.Picker2Delta;
                    return true;
                }
            }
            return false;
        }
        private bool UpdateToolTip()
        {
            if (ToolTipBorder.Visibility == Visibility.Visible)
            {
                if (DrawModel.ToolTipDelta != _toolTipDelta)
                {
                    _toolTipDelta = DrawModel.ToolTipDelta;
                    return true;
                }
            }
            return false;
        }
        private byte _editCanvasDelta;
        private byte _overCanvasDelta;
        private byte _pick1CanvasDelta;
        private byte _pick2CanvasDelta;
        private byte _toolTipDelta;
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
