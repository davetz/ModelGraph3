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

            ConfigDrawItems();
            ConfigFlyTree();
            ConfigPicker1();
            ConfigPicker2();
            ConfigOverview();
            ConfigSideTree();
            ConfigControlBar();
            ConfigColorPicker();

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


        #region CheckDrawItems  ===============================================
        private void ChedkDrawItems()
        {
            if (DrawModel.SelectorIsVisible != _selectorIsVisible)
            {
                _selectorIsVisible = DrawModel.SelectorIsVisible;
                if (_selectorIsVisible) ShowSelectorGrid(); else HideSelectorGrid();
            }
            if (DrawModel.ToolTipIsVisible != _toolTipIsVisible)
            {
                _toolTipIsVisible = DrawModel.ToolTipIsVisible;
                if (_toolTipIsVisible) ShowToolTip(); else HideToolTip();
            }
            else if (_toolTipIsVisible && UpdateToolTip())
                ShowToolTip();

            if (DrawModel.FlyTreeIsVisible != _flyTreeIsVisible)
            {
                _flyTreeIsVisible = DrawModel.FlyTreeIsVisible;
                if (_flyTreeIsVisible) ShowFlyTree(); else HideFlyTree();
            }

            if (DrawModel.LeftCanvasIsVisible != _picker1IsVisible)
            {
                _picker1IsVisible = DrawModel.LeftCanvasIsVisible;
                if (_picker1IsVisible) RestorePicker1(); else HidePicker1();
            }

            if (DrawModel.RightCanvasIsVisible != _picker2IsVisible)
            {
                _picker2IsVisible = DrawModel.RightCanvasIsVisible;
                if (_picker2IsVisible) RestorePicker2(); else HidePicker2();
            }

            if (DrawModel.SideTreeIsVisible != _sideTreeIsVisible)
            {
                _sideTreeIsVisible = DrawModel.SideTreeIsVisible;
                SideTreeGrid.Visibility = _sideTreeIsVisible ? Visibility.Visible : Visibility.Collapsed;
            }

            if (DrawModel.OverviewIsVisible != _overviewIsVisible)
            {
                _overviewIsVisible = DrawModel.OverviewIsVisible;
                if (_overviewIsVisible) RestoreOverview(); else HideOverview();
            }
        }
        private void ConfigDrawItems()
        {
            _toolTipIsVisible = !DrawModel.ToolTipIsVisible;
            _picker1IsVisible = !DrawModel.LeftCanvasIsVisible;
            _picker2IsVisible = !DrawModel.RightCanvasIsVisible;
            _selectorIsVisible = !DrawModel.SelectorIsVisible;
            _flyTreeIsVisible = !DrawModel.FlyTreeIsVisible;
            _sideTreeIsVisible = !DrawModel.SideTreeIsVisible;
            _overviewIsVisible = !DrawModel.OverviewIsVisible;
        }
        private bool _toolTipIsVisible;
        private bool _picker1IsVisible;
        private bool _picker2IsVisible;
        private bool _selectorIsVisible;
        private bool _flyTreeIsVisible;
        private bool _sideTreeIsVisible;
        private bool _overviewIsVisible;
        #endregion

        #region CheckDataDelta  ===============================================
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
            UpdateUndoRedoControls();

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
            DrawModel.DrawControlReady();
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
