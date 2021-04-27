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

        #region StrokeStyle  ==================================================
        private CanvasStrokeStyle StrokeStyle(StrokeType s)
        {
            var ss = _strokeStyle;
            if (s != Core.StrokeType.Filled)
            {
                var sc = s & Core.StrokeType.SC_Triangle;
                var dc = s & Core.StrokeType.DC_Triangle;
                var ec = s & Core.StrokeType.EC_Triangle;
                var ds = s & Core.StrokeType.Filled;

                ss.EndCap = ec == Core.StrokeType.EC_Round ? CanvasCapStyle.Round : ec == Core.StrokeType.EC_Square ? CanvasCapStyle.Square : ec == Core.StrokeType.EC_Triangle ? CanvasCapStyle.Triangle : CanvasCapStyle.Flat;
                ss.DashCap = dc == Core.StrokeType.DC_Round ? CanvasCapStyle.Round : dc == Core.StrokeType.DC_Square ? CanvasCapStyle.Square : dc == Core.StrokeType.DC_Triangle ? CanvasCapStyle.Triangle : CanvasCapStyle.Flat;
                ss.StartCap = sc == Core.StrokeType.SC_Round ? CanvasCapStyle.Round : sc == Core.StrokeType.SC_Square ? CanvasCapStyle.Square : sc == Core.StrokeType.SC_Triangle ? CanvasCapStyle.Triangle : CanvasCapStyle.Flat;
                ss.DashStyle = ds == Core.StrokeType.Dotted ? CanvasDashStyle.Dot : ds == Core.StrokeType.Dashed ? CanvasDashStyle.Dash : CanvasDashStyle.Solid;
                ss.LineJoin = CanvasLineJoin.Round;
            }
            return ss;
        }
        private CanvasStrokeStyle _strokeStyle = new CanvasStrokeStyle();
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

        #region DrawCanvas_Draw  ==============================================
        private void DrawCanvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (_isFirstCall) { _isFirstCall = false; PanZoomReset(); }

            if (sender == EditCanvas)
            {
                Draw(Model.HelperData);
                Draw(Model.EditorData);
                if (SelectorGrid.Visibility == Visibility.Visible)
                    UpdateSelectorGrid();
            }
            else if (sender == OverCanvas) Draw(Model.EditorData);
            else if (sender == Pick1Canvas) Draw(Model.Picker1Data);
            else if (sender == Pick2Canvas) Draw(Model.Picker2Data);

            void Draw(IDrawData data)
            {
                if (data is null) return;

                var (scale, offset) = CanvasScaleOffset[sender];
                var ds = args.DrawingSession;

                foreach (var (P, (K, S, W), (A, R, G, B)) in data.Parms)
                {
                    var isFilled = S == StrokeType.Filled;
                    var V = W * scale;
                    if (V < 1) V = 1;
                    if (K < ShapeType.SimpleShapeMask)
                    {
                        var color = Color.FromArgb(A, R, G, B);
                        var stroke = StrokeStyle(S);
                        switch (K)
                        {
                            case ShapeType.Line:
                                for (int i = 0; i < P.Length; i += 2)
                                {
                                    var a = P[i] * scale + offset;
                                    var b = P[i + 1] * scale;
                                    ds.DrawLine(a, b + offset, color, V, stroke);
                                }
                                break;
                            case ShapeType.Circle:
                                if (isFilled)
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        ds.FillCircle(a, b.X, color);
                                    }
                                else
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        ds.DrawCircle(a, b.X, color, V, stroke);
                                    }
                                break;
                            case ShapeType.Ellipse:
                                if (isFilled)
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        ds.FillEllipse(a, b.X, b.Y, color);
                                    }
                                else
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        ds.DrawEllipse(a, b.X, b.Y, color, V, stroke);
                                    }
                                break;
                            case ShapeType.EqualRect:
                                var d = P[0] * scale;
                                if (isFilled)
                                    for (int i = 1; i < P.Length; i++)
                                    {
                                        var a = P[i] * scale + offset;
                                        ds.FillRectangle(a.X, a.Y, d.X, d.Y, color);
                                    }
                                else
                                    for (int i = 1; i < P.Length; i++)
                                    {
                                        var a = P[i] * scale + offset;
                                        ds.DrawRectangle(a.X, a.Y, d.X, d.Y, color, V, stroke);
                                    }
                                break;
                            case ShapeType.CornerRect:
                                if (isFilled)
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        ds.FillRectangle(a.X, a.Y, b.X, b.Y, color);
                                    }
                                else
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        ds.DrawRectangle(a.X, a.Y, b.X, b.Y, color, V, stroke);
                                    }
                                break;
                            case ShapeType.CenterRect:
                                if (isFilled)
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        var e = a - b;
                                        var f = 2 * b;
                                        ds.FillRectangle(e.X, e.Y, f.X, f.Y, color);
                                    }
                                else
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        var e = a - b;
                                        var f = 2 * b;
                                        ds.DrawRectangle(e.X, e.Y, f.X, f.Y, color, V, stroke);
                                    }
                                break;
                            case ShapeType.RoundedRect:
                                var r = 8 * scale;
                                if (isFilled)
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        var e = a - b;
                                        var f = 2 * b;
                                        ds.FillRoundedRectangle(e.X, e.Y, f.X, f.Y, r, r, color);
                                    }
                                else
                                    for (int i = 0; i < P.Length; i += 2)
                                    {
                                        var a = P[i] * scale + offset;
                                        var b = P[i + 1] * scale;
                                        var e = a - b;
                                        var f = 2 * b;
                                        ds.DrawRoundedRectangle(e.X, e.Y, f.X, f.Y, r, r, color, V, stroke);
                                    }
                                break;
                            case ShapeType.Pin2:
                                {
                                    var b = 2 * scale;
                                    if (isFilled)
                                        for (int i = 0; i < P.Length; i++)
                                        {
                                            var a = P[i] * scale + offset;
                                            ds.FillCircle(a, b, color);
                                            ds.DrawCircle(a, b + V, Colors.Black, V, stroke);
                                        }
                                    else
                                        for (int i = 0; i < P.Length; i++)
                                        {
                                            var a = P[i] * scale + offset;
                                            ds.DrawCircle(a, b - V, Colors.Black, V, stroke);
                                            ds.DrawCircle(a, b, color, V, stroke);
                                            ds.DrawCircle(a, b + V, Colors.Black, V, stroke);
                                        }
                                }
                                break;
                            case ShapeType.Pin4:
                                {
                                    var b = 4 * scale;
                                    if (isFilled)
                                        for (int i = 0; i < P.Length; i++)
                                        {
                                            var a = P[i] * scale + offset;
                                            ds.FillCircle(a, b, color);
                                            ds.DrawCircle(a, b + V, Colors.Black, V, stroke);
                                        }
                                    else
                                        for (int i = 0; i < P.Length; i++)
                                        {
                                            var a = P[i] * scale + offset;
                                            ds.DrawCircle(a, b - V, Colors.Black, V, stroke);
                                            ds.DrawCircle(a, b, color, V, stroke);
                                            ds.DrawCircle(a, b + V, Colors.Black, V, stroke);
                                        }
                                }
                                break;
                            case ShapeType.Grip2:
                                {
                                    var e = 2 * scale;
                                    var f = 2 * e;
                                    if (isFilled)
                                        for (int i = 0; i < P.Length; i++)
                                        {
                                            var a = P[i] * scale + offset;
                                            ds.FillRectangle(a.X - e, a.Y - e, f, f, color);
                                        }
                                    else
                                        for (int i = 0; i < P.Length; i += 2)
                                        {
                                            var a = P[i] * scale + offset;
                                            ds.DrawRectangle(a.X - e, a.Y - e, f, f, color, V, stroke);
                                        }
                                }
                                break;
                            case ShapeType.Grip4:
                                {
                                    var e = 4 * scale;
                                    var f = 2 * e;
                                    if (isFilled)
                                        for (int i = 0; i < P.Length; i++)
                                        {
                                            var a = P[i] * scale + offset;
                                            ds.FillRectangle(a.X - e, a.Y - e, f, f, color);
                                        }
                                    else
                                        for (int i = 0; i < P.Length; i += 2)
                                        {
                                            var a = P[i] * scale + offset;
                                            ds.DrawRectangle(a.X - e, a.Y - e, f, f, color, V, stroke);
                                        }
                                }
                                break;
                        }
                    }
                    else
                    {
                        using (var pb = new CanvasPathBuilder(ds))
                        {
                            pb.BeginFigure(P[0] * scale + offset);                          
                            if ((K & ShapeType.JointedLines) != 0 || (K & ShapeType.ClosedLines) != 0)
                            {
                                for (int i = 1; i < P.Length; i++)
                                {
                                    pb.AddLine(P[i] * scale + offset);
                                }
                            }
                            else
                            {
                                var N = P.Length;
                                for (var i = 0; i < N - 2;)
                                {
                                    pb.AddCubicBezier(P[i] * scale + offset, P[++i] * scale + offset, P[++i] * scale + offset);
                                }
                            }
                            if ((K & ShapeType.ClosedLines) != 0)
                                pb.EndFigure(CanvasFigureLoop.Closed);
                            else
                                pb.EndFigure(CanvasFigureLoop.Open);

                            using (var geo = CanvasGeometry.CreatePath(pb))
                            {
                                if (isFilled)
                                    ds.FillGeometry(geo, Color.FromArgb(A, R, G, B));
                                else
                                    ds.DrawGeometry(geo, Color.FromArgb(A, R, G, B), V, StrokeStyle(S));
                            }
                        }
                    }
                }

                foreach (var ((P, T), (A, R, G, B)) in data.Text)
                {
                    var p = P * scale + offset;
                    ds.DrawText(T, p, Color.FromArgb(A, R, G, B));
                }
            }
        }
        private bool _isFirstCall = true;
        #endregion

        #region Canavas_Loaded  ===============================================
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

        #region ModelCanvas_Unloaded  =========================================
        private void ModelCanvas_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Unloaded -= ModelCanvas_Unloaded;

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
