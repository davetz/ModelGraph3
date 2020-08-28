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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ModelGraph.Controls
{
    public sealed partial class DrawCanvasControl : UserControl
    {
        private ICanvasModel _model;
        private CoreDispatcher _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

        private float _scale = 0.5f; //scale the view extent so that it fits on the canvas
        private Vector2 _offset = new Vector2(); //offset need to center the view extent on the canvas

        #region Constructor/Initialize  =======================================
        public DrawCanvasControl()
        {
            this.InitializeComponent();
        }

        public void Initialize(ICanvasModel model)
        {
            _model = model;
            EditCanvas.DataContext = _model.EditorData;
            Pick1Canvas.DataContext = _model.Picker1Data;
            Pick2Canvas.DataContext = _model.Picker2Data;
            ShowPicker1();
            ShowPicker2();
        }

        public void Refresh() => EditCanvas.Invalidate();
        #endregion


        #region PanZoom  ======================================================
        private const float maxScale = 4;
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

            var e = _model.EditorExtent;
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
            _scale = z;

            var ec = new Vector2(e.CenterX, e.CenterY) * z; //center point of scaled view extent
            var ac = new Vector2(aw / 2, ah / 2); //center point of the canvas
            _offset = ac - ec; //complete offset need to center the view extent on the canvas

            EditCanvas.Invalidate();
        }
        #endregion

        #region Picker_PointerPressed  ========================================
        private async void Pick2Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(Pick2Canvas).Position;
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { _model.Picker2Select((int)p.Y); });
            Pick2Canvas.Invalidate();
        }
        private void ShowPicker1()
        {
            Pick2Canvas.Width = _model.Picker2Width;
            if (_model.Picker1Width < 4)
                HidePicker1();
            else
            {
                Picker1Grid.Visibility = Visibility.Visible;
                Picker1GridColumn.Width = new GridLength(_model.Picker2Width + 4);
            }
        }
        private void HidePicker1()
        {
                Picker1Grid.Visibility = Visibility.Collapsed;
                Picker1GridColumn.Width = new GridLength(0);
        }
        private void ShowPicker2()
        {
            Pick2Canvas.Width = _model.Picker2Width;
            if (_model.Picker2Width < 4)
                HidePicker2();
            else
            {
                Picker2Grid.Visibility = Visibility.Visible;
                Picker2GridColumn.Width = new GridLength(_model.Picker2Width + 8);
            }
        }
        private void HidePicker2()
        {
            Picker2Grid.Visibility = Visibility.Collapsed;
            Picker2GridColumn.Width = new GridLength(0);
        }
        #endregion


        #region StrokeStyle  ==================================================
        private CanvasStrokeStyle StrokeStyle(Stroke s)
        {
            var ss = _strokeStyle;
            ss.DashStyle = s == Stroke.IsDotted ? CanvasDashStyle.Dot : CanvasDashStyle.Solid;
            ss.StartCap = CanvasCapStyle.Flat;
            ss.EndCap = CanvasCapStyle.Flat;
            ss.DashCap = CanvasCapStyle.Round;
            ss.LineJoin = CanvasLineJoin.Round;
            return ss;
        }
        private CanvasStrokeStyle _strokeStyle = new CanvasStrokeStyle();
        #endregion

        #region DrawCanvas_Draw  ==============================================
        private void DrawCanvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var data = sender.DataContext as IDrawData;
            if (data != null)
            {
                var scale = 1.0f;
                var offset = new Vector2();
                if (sender == EditCanvas)
                {
                    scale = _scale;
                    offset = _offset;
                }
                var ds = args.DrawingSession;

                foreach (var (P, (S, W), (A, R, G, B)) in data.Lines)
                {
                    using (var pb = new CanvasPathBuilder(ds))
                    {
                        pb.BeginFigure(P[0] * scale + offset);
                        for (int i = 1; i < P.Length; i++)
                        {
                            pb.AddLine(P[i] * scale + offset);
                        }
                        pb.EndFigure(CanvasFigureLoop.Open);

                        using (var geo = CanvasGeometry.CreatePath(pb))
                        {
                            ds.DrawGeometry(geo, Color.FromArgb(A, R, G, B), W, StrokeStyle(S));
                        }
                    }
                }

                foreach (var (P, (S, W), (A, R, G, B)) in data.Splines)
                {
                    using (var pb = new CanvasPathBuilder(ds))
                    {
                        pb.BeginFigure(P[0] * scale + offset);
                        var N = P.Length;
                        for (var i = 0; i < N - 2;)
                        {
                            pb.AddCubicBezier(P[i] * scale + offset, P[++i] * scale + offset, P[++i] * scale + offset);
                        }
                        pb.EndFigure(CanvasFigureLoop.Open);

                        using (var geo = CanvasGeometry.CreatePath(pb))
                        {
                            ds.DrawGeometry(geo, Color.FromArgb(A, R, G, B), W, StrokeStyle(S));
                        }
                    }
                }

                foreach (var ((C, D), (S, W), (A, R, G, B)) in data.Rects)
                {
                    var d = D * scale;
                    var c = (C * scale + offset) - d;
                    var r = 2 * d;

                    if (S == Stroke.IsFilled)
                        ds.FillRoundedRectangle(c.X, c.Y, r.X, r.Y, 6, 6, Color.FromArgb(A, R, G, B));
                    else
                        ds.DrawRoundedRectangle(c.X, c.Y, r.X, r.Y, 6, 6, Color.FromArgb(A, R, G, B), W, StrokeStyle(S));
                }

                foreach (var ((C, D), (S, W), (A, R, G, B)) in data.Circles)
                {
                    var r = D * scale;
                    var c = C * scale + offset;

                    if (S == Stroke.IsFilled)
                        ds.FillCircle(c.X, c.Y, r, Color.FromArgb(A, R, G, B));
                    else
                        ds.DrawCircle(c.X, c.Y, r, Color.FromArgb(A, R, G, B), W, StrokeStyle(S));
                }

                foreach (var ((P, T), (A, R, G, B)) in data.Text)
                {
                    var p = P * scale + offset;
                    ds.DrawText(T, p, Color.FromArgb(A, R, G, B));
                }
            }

            // refresh both picker1 & picker2 canvases
            if (sender == EditCanvas)
                Pick1Canvas.Invalidate();
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
                SetViewIdle();
                PanZoomReset();
            }
        }
        bool _isDrawCanvasLoaded;

        private void RootCanvas_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _isRootCanvasLoaded = true;
            RootCanvas.Loaded -= RootCanvas_Loaded;
            SetViewIdle();
            if (_isDrawCanvasLoaded) PanZoomReset();
        }
        bool _isRootCanvasLoaded;
        #endregion


        private void RootCanasContextRuested(Windows.UI.Xaml.UIElement sender, ContextRequestedEventArgs args)
        {
            if (args.TryGetPosition(RootCanvas, out Point p))
            {
                args.Handled = true;

                sender.ContextFlyout.ShowAt(RootCanvas);
            }
        }

        #region RootCanvas_DoubleTapped  ======================================
        private void RootCanvas_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            e.Handled = true;
            PostEvent(EventType.DoubleTap);
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
                    PostEvent(EventType.Drag);
                else
                    PostEvent(EventType.Skim);
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

                PostEvent(EventType.Tap);
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

                PostEvent(EventType.TapEnd);
            }
        }
        #endregion

        #region HelperMethods  ================================================
        private void SetGridPoint1(PointerRoutedEventArgs e) => _model.GridPoint1 = GridPoint(e);
        private void SetGridPoint2(PointerRoutedEventArgs e) => _model.GridPoint2 = GridPoint(e);
        private void SetDrawPoint1(PointerRoutedEventArgs e) => _model.DrawPoint1 = DrawPoint(e);
        private void SetDrawPoint2(PointerRoutedEventArgs e) => _model.DrawPoint2 = DrawPoint(e);
        private Vector2 GridPoint(PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(RootGrid).Position;
            return new Vector2((float)p.X, (float)p.Y);
        }
        private Vector2 DrawPoint(PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(EditCanvas).Position;
            var x = (p.X - _offset.X) / _scale;
            var y = (p.Y - _offset.Y) / _scale;
            return new Vector2((float)x, (float)y);
        }
        private (float top, float left, float width, float height) GetResizerParams()
        {
            var x1 = _model.NodePoint1.X;
            var y1 = _model.NodePoint1.Y;
            var x2 = _model.NodePoint2.X;
            var y2 = _model.NodePoint2.Y;


            var dx = x2 - x1;
            var dy = y2 - y1;

            var width = dx * _scale;
            var height = dy * _scale;

            var top = y1 * _scale + _offset.X;
            var left = x1 * _scale + _offset.Y;

            return (top, left, width, height);
        }
        #endregion


        #region Event/Mode/State/Action  ======================================
        internal enum EventType { Idle, Tap, DoubleTap, TapEnd, ContextMenu, Skim, Drag, TopHit, LeftHit, RightHit, BottomHit, TopLeftHit, TopRightHit, BottomLeftHit, BottomRightHit };
        internal enum StateType
        {
            Unknown,

            ViewIdle,
            ViewOnVoidTap, ViewOnVoidDrag,   //trace a new region

            ViewOnPinSkim, ViewOnNodeSkim, ViewOnRegionSkim,  //show tooltips
            ViewOnPinTap, ViewOnNodeTap, ViewOnRegionTap,     //show property sheet

            ResizeTop, ResizeLeft, ResizeRight, ResizeBottom,
            ResizeTopLeft, ResizeTopRight, ResizeBottomLeft, ResizeBottomRight,

            MoveIdle,
            MoveOnNodeSkim, MoveOnRegionSkim,
            MovenNodeTap, MoveOnRegionTap,
            MovenNodeDrag, MoveOnRegionDrag,

            CopyIdle,
            CopyOnNodeSkim, CopyOnRegionSkim,
            CopyOnNodeTap, CopyOnRegionTap,
            CopyOnNodeDrag, CopyOnRegionDrag,

            LinkIdle,
            LinkOnPinSkim, LinkOnNodeSkim,
            LinkOnPinTap, LinkOnNodeTap,
            LinkOnPinDrag, LinkOnNodeDrag,

            CreateIdle, CreateTap, CreateOnNode,
        };

        internal void PostEvent(EventType evt) { if (Event_Action.TryGetValue(evt, out Action action)) action(); }

        internal bool TrySetState(StateType state)
        {
            if (_state == state) return false; // we have not changed the state, so there is nothing to do
            _state = state;
            Debug.WriteLine($"State: {_state}");

            if (state == StateType.CreateIdle)
                ShowPicker2();
            else
                HidePicker2();

            Event_Action.Clear();
            HideTootlip();
            HideResizerGrid();
            HideSelectorGrid();
            RestorePointerCursor();

            return true;    // let the caller know that we have changed state and have cleared the Event_Action dictionary
        }
        private StateType _state = StateType.Unknown;

        internal void SetEventAction(EventType evt, Action act)
        {
            Event_Action[evt] = act;
        }
        private readonly Dictionary<EventType, Action> Event_Action = new Dictionary<EventType, Action>();
        #endregion


        #region View_Mode  ====================================================

        #region SetViewIdle  ==================================================
        void SetViewIdle()
        {
            if (_isRootCanvasLoaded)
            {
                if (TrySetState(StateType.ViewIdle))
                {
                    SetEventAction(EventType.Tap, ViewIdle_TapHitTest);
                    SetEventAction(EventType.Skim, ViewIdle_SkimHitTest);
                    SetEventAction(EventType.TopHit, SetResizeTopHit);
                    SetEventAction(EventType.LeftHit, SetResizeLeftHit);
                    SetEventAction(EventType.RightHit, SetResizeRightHit);
                    SetEventAction(EventType.BottomHit, SetResizeBottomHit);
                    SetEventAction(EventType.TopLeftHit, SetResizeTopLeftHit);
                    SetEventAction(EventType.TopRightHit, SetResizeTopRightHit);
                    SetEventAction(EventType.BottomLeftHit, SetResizeBottomLeftHit);
                    SetEventAction(EventType.BottomRightHit, SetResizeBottomRightHit);
                }
            }
        }
       
        async void ViewIdle_SkimHitTest()
        {
            var anyHit = false;
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { anyHit = _model.SkimHitTest(); });
            if (anyHit)
            {
                if (_model.NodeHit)
                    ShowTooltip();
            }
            else
            {
                HideTootlip();
            }
        }
        async void ViewIdle_TapHitTest()
        {
            var anyHit = false;
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { anyHit = _model.TapHitTest(); });
            if (anyHit)
            {
                if (_model.NodeHit)
                {
                    //_model.ShowPropertyPanel();
                    ShowResizerGrid();
                }
            }
            else
            {
                HideResizerGrid();
                HideAlignmentGrid();
                HideTootlip();
                SetViewOnVoidTap();
                //_model.HidePropertyPanel();
            }
        }

        void SetViewOnVoidTap()
        {
            if (TrySetState(StateType.ViewOnVoidTap))
            {
                SetEventAction(EventType.TapEnd, SetViewIdle);
                SetEventAction(EventType.Drag, SetViewOnVoidDrag);
            }
        }
        void SetViewOnVoidDrag()
        {
            if (TrySetState(StateType.ViewOnVoidDrag))
            {
                ShowSelectorGrid();
                SetEventAction(EventType.TapEnd, RegionTraceEnd);
                SetEventAction(EventType.Drag, TracingRegion);
            }
        }
        void RegionTraceEnd()
        {
            ShowAlignmentGrid();
            if (!_model.IsValidRegion())
                HideAlignmentGrid();
            SetViewIdle();
        }
        void TracingRegion()
        {
            UpdateSelectorGrid();
        }
        #endregion

        #region SetResize  ====================================================
        void SetResizeTopHit()
        {
            if (TrySetState(StateType.ResizeTop))
            {
                SetEventAction(EventType.Drag, ResizeTopDrag);
                SetEventAction(EventType.TapEnd, ResizeEnd);
            }
        }
        void SetResizeLeftHit()
        {
            if (TrySetState(StateType.ResizeLeft))
            {
                SetEventAction(EventType.Drag, ResizeLeftDrag);
                SetEventAction(EventType.TapEnd, ResizeEnd);
            }
        }
        void SetResizeRightHit()
        {
            if (TrySetState(StateType.ResizeRight))
            {
                SetEventAction(EventType.Drag, ResizeRightDrag);
                SetEventAction(EventType.TapEnd, ResizeEnd);
            }
        }
        void SetResizeBottomHit()
        {
            if (TrySetState(StateType.ResizeBottom))
            {
                SetEventAction(EventType.Drag, ResizeBottomDrag);
                SetEventAction(EventType.TapEnd, ResizeEnd);
            }
        }
        void SetResizeTopLeftHit()
        {
            if (TrySetState(StateType.ResizeTopLeft))
            {
                SetEventAction(EventType.Drag, ResizeTopLeftDrag);
                SetEventAction(EventType.TapEnd, ResizeEnd);
            }
        }
        void SetResizeTopRightHit()
        {
            if (TrySetState(StateType.ResizeTopRight))
            {
                SetEventAction(EventType.Drag, ResizeTopRightDrag);
                SetEventAction(EventType.TapEnd, ResizeEnd);
            }
        }
        void SetResizeBottomLeftHit()
        {
            if (TrySetState(StateType.ResizeBottomLeft))
            {
                SetEventAction(EventType.Drag, ResizeBottomLeftDrag);
                SetEventAction(EventType.TapEnd, ResizeEnd);
            }
        }
        void SetResizeBottomRightHit()
        {
            if (TrySetState(StateType.ResizeBottomRight))
            {
                SetEventAction(EventType.Drag, ResizeBottomRightDrag);
                SetEventAction(EventType.TapEnd, ResizeEnd);
            }
        }
        void ResizeEnd()
        {
            _model.ResizePropagate();
            _model.RefreshDrawData();
            EditCanvas.Invalidate();
            RestorePointerCursor();
            SetViewIdle();
        }
        void ResizeTopDrag()
        {
            _model.ResizeTop();
            UpdateResizerGrid();
        }
        void ResizeLeftDrag()
        {
            _model.ResizeLeft();
            UpdateResizerGrid();
        }
        void ResizeRightDrag()
        {
            _model.ResizeRight();
            UpdateResizerGrid();
        }
        void ResizeBottomDrag()
        {
            _model.ResizeBottom();
            UpdateResizerGrid();
        }
        void ResizeTopLeftDrag()
        {
            _model.ResizeTopLeft();
            UpdateResizerGrid();
        }
        void ResizeTopRightDrag()
        {
            _model.ResizeTopRight();
            UpdateResizerGrid();
        }
        void ResizeBottomLeftDrag()
        {
            _model.ResizeBottomLeft();
            UpdateResizerGrid();
        }
        void ResizeBottomRightDrag()
        {
            _model.ResizeBottomRight();
            UpdateResizerGrid();
        }
        #endregion

        #region SetViewOnNodeSkim  ============================================
        void SetView_OnNode_Skim()
        {
            if (TrySetState(StateType.ViewOnNodeSkim))
            {
                SetEventAction(EventType.Skim, View_OnNode_SkimHitTest);
            }
        }
        async void View_OnNode_SkimHitTest()
        {
            var anyHit = false;
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { anyHit = _model.SkimHitTest(); });
            if (anyHit && _model.NodeHit)
            {

            }
        }
        #endregion

        #endregion

        #region Move_Mode  ====================================================
        void SetMoveIdle()
        {
            if (TrySetState(StateType.MoveIdle))
            {
                SetEventAction(EventType.Skim, MoveIdle_SkimHitTest);
                SetEventAction(EventType.Tap, MoveIdle_TapHitTest);
                SetEventAction(EventType.TapEnd, MoveIdle_End);
            }
        }
        void MoveIdle_End()
        {
            RestorePointerCursor();
        }
        async void MoveIdle_SkimHitTest()
        {
            var anyHit = false;
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { anyHit = _model.SkimHitTest(); });
            if (anyHit)
            {
                if (_model.RegionHit || _model.NodeHit)
                {
                    TrySetNewCursor(CoreCursorType.Hand);
                }
            }
            else
            {
                RestorePointerCursor();
            }
        }
        async void MoveIdle_TapHitTest()
        {
            var anyHit = false;
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { anyHit = _model.TapHitTest(); });
            if (anyHit)
            {
                if (_model.RegionHit || _model.NodeHit)
                {
                    TrySetNewCursor(CoreCursorType.SizeAll);
                    if (_model.RegionHit)
                        SetMoveRegionDrag();
                    else if (_model.NodeHit)
                        SetMoveNodeDrag();
                }
                else
                {
                    RestorePointerCursor();
                }
            }
        }
        void SetMoveNodeDrag()
        {
            if (TrySetState(StateType.MoveOnRegionDrag))
            {
                SetEventAction(EventType.Drag, MovingNode);
                SetEventAction(EventType.TapEnd, SetMoveIdle);
            }
        }
        async void MovingNode()
        {
            var ok = false;
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { ok = _model.MoveNode(); });
            if (ok)
            {
                _model.RefreshDrawData();
                EditCanvas.Invalidate();
            }
        }
        void SetMoveRegionDrag()
        {
            if (TrySetState(StateType.MoveOnRegionDrag))
            {
                SetEventAction(EventType.Drag, MovingRegion);
                SetEventAction(EventType.TapEnd, SetMoveIdle);
            }
        }
        async void MovingRegion()
        {
            var ok = false;
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { ok = _model.MoveRegion(); });
            if (ok)
            {
                _model.RefreshDrawData();
                EditCanvas.Invalidate();
            }
        }
        #endregion

        #region Link_Mode  ====================================================

        #endregion

        #region Create_Mode  ==================================================
        void SetCreateIdle()
        {
            if (TrySetState(StateType.CreateIdle))
            {
                Picker2GridColumn.Width = new GridLength(_model.Picker2Width);
                SetEventAction(EventType.Tap, CreateNewNode);
            }
        }
        async void CreateNewNode()
        {
            var ok = false;
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { ok = _model.CreateNode(); });
            EditCanvas.Invalidate();
            ViewSelect.IsChecked = true;
        }
        #endregion


        #region RadioButton_Events  ===========================================
        private void ViewSelect_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e) => SetViewIdle();
        private void MoveSelect_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e) => SetMoveIdle();
        private void LinkSelect_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e) { }
        private void CopySelect_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e) { }
        private void CreateSelect_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e) => SetCreateIdle();
        private void OperateSelect_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e) { }
        #endregion

        #region ModelCanvas_Unloaded  =========================================
        private void ModelCanvas_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Unloaded -= ModelCanvas_Unloaded;

            if (EditCanvas != null)
            {
                EditCanvas.RemoveFromVisualTree();
                EditCanvas = null;
            }
        }
        #endregion
    }
}
