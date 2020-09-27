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
        }

        internal async void Refresh()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if(FlyTreeCanvas.IsEnabled) FlyTreeCanvas.Refresh();
                if (SideTreeCanvas.IsEnabled) SideTreeCanvas.Refresh();
                if (Model.IsToolTipVisible) ShowToolTip();
                else HideToolTip();
            });


            EditCanvas.Invalidate();
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
            TrySetNewCursor(CoreCursorType.SizeAll);
        }
        private void OverviewResize_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (Model.DrawState != DrawState.ResizeOverview)
                RestorePointerCursor();
        }

        private void OverviewResize_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if ((Model.DrawState & DrawState.ModeMask) == DrawState.ViewMode)
            {
                _pointerIsPressed = true;
                Model.GridPoint1 = new Vector2(0, 0);
                Model.GridPoint2 = GridPoint(e);
                if (Model.TrySetState(DrawState.ResizeOverview))
                {
                    TrySetNewCursor(CoreCursorType.SizeAll);
                    Model.SetEventAction(DrawEvent.Drag, ResizingOverview);
                    Model.SetEventAction(DrawEvent.TapEnd, RestoreState);
                }
            }
        }
        private void RestoreState()
        {
            Model.TrySetState(DrawState.ViewMode);
        }
        private void ResizingOverview()
        {
            var size = Vector2.Abs(Model.GridPoint1 - Model.GridPoint2);
            if (size.X < OverviewBorder.MinWidth) return;
            if (size.Y < OverviewBorder.MinHeight) return;

            OverviewBorder.Width = size.X;
            OverviewBorder.Height = size.Y;
            SetOverviewScaleOffset();
            OverCanvas.Invalidate();
        }
        #endregion

        #region PanZoom  ======================================================
        private float _editScale = 0.5f; //scale the view extent so that it fits on the canvas
        private Vector2 _editOffset = new Vector2(); //offset need to center the view extent on the canvas
        private float _overScale = 0.5f; //scale the view extent so that it fits on the canvas
        private Vector2 _overOffset = new Vector2(); //offset need to center the view extent on the canvas
        private float _pick1Scale = 0.5f; //scale the view extent so that it fits on the canvas
        private Vector2 _pick1Offset = new Vector2(); //offset need to center the view extent on the canvas
        private float _pick2Scale = 0.5f; //scale the view extent so that it fits on the canvas
        private Vector2 _pick2Offset = new Vector2(); //offset need to center the view extent on the canvas
        (float,Vector2) GetScaleOffset(CanvasControl sender)
        {
            if (sender == EditCanvas) return (_editScale, _editOffset);
            if (sender == OverCanvas) return (_overScale, _overOffset);
            if (sender == Pick1Canvas) return (_pick1Scale, _pick1Offset);
            if (sender == Pick2Canvas) return (_pick2Scale, _pick2Offset);
            return (1.0f, new Vector2(0, 0));
        }

        private const float maxScale = 10;
        private const float minZoomDiagonal = 8000;

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
            var aw = (float)EditCanvas.ActualWidth;
            var ah = (float)EditCanvas.ActualHeight;

            var e = Model.EditorExtent;
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
            _editScale = z;

            var ec = new Vector2(e.CenterX, e.CenterY) * z; //center point of scaled view extent
            var ac = new Vector2(aw / 2, ah / 2); //center point of the canvas
            _editOffset = ac - ec; //complete offset need to center the view extent on the canvas

            SetOverviewScaleOffset();
            SetPicker1ScaleOffset();
            SetPicker2ScaleOffset();
            EditCanvas.Invalidate();
        }
        private void SetOverviewScaleOffset()
        {
            var aw = (float)OverCanvas.ActualWidth;
            var ah = (float)OverCanvas.ActualHeight;

            var e = Model.EditorExtent;
            var ew = (float)e.Width;
            var eh = (float)e.Hieght;

            if (aw < 1) aw = 1;
            if (ah < 1) ah = 1;
            if (ew < 1) ew = 1;
            if (eh < 1) eh = 1;

            var zw = aw / ew;
            var zh = ah / eh;
            var z = (zw < zh) ? zw : zh;

            _overScale = z;

            var ec = new Vector2(e.CenterX, e.CenterY) * z; //center point of scaled view extent
            var ac = new Vector2(aw / 2, ah / 2); //center point of the canvas
            _overOffset = ac - ec; //complete offset need to center the view extent on the canvas
        }
        private void SetPicker1ScaleOffset()
        {
            var aw = (float)Pick1Canvas.ActualWidth;
            var ew = (float)Model.Picker1Width;

            if (aw < 1) aw = 1;
            if (ew < 1) ew = 1;

            _pick1Scale = aw / ew;
            _pick1Offset = new Vector2(aw / 2, aw / 2); //center point of the canvas
        }
        private void SetPicker2ScaleOffset()
        {
            var aw = (float)Pick2Canvas.ActualWidth;
            var ew = (float)Model.Picker2Width;

            if (aw < 1) aw = 1;
            if (ew < 1) ew = 1;

            _pick2Scale = aw / ew;
            _pick2Offset = new Vector2(aw / 2, aw / 2); //center point of the canvas
        }
        #endregion

        #region Picker_PointerPressed  ========================================
        private async void Pick1Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(Pick1Canvas).Position;
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Model.Picker1Select((int)p.Y); });
            EditCanvas.Invalidate();
        }
        private async void Pick2Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(Pick2Canvas).Position;
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Model.Picker2Select((int)p.Y); });
            EditCanvas.Invalidate();
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
                var ds = s & Core.StrokeType.DashMask;

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

        #region DrawCanvas_Draw  ==============================================
        private void DrawCanvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (sender == EditCanvas)
            {
                DoDraw(Model.HelperData);
                DoDraw(Model.EditorData);

                if (OverCanvas.IsEnabled) OverCanvas.Invalidate();
                else if (Pick1Canvas.IsEnabled) Pick1Canvas.Invalidate();
                else if (Pick2Canvas.IsEnabled) Pick2Canvas.Invalidate();
            }
            else if (sender == OverCanvas)
            {
                DoDraw(Model.EditorData);
                if (Pick1Canvas.IsEnabled) Pick1Canvas.Invalidate();
                else if (Pick2Canvas.IsEnabled) Pick2Canvas.Invalidate();
            }
            else if (sender == Pick1Canvas)
            {
                DoDraw(Model.Picker1Data);
                if (Pick2Canvas.IsEnabled) Pick2Canvas.Invalidate();
            }
            else if (sender == Pick2Canvas)
            {
                DoDraw(Model.Picker2Data);
            }

            void DoDraw(IDrawData data)
            {
                if (data is null) return;

                var (scale, offset) = GetScaleOffset(sender);
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
                                ds.FillRoundedRectangle(e.X, e.Y, f.X, f.Y, 6, 6, color);
                            else
                                ds.DrawRoundedRectangle(e.X, e.Y, f.X, f.Y, 6, 6, color, v, stroke);
                            break;
                    }
                }
            }


            // refresh both picker1 & picker2 canvases
            if (sender == EditCanvas)
            {
                Pick1Canvas.Invalidate();
                OverCanvas.Invalidate();
            }
            else if (sender == Pick1Canvas)
                Pick2Canvas.Invalidate();
        }
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
        private void RootCanvas_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            e.Handled = true;
            if ((Model.DrawState & DrawState.NowMask) == DrawState.NowOnVoid) PanZoomReset();
        }
        #endregion

        #region RootCanvas_PointerMoved  ======================================
        private void RootCanvas_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_isRootCanvasLoaded)
            {
                SetGridPoint2(e);
                SetDrawPoint2(e);

                e.Handled = true;

                if (_pointerIsPressed)
                    PostEvent(DrawEvent.Drag);
                else
                    PostEvent(DrawEvent.Skim);
            }
        }
        private bool _pointerIsPressed;
        #endregion

        #region RootCanvas_PointerPressed  ====================================
        private void RootCanvas_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_isRootCanvasLoaded)
            {
                _pointerIsPressed = true;
                SetGridPoint1(e);
                SetDrawPoint1(e);
                e.Handled = true;

                PostEvent(DrawEvent.Tap);
            }
        }
        #endregion

        #region RootCanvas_PointerReleased  ===================================
        private void RootCanvas_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_isRootCanvasLoaded)
            {
                _pointerIsPressed = false;
                SetGridPoint2(e);
                SetDrawPoint2(e);
                e.Handled = true;

                PostEvent(DrawEvent.TapEnd);
            }
        }
        #endregion

        #region HelperMethods  ================================================
        private void SetGridPoint1(PointerRoutedEventArgs e) => Model.GridPoint1 = GridPoint(e);
        private void SetGridPoint2(PointerRoutedEventArgs e) => Model.GridPoint2 = GridPoint(e);
        private void SetDrawPoint1(PointerRoutedEventArgs e) => Model.DrawPoint1 = DrawPoint(e);
        private void SetDrawPoint2(PointerRoutedEventArgs e) => Model.DrawPoint2 = DrawPoint(e);
        private Vector2 GridPoint(PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(RootGrid).Position;
            return new Vector2((float)p.X, (float)p.Y);
        }
        private Vector2 DrawPoint(PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(EditCanvas).Position;
            var x = (p.X - _editOffset.X) / _editScale;
            var y = (p.Y - _editOffset.Y) / _editScale;
            return new Vector2((float)x, (float)y);
        }
        private (float top, float left, float width, float height) GetResizerParams()
        {
            var (x1, y1, x2, y2) = Model.ResizerExtent.GetFloat();

            var dx = x2 - x1;
            var dy = y2 - y1;

            var width = dx * _editScale;
            var height = dy * _editScale;

            var top = y1 * _editScale + _editOffset.X;
            var left = x1 * _editScale + _editOffset.Y;

            return (top, left, width, height);
        }
        private (float,float) GetToolTipGridPoint()
        {
            var p = Model.ToolTipTarget * _editScale + _editOffset;
            return (p.X, p.Y);
        }
        #endregion

        #region PostEvent  ====================================================
        internal async void PostEvent(DrawEvent evt)
        {
            if (Model.DrawEvent_Action.TryGetValue(evt, out Action action))
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { action(); });
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
                ColorSampleBoarder.Background = new SolidColorBrush(Color.FromArgb(A, R, G, B));
                ColorSampleTextBox.Foreground = (G > R + B) ? new SolidColorBrush(Colors.Black) : (R + G + B > 400) ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White); ;
                ColorSampleTextBox.Text = "\uF0AE";
                ColorPickerControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                ColorPickerControl.Visibility = Visibility.Visible;
                ColorSampleTextBox.Text = "\uF0AD";
                ColorPickerControl.Color = Color.FromArgb(A, R, G, B);
            }
        }
        private void ColorPickerControl_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            if (args.NewColor != args.OldColor)
            {
                Model.ColorARGB = (args.NewColor.A, args.NewColor.R, args.NewColor.G, args.NewColor.B);
                Model.ColorARGBChanged();
            }
        }
        #endregion

    }
}
