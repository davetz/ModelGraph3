using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using ModelGraph.Core;

namespace ModelGraph.Controls
{
    public sealed partial class CanvasDrawControl : UserControl
    {
        public IDrawModel Model { get; private set; }
        private readonly CoreDispatcher _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

        #region Constructor/Initialize  =======================================
        public CanvasDrawControl()
        {
            InitializeComponent();
            HidePicker1();
            HidePicker2();
            HideOverview();
        }

        public void Initialize(IDrawModel model)
        {
            Model = model;
            ConfigPicker1();
            ConfigOverview();
            if (model.DrawConfig.TryGetValue(DrawItem.Editor, out (int,SizeType) edr))
            {
                EditorGridColumn.Width = new GridLength(edr.Item1, (GridUnitType)edr.Item2);
            }
            ConfigPicker2();
            ConfigSideTree();

            if (model.FlyTreeModel is ITreeModel)
                FlyTreeCanvas.Initialize(model.FlyTreeModel);
            else
                FlyTreeCanvas.IsEnabled = false;

            if (model.SideTreeModel is ITreeModel)
            {

                SideTreeCanvas.Initialize(model.SideTreeModel);
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

        #region CheckDrawItems  ===============================================
        private void ChedkDrawItems()
        {
            var visibleItems = Model.VisibleDrawItems;
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
                if (Model.EditorData.DataDelta != _editCanvasDelta)
                {
                    _editCanvasDelta = Model.EditorData.DataDelta;
                    return true;
                }
            }
            return false;
        }
        private bool UpdateOverCanvas()
        {
            if (OverCanvas.IsEnabled && OverviewBorder.Visibility == Visibility.Visible)
            {
                if (Model.EditorData.DataDelta != _overCanvasDelta)
                {
                    _overCanvasDelta = Model.EditorData.DataDelta;
                    return true;
                }
            }
            return false;
        }
        private bool UpdatePick1Canvas()
        {
            if (Pick1Canvas.IsEnabled && Pick1Canvas.Visibility == Visibility.Visible)
            {
                if (Model.Picker1Data.DataDelta != _pick1CanvasDelta)
                {
                    _pick1CanvasDelta = Model.Picker1Data.DataDelta;
                    return true;
                }
            }
            return false;
        }
        private bool UpdatePick2Canvas()
        {
            if (Pick2Canvas.IsEnabled && Pick2Canvas.Visibility == Visibility.Visible)
            {
                if (Model.Picker2Data.DataDelta != _pick2CanvasDelta)
                {
                    _pick2CanvasDelta = Model.Picker2Data.DataDelta;
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
            var cursor = Model.GetModeStateCursor();
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
            if (Model.TryGetEventAction(evt, out Action action))
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { action(); });
            RefreshAll();
        }
        internal void ExecuteAction(DrawEvent evt)
        {
            if (Model.TryGetEventAction(evt, out Action action))
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
            Model.PostRefresh();
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
