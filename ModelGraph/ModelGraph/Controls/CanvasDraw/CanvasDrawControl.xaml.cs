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
        }
        #endregion

        #region LayoutControl  ================================================

        #region Overview  =====================================================
        internal void SetOverview(int width, int height, bool canResize = false)
        {
            OverviewBorder.Width = width + OverviewBorder.BorderThickness.Right;
            OverviewBorder.Height = height;
            OverviewResize.Visibility = canResize ? Visibility.Visible : Visibility.Collapsed;

            if (width < 4)
                HideOverview();
            else
            {
                _overviewIsValid = true; // enable method ShowOverview()
                ShowOverview();
            }
        }
        private bool _overviewIsValid;
        internal void ShowOverview()
        {
            if (_overviewIsValid)
            {
                OverCanvas.IsEnabled = true;  //enable CanvasDraw
                OverviewBorder.Visibility = Visibility.Visible;
            }
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

            Pick1Canvas.Width = width;
            if (width < 4)
                HidePicker1();
            else
            {
                Pick1Canvas.IsEnabled = true;  //enable CanvasDraw
                Picker1Grid.Visibility = Visibility.Visible;
                Picker1GridColumn.Width = new GridLength(width + Picker1Grid.Margin.Right);
                Picker1Grid.Margin = new Thickness(0, topMargin, Picker1Grid.Margin.Right, 0);
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
            Pick2Canvas.Width = width;
            if (width < 4)
                HidePicker2();
            else
            {
                Pick2Canvas.IsEnabled = true;  //enable CanvasDraw
                Picker2Grid.Visibility = Visibility.Visible;
                Picker2GridColumn.Width = new GridLength(width + Picker2Grid.Margin.Left);
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
        #endregion

        #endregion

        #region OverviewResize  ===============================================

        private void OverviewResize_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!_pointerIsPressed) TrySetNewCursor(CoreCursorType.SizeAll);
        }
        private void OverviewResize_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!_pointerIsPressed) RestorePointerCursor();
        }

        private void OverviewResize_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _pointerIsPressed = true;
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

        #region Refresh  ======================================================
        internal async void Refresh()
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, RefreshAll);

            void RefreshAll()
            {
                CheckColorChange();
                if (FlyTreeCanvas.IsEnabled) FlyTreeCanvas.Refresh();
                if (SideTreeCanvas.IsEnabled) SideTreeCanvas.Refresh();

                if (EditCanvas.IsEnabled) EditCanvas.Invalidate();
                if (OverCanvas.IsEnabled) OverCanvas.Invalidate();
                if (Pick1Canvas.IsEnabled) Pick1Canvas.Invalidate();
                if (Pick2Canvas.IsEnabled) Pick2Canvas.Invalidate();

                if (Model.IsToolTipVisible) ShowToolTip();
                else HideToolTip();
            }
        }
        internal async void PostEvent(DrawEvent evt)
        {
            if (Model.DrawEvent_Action.TryGetValue(evt, out Action action))
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { action(); });
        }
        #endregion

        #region DrawCanvas_Draw  ==============================================
        private void DrawCanvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (_isFirstCall) { _isFirstCall = false; PanZoomReset(); }

            if (sender == EditCanvas) { Draw(Model.HelperData); Draw(Model.EditorData); }
            else if (sender == OverCanvas) Draw(Model.EditorData);
            else if (sender == Pick1Canvas) Draw(Model.Picker1Data);
            else if (sender == Pick2Canvas) Draw(Model.Picker2Data);

            void Draw(IDrawData data)
            {
                if (data is null) return;

                var (scale, offset) = CanvasScaleOffset[sender];
                var ds = args.DrawingSession;

                foreach (var (P, (K, S, W), (A, R, G, B)) in data.Lines)
                {
                    if (K < ShapeType.MultipleSimpleShapesLimit)
                    {
                        var k = K & ShapeType.SimpleShapeMask;
                        var color = Color.FromArgb(A, R, G, B);
                        var stroke = StrokeStyle(S);
                        var isFilled = S == StrokeType.Filled;

                        for (int i = 0; i < P.Length; i += 2)
                        {
                            var c = P[i] * scale + offset;
                            var d = P[i + 1] * scale;
                            DrawShape(c, d, color, stroke, k, isFilled, W);
                        }
                    }
                    else
                    {
                        var V = W * scale;
                        if (V < 1) V = 1;

                        using (var pb = new CanvasPathBuilder(ds))
                        {
                            pb.BeginFigure(P[0] * scale + offset);
                            if (K == ShapeType.JointedLines)
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
                            pb.EndFigure(CanvasFigureLoop.Open);

                            using (var geo = CanvasGeometry.CreatePath(pb))
                            {
                                ds.DrawGeometry(geo, Color.FromArgb(A, R, G, B), V, StrokeStyle(S));
                            }
                        }
                    }
                }

                foreach (var ((C, D), (K, S, W), (A, R, G, B)) in data.Shapes)
                {
                    var d = D * scale;
                    var c = C * scale + offset;

                    DrawShape(c, d, Color.FromArgb(A, R, G, B), StrokeStyle(S), K, (S == StrokeType.Filled), W);
                }

                foreach (var ((P, T), (A, R, G, B)) in data.Text)
                {
                    var p = P * scale + offset;
                    ds.DrawText(T, p, Color.FromArgb(A, R, G, B));
                }

                void DrawShape(Vector2 a, Vector2 b, Color color, CanvasStrokeStyle stroke, ShapeType shape, bool isFilled, byte w)
                {
                    var v = w * scale;
                    if (v < 1) v = 1;
                    switch (shape)
                    {
                        case ShapeType.Line:
                            ds.DrawLine(a, b + offset, color, v, stroke);
                            break;
                        case ShapeType.Circle:
                            if (isFilled)
                                ds.FillCircle(a, b.X, color);
                            else
                                ds.DrawCircle(a, b.X, color, v, stroke);
                            break;
                        case ShapeType.Ellipse:
                            if (isFilled)
                                ds.FillEllipse(a, b.X, b.Y, color);
                            else
                                ds.DrawEllipse(a, b.X, b.Y, color, v, stroke);
                            break;
                        case ShapeType.Rectangle:
                            var e = a - b;
                            var f = 2 * b;
                            if (isFilled)
                                ds.FillRectangle(e.X, e.Y, f.X, f.Y, color);
                            else
                                ds.DrawRectangle(e.X, e.Y, f.X, f.Y, color, v, stroke);
                            break;
                        case ShapeType.RoundedRectangle:
                            e = a - b;
                            f = 2 * b;
                            if (isFilled)
                                ds.FillRoundedRectangle(e.X, e.Y, f.X, f.Y, 8, 8, color);
                            else
                                ds.DrawRoundedRectangle(e.X, e.Y, f.X, f.Y, 8, 8, color, v, stroke);
                            break;
                    }
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

                sender.ContextFlyout.ShowAt(RootCanvas);
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
                SetGridPoint2(e);
                SetDrawPoint2(EditCanvas, Model.EditorData, e);

                e.Handled = true;

                if (_overridePointerMoved is null)
                {
                    if (_pointerIsPressed)
                        PostEvent(DrawEvent.Drag);
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
            if (_pointerIsPressed)
            {
                SetDrawPoint2(Pick1Canvas, Model.Picker1Data, e);
                PostEvent(DrawEvent.Picker1Drag);
            }
        }
        private bool _pointerIsPressed;
        #endregion

        #region PointerPressed  ===============================================
        private void RootCanvas_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_isRootCanvasLoaded)
            {
                _pointerIsPressed = true;
                SetGridPoint1(e);
                SetDrawPoint1(EditCanvas, Model.EditorData, e);
                e.Handled = true;

                if (_overridePointerPressed is null)
                    PostEvent(DrawEvent.Tap);
                else
                    _overridePointerPressed();
            }
        }
        private void Pick1Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _pointerIsPressed = true;
            SetDrawPoint1(Pick1Canvas, Model.Picker1Data, e);
            e.Handled = true;

            if (e.KeyModifiers == Windows.System.VirtualKeyModifiers.Control)
                PostEvent(DrawEvent.Picker1CtrlTap);
            else
                PostEvent(DrawEvent.Picker1Tap);
        }
        private void Pick2Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _pointerIsPressed = true;
            SetDrawPoint1(Pick2Canvas, Model.Picker2Data, e);
            e.Handled = true;

            PostEvent(DrawEvent.Picker2Tap);
        }
        private void OverCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _pointerIsPressed = true;
            e.Handled = true;

            PostEvent(DrawEvent.OverviewTap);
        }
        #endregion

        #region PointerReleased  ==============================================
        private void RootCanvas_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_isRootCanvasLoaded)
            {
                _pointerIsPressed = false;
                SetGridPoint2(e);
                SetDrawPoint2(EditCanvas, Model.EditorData, e);
                e.Handled = true;

                if (_overridePointerReleased is null)
                    PostEvent(DrawEvent.TapEnd);
                else
                    _overridePointerReleased();
            }
        }
        private void Canvas_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _pointerIsPressed = false;
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
        private (float,float) GetToolTipGridPoint()
        {
            var (scale, offset) = CanvasScaleOffset[EditCanvas];
            var p = Model.ToolTipTarget * scale + offset;
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
        private void ColorSampleBorder_PointerPressed(object sender, PointerRoutedEventArgs e) => ToggleColorPicker();
        private void ToggleColorPicker()
        {
            var (A, R, G, B) = Model.ColorARGB;
            if (ColorPickerControl.Visibility == Visibility.Visible)
            {
                ColorPickerControl.Visibility = Visibility.Collapsed;
                ColorSampleTextBox.Text = "\uF0AE";
                ColorSampleBoarder.Background = new SolidColorBrush(Color.FromArgb(A, R, G, B));
                ColorSampleTextBox.Foreground = (G > R + B) ? new SolidColorBrush(Colors.Black) : (R + G + B > 400) ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White); ;
            }
            else
            {
                ColorSampleTextBox.Text = "\uF0AD";
                ColorPickerControl.Visibility = Visibility.Visible;
                ColorPickerControl.Color = Color.FromArgb(A, R, G, B);
            }
        }
        private void CheckColorChange()
        {
            var (A, R, G, B) = Model.ColorARGB;
            var color = Color.FromArgb(A, R, G, B);
            if (color != _prevColor)
            {
                _prevColor = color;
                if (ColorPickerControl.Visibility == Visibility.Visible)
                {
                    if (!_ignoreColorChange)
                        ColorPickerControl.Color = Color.FromArgb(A, R, G, B);
                }
                else
                {
                    ColorSampleBoarder.Background = new SolidColorBrush(Color.FromArgb(A, R, G, B));
                    ColorSampleTextBox.Foreground = (G > R + B) ? new SolidColorBrush(Colors.Black) : (R + G + B > 400) ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White); ;
                }
            }

        }
        private Color _prevColor;
        private bool _ignoreColorChange;
        private void ColorPickerControl_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            _prevColor = args.NewColor;
            _ignoreColorChange = true;

            Model.ColorARGB = (args.NewColor.A, args.NewColor.R, args.NewColor.G, args.NewColor.B);
        }
        #endregion

    }
}
