using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using ModelGraph.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace ModelGraph.Controls
{
    public sealed partial class CanvasDrawControl : UserControl
    {
        public IDrawModel Model { get; private set; }
        private readonly CoreDispatcher _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
        private Vector2 GridPoint1;
        private Vector2 GridPoint2;

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
                SideTreeCanvas.Initialize(model.SideTreeModel);
            else
                SideTreeCanvas.IsEnabled = false;

            CanvasScaleOffset[EditCanvas] = (0.5f, new Vector2());
            CanvasScaleOffset[OverCanvas] = (0.5f, new Vector2());
            CanvasScaleOffset[Pick1Canvas] = (0.5f, new Vector2());
            CanvasScaleOffset[Pick2Canvas] = (0.5f, new Vector2());
            ChedkDrawItems();
        }
        #endregion

        #region LayoutControl  ================================================

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

                if ((visibleItems & DrawItem.Resizer) == 0 && ResizerGrid.Visibility != Visibility.Collapsed)
                    ResizerGrid.Visibility = Visibility.Collapsed;
                if ((visibleItems & DrawItem.Resizer) != 0 && ResizerGrid.Visibility != Visibility.Visible)
                    ResizerGrid.Visibility = Visibility.Visible;

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

        #region Overview  =====================================================
        internal void SetOverview(int width, int height, bool canResize = false)
        {
            OverviewBorder.Width = width + OverviewBorder.BorderThickness.Right;
            OverviewBorder.Height = height;
            _overviewCanResize = canResize;
            _overviewIsValid = width > 4;
            RestoreOverview();
        }
        private bool _overviewCanResize;
        private bool _overviewIsValid;
        private void RestoreOverview()
        {
            if (_overviewIsValid)
            {
                OverviewResize.Visibility = _overviewCanResize ? Visibility.Visible : Visibility.Collapsed;
                OverCanvas.IsEnabled = true;  //enable CanvasDraw
                OverviewBorder.Visibility = (Model.VisibleDrawItems & DrawItem.Overview) == 0 ? Visibility.Collapsed : Visibility.Visible; ;
                SetScaleOffset(OverCanvas, Model.EditorData);
            }
            else
                HideOverview();
        }
        internal void HideOverview()
        {
            OverCanvas.IsEnabled = false;  //disable CanvasDraw
            OverviewBorder.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region Picker1  ======================================================
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
        #endregion

        #region Picker2  ======================================================
        internal void ShowPicker2(int width) // this is only way to show Picker2
        {
            _picker2Width = width;
            RestorePicker2();
        }
        private int _picker2Width;
        private void RestorePicker2()
        {
            Pick2Canvas.Width = _picker2Width;
            if (_picker2Width < 4)
                HidePicker2();
            else
            {
                Pick2Canvas.IsEnabled = true;  //enable CanvasDraw
                Picker2Grid.Visibility = Visibility.Visible;
                Picker2GridColumn.Width = new GridLength(_picker2Width + Picker2Grid.Margin.Left);
            }
        }
        internal void HidePicker2()
        {
            Pick2Canvas.IsEnabled = false; //disable CanvasDraw
            Picker2GridColumn.Width = new GridLength(0);
            Picker2Grid.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region Set<Fly,Side>TreeSize  ========================================
        internal void SetFlyTreeSize(int width, int height)
        {
            if (FlyTreeCanvas.IsEnabled) FlyTreeCanvas.SetSize(width, height);
        }
        internal void SetSideTreeSize(int width, int height)
        {
            if (SideTreeCanvas.IsEnabled)
            {
                SideTreeGrid.Visibility = Visibility.Visible;
                SideTreeCanvas.SetSize(width, height);
                PanZoomReset();
            }
        }
        private void ShowFlyTree()
        {
            var (x, y) = GetFlyoutPoint();
            Canvas.SetTop(FlyTreeGrid, y);
            Canvas.SetLeft(FlyTreeGrid, x);
            var sz = Model.FlyOutSize;
            FlyTreeCanvas.SetSize(sz.X, sz.Y);
            FlyTreeCanvas.IsEnabled = true;
            FlyTreeGrid.Visibility = Visibility.Visible;

        }
        private void HideFlyTree()
        {
            FlyTreeGrid.Visibility = Visibility.Collapsed;
            FlyTreeCanvas.IsEnabled = false;
        }
        #endregion

        #endregion

        #region OverviewResize  ===============================================

        private void OverviewResize_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!_editPointerIsPressed) TrySetNewCursor(CoreCursorType.SizeAll);
        }
        private void OverviewResize_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!_editPointerIsPressed) RestorePointerCursor();
        }

        private void OverviewResize_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _editPointerIsPressed = true;
            GridPoint1 = new Vector2(0, 0);
            GridPoint2 = GridPoint(e);
            OverridePointerMoved(ResizingOverview);
            OverridePointerReleased(ClearPointerOverrides);
        }
        private void ResizingOverview()
        {
            var size = Vector2.Abs(GridPoint1 - GridPoint2);
            if (size.X < OverviewBorder.MinWidth) return;
            if (size.Y < OverviewBorder.MinHeight) return;

            OverviewBorder.Width = size.X;
            OverviewBorder.Height = size.Y;
            SetScaleOffset(OverCanvas, Model.EditorData);
            OverCanvas.Invalidate();
        }
        #endregion

        #region PanZoom  ======================================================
        private const float maxScale = 10;
        private const float minZoomDiagonal = 8000;
        private readonly Dictionary<CanvasControl, (float, Vector2)> CanvasScaleOffset = new Dictionary<CanvasControl, (float, Vector2)>(4);

        private void Pan(Vector2 adder)
        {
        }
        private void Zoom(float changeFactor)
        {
        }
        private void ZoomToExtent()
        {
        }
        private void PanZoomReset()
        {
            if (EditCanvas.IsEnabled)
            {
                SetScaleOffset(EditCanvas, Model.EditorData);
                EditCanvas.Invalidate();
            }
            if (OverCanvas.IsEnabled)
            {
                SetScaleOffset(OverCanvas, Model.EditorData);
                OverCanvas.Invalidate();
            }
            if (Pick1Canvas.IsEnabled)
            {
                SetScaleOffset(Pick1Canvas, Model.Picker1Data);
                Pick1Canvas.Invalidate();
            }
            if (Pick2Canvas.IsEnabled)
            {
                SetScaleOffset(Pick2Canvas, Model.Picker2Data);
                Pick2Canvas.Invalidate();
            }
        }
        private void SetScaleOffset(CanvasControl canvas, IDrawData data)
        {
            if (!canvas.IsEnabled || data is null) return;

            var aw = (float)canvas.ActualWidth;
            var ah = (float)canvas.ActualHeight;

            var e = data.Extent;
            var ew = (float)e.Width;
            var eh = (float)e.Hieght;

            if (aw < 1) aw = 1;
            if (ah < 1) ah = 1;
            if (ew < 1) ew = 1;
            if (eh < 1) eh = 1;

            var zw = aw / ew;
            var zh = ah / eh;
            var z = (zw < zh) ? zw : zh;

            // zoom required to make the view extent fit the canvas
            if (z > maxScale) z = maxScale;

            var ec = new Vector2(e.CenterX, e.CenterY) * z; //center point of scaled view extent
            var ac = eh == 1 ? new Vector2( aw /2, aw / 2) : new Vector2(aw / 2, ah / 2); //center point of the canvas

            CanvasScaleOffset[canvas] = (z, ac - ec);
        }
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

        #region CheckTreeModelDelta  ==========================================
        private bool UpdateFlyTree()
        {
            if (FlyTreeCanvas.IsEnabled && FlyTreeGrid.Visibility == Visibility.Visible)
            {
                if (Model.FlyTreeDelta != _flyTreeDelta)
                {
                    _flyTreeDelta = Model.FlyTreeDelta;
                    return true;
                }
            }
            return false;
        }
        private bool UpdateSideTree()
        {
            if (SideTreeCanvas.IsEnabled && SideTreeGrid.Visibility == Visibility.Visible)
            {
                if (Model.SideTreeDelta != _sideTreeDelta)
                {
                    _sideTreeDelta = Model.SideTreeDelta;
                    return true;
                }
            }
            return false;
        }
        uint _flyTreeDelta = 9;
        uint _sideTreeDelta =9;
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
            var cursor = Model.GetDrawStateCursor();
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
            if (Model.TryGetDrawEventAction(evt, out Action action))
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { action(); });
            RefreshAll();
        }
        internal void ExecuteAction(DrawEvent evt)
        {
            if (Model.TryGetDrawEventAction(evt, out Action action))
            {
                action(); //imediately execute the drag event
                EditCanvas.Invalidate(); //and then trigger editCanvas refresh
                RefreshAll();
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
                    if (K < ShapeType.SimpleShapeMask)
                    {
                        var color = Color.FromArgb(A, R, G, B);
                        var stroke = StrokeStyle(S);
                        var v = W * scale;
                        if (v < 1) v = 1;
                        switch (K)
                        {
                            case ShapeType.Line:
                                for (int i = 0; i < P.Length; i += 2)
                                {
                                    var a = P[i] * scale + offset;
                                    var b = P[i + 1] * scale;
                                    ds.DrawLine(a, b + offset, color, v, stroke);
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
                                        ds.DrawCircle(a, b.X, color, v, stroke);
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
                                        ds.DrawEllipse(a, b.X, b.Y, color, v, stroke);
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
                                        ds.DrawRectangle(a.X, a.Y, d.X, d.Y, color, v, stroke);
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
                                        ds.DrawRectangle(a.X, a.Y, b.X, b.Y, color, v, stroke);
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
                                        ds.DrawRectangle(e.X, e.Y, f.X, f.Y, color, v, stroke);
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
                                        ds.DrawRoundedRectangle(e.X, e.Y, f.X, f.Y, r, r, color, v, stroke);
                                    }
                                break;
                        }
                    }
                    else
                    {
                        var V = W * scale;
                        if (V < 1) V = 1;

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
        }
        bool _isDrawCanvasLoaded;

        private void RootCanvas_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _isRootCanvasLoaded = true;
            RootCanvas.Loaded -= RootCanvas_Loaded;
            if (_isDrawCanvasLoaded) PanZoomReset();
        }
        bool _isRootCanvasLoaded;
        #endregion


        #region RootCanas_ContextRuested  =====================================
        private void RootCanas_ContextRuested(Windows.UI.Xaml.UIElement sender, ContextRequestedEventArgs args)
        {
            if (args.TryGetPosition(RootCanvas, out Point p))
            {
                args.Handled = true;
                PostEvent(DrawEvent.ContextMenu);
            }
        }
        #endregion

        #region RootCanvas_DoubleTapped  ======================================
        private void RootCanvas_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) => PanZoomReset();
        #endregion

        #region PointerMoved  =================================================
        private void RootCanvas_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_isRootCanvasLoaded)
            {
                var p = e.GetCurrentPoint(EditCanvas).Position;
                if (p.X < 0 || p.Y < 0) return;

                SetGridPoint2(e);
                SetDrawPoint2(EditCanvas, Model.EditorData, e);

                e.Handled = true;

                if (_overridePointerMoved is null)
                {
                    if (_editPointerIsPressed)
                    {
                        if (_editCtrlPointerPressed)
                            ExecuteAction(DrawEvent.CtrlDrag); // we want a fast responce
                        else if (_editShiftPointerPressed)
                            ExecuteAction(DrawEvent.ShiftDrag); // we want a fast responce
                        else
                            ExecuteAction(DrawEvent.Drag);      // we want a fast responce
                    }
                    else
                        PostEvent(DrawEvent.Skim);
                }
                else
                    _overridePointerMoved();
            }
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
        private bool _editPointerIsPressed;
        private bool _editCtrlPointerPressed;
        private bool _editShiftPointerPressed;
        #endregion

        #region PointerPressed  ===============================================
        private void RootCanvas_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _editPointerIsPressed = true;
            _editCtrlPointerPressed = e.KeyModifiers.HasFlag(Windows.System.VirtualKeyModifiers.Control);
            _editShiftPointerPressed = e.KeyModifiers.HasFlag(Windows.System.VirtualKeyModifiers.Shift);
            SetGridPoint1(e);
            SetDrawPoint1(EditCanvas, Model.EditorData, e);
            e.Handled = true;

            if (_overridePointerPressed is null)
            {
                if (_editCtrlPointerPressed)
                    PostEvent(DrawEvent.CtrlTap);
                else if (_editShiftPointerPressed)
                    PostEvent(DrawEvent.ShiftTap);
                else
                    PostEvent(DrawEvent.Tap);
            }
            else
                _overridePointerPressed();
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
        private void Pick2Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SetDrawPoint1(Pick2Canvas, Model.Picker2Data, e);
            e.Handled = true;
            PostEvent(DrawEvent.Picker2Tap);
        }
        private void OverCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;
            PostEvent(DrawEvent.OverviewTap);
        }
        #endregion

        #region PointerReleased  ==============================================
        private void RootCanvas_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _editPointerIsPressed = false;
            SetGridPoint2(e);
            SetDrawPoint2(EditCanvas, Model.EditorData, e);
            e.Handled = true;

            if (_overridePointerReleased is null)
                PostEvent(DrawEvent.TapEnd);
            else
                _overridePointerReleased();
        }
        private void Pick1Canvas_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _pickPointerIsPressed = false;
            e.Handled = true;
        }
        #endregion

        #region PointerEventOverride  =========================================
        private void OverridePointerMoved(Action action) => _overridePointerMoved = action;
        private void OverridePointerPressed(Action action) => _overridePointerPressed = action;
        private void OverridePointerReleased(Action action) => _overridePointerReleased = action;
        private void ClearPointerOverrides()
        {
            RestorePointerCursor();
            _overridePointerPressed = _overridePointerMoved = _overridePointerReleased = null;
        }

        private Action _overridePointerMoved;
        private Action _overridePointerPressed;
        private Action _overridePointerReleased;
        #endregion

        #region KeyboardAccelerators  =========================================
        private void KeyboardAccelerator_AKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.AKey);
        private void KeyboardAccelerator_BKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.BKey);
        private void KeyboardAccelerator_CKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.CKey);
        private void KeyboardAccelerator_DKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.DKey);
        private void KeyboardAccelerator_EKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.EKey);
        private void KeyboardAccelerator_FKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.FKey);
        private void KeyboardAccelerator_GKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.GKey);
        private void KeyboardAccelerator_LKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.LKey);
        private void KeyboardAccelerator_MKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.MKey);
        private void KeyboardAccelerator_NKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.NKey);
        private void KeyboardAccelerator_OKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.OKey);
        private void KeyboardAccelerator_PKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.PKey);
        private void KeyboardAccelerator_QKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.QKey);
        private void KeyboardAccelerator_RKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.RKey);
        private void KeyboardAccelerator_SKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.SKey);
        private void KeyboardAccelerator_TKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.TKey);
        private void KeyboardAccelerator_UKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.UKey);
        private void KeyboardAccelerator_VKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.VKey);
        private void KeyboardAccelerator_WKey_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.WKey);

        private void KeyboardAccelerator_Escape_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.Escape);
        private void KeyboardAccelerator_UpArrow_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.UpArrowKey);
        private void KeyboardAccelerator_DownArrow_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.DownArrowKey);
        private void KeyboardAccelerator_LeftArrow_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.LeftArrowKey);
        private void KeyboardAccelerator_RightArrow_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TestDrawEvent(DrawEvent.RightArrowKey);

        private void TestDrawEvent(DrawEvent evt)
        {
            if (Model.TryGetDrawEventAction(evt, out _))
            {
                PostEvent(evt);
            }
        }
        private void RootCanvas_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            RestorePointerCursor();
            SideTreeCanvas.Focus(FocusState.Programmatic);
        }
        private void RootCanvas_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            RestoreDrawCursor();
        }
        #endregion


        #region HelperMethods  ================================================
        private void SetGridPoint1(PointerRoutedEventArgs e) => GridPoint1 = GridPoint(e);
        private void SetGridPoint2(PointerRoutedEventArgs e) => GridPoint2 = GridPoint(e);
        private void SetDrawPoint1(CanvasControl canvas, IDrawData data, PointerRoutedEventArgs e) => data.Point1 = DrawPoint(canvas, e);
        private void SetDrawPoint2(CanvasControl canvas, IDrawData data, PointerRoutedEventArgs e) => data.Point2 = DrawPoint(canvas, e);
        private Vector2 GridPoint(PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(RootGrid).Position;
            return new Vector2((float)p.X, (float)p.Y);
        }
        private Vector2 DrawPoint(CanvasControl canvas, PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(canvas).Position;
            var (scale, offset) = CanvasScaleOffset[canvas];
            var x = (p.X - offset.X) / scale;
            var y = (p.Y - offset.Y) / scale;
            return new Vector2((float)x, (float)y);
        }
        private (float top, float left, float width, float height) GetResizerParams()
        {
            var (scale, offset) = CanvasScaleOffset[EditCanvas];
            var (x1, y1, x2, y2) = Model.ResizerExtent.GetFloat();

            var dx = x2 - x1;
            var dy = y2 - y1;

            var width = dx * scale;
            var height = dy * scale;

            var top = y1 * scale + offset.X;
            var left = x1 * scale + offset.Y;

            return (top, left, width, height);
        }
        private (float,float) GetFlyoutPoint()
        {
            var (scale, offset) = CanvasScaleOffset[EditCanvas];
            var p = Model.FlyOutPoint * scale + offset;
            return (p.X, p.Y);
        }
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

        #region ColorPickerConrol  ============================================
        private void ColorPickerControl_ColorChanged(ColorPicker sender, ColorChangedEventArgs args) => SetNewColor(args.NewColor);
        private void ColorSampleBorder_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var (A, R, G, B) = Model.ColorARGB;
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
            Model.ColorARGB = (color.A, color.R, color.G, color.B);
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
            var (A, R, G, B) = Model.ColorARGB;
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
        #endregion

    }
}
