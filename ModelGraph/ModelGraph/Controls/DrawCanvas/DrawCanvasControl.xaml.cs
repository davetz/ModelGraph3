﻿using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using ModelGraph.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ModelGraph.Controls
{
    public sealed partial class DrawCanvasControl : UserControl
    {
        private IDrawCanvas _selector;
        private CoreDispatcher _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

        private float _scale = 0.5f; //scale the view extent so that it fits on the canvas
        private Vector2 _offset = new Vector2(); //offset need to center the view extent on the canvas

        #region Constructor/Initialize  =======================================
        public DrawCanvasControl()
        {
            this.InitializeComponent();
        }

        public void Initialize(IDrawCanvas selector)
        {
            _selector = selector;
        }

        public void Refresh() => DrawCanvas.Invalidate();
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
            var aw = (float)DrawCanvas.ActualWidth;
            var ah = (float)DrawCanvas.ActualHeight;

            var e = _selector.DrawingExtent;
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

            DrawCanvas.Invalidate();
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
            var data = _selector;
            if (data is null) return;

            var ds = args.DrawingSession;


            foreach (var (P, (S, W), (A, R, G, B)) in data.DrawLines)
            {
                using (var pb = new CanvasPathBuilder(ds))
                {
                    pb.BeginFigure(P[0] * _scale + _offset);
                    for (int i = 1; i < P.Length; i++)
                    {
                        pb.AddLine(P[i] * _scale + _offset);
                    }
                    pb.EndFigure(CanvasFigureLoop.Open);

                    using (var geo = CanvasGeometry.CreatePath(pb))
                    {
                        ds.DrawGeometry(geo, Color.FromArgb(A, R, G, B), W, StrokeStyle(S));
                    }
                }
            }

            foreach (var (P, (S, W), (A, R, G, B)) in data.DrawSplines)
            {
                using (var pb = new CanvasPathBuilder(ds))
                {
                    pb.BeginFigure(P[0] * _scale + _offset);
                    var N = P.Length;
                    for (var i = 0; i < N - 2;)
                    {
                        pb.AddCubicBezier(P[i] * _scale + _offset, P[++i] * _scale + _offset, P[++i] * _scale + _offset);
                    }
                    pb.EndFigure(CanvasFigureLoop.Open);

                    using (var geo = CanvasGeometry.CreatePath(pb))
                    {
                        ds.DrawGeometry(geo, Color.FromArgb(A, R, G, B), W, StrokeStyle(S));
                    }
                }
            }

            foreach (var ((C, D), (S, W), (A, R, G, B)) in data.DrawRects)
            {
                var d = D * _scale;
                var c = (C * _scale + _offset) + d;
                var r = 2 * d;

                if (S == Stroke.IsFilled)
                    ds.FillRoundedRectangle(c.X, c.Y, r.X, r.Y , 5, 5, Color.FromArgb(A, R, G, B));
                else
                    ds.DrawRoundedRectangle(c.X, c.Y, r.X, r.Y, 5, 5, Color.FromArgb(A, R, G, B), W, StrokeStyle(S));
            }

            foreach (var((C, D), (S, W), (A, R, G, B)) in data.DrawCircles)
            {
                var r = D * _scale;
                var c = C * _scale + _offset;

                if (S == Stroke.IsFilled)
                    ds.FillCircle(c.X,c.Y, r, Color.FromArgb(A, R, G, B));
                else
                    ds.DrawCircle(c.X, c.Y, r, Color.FromArgb(A, R, G, B), W, StrokeStyle(S));
            }

            foreach (var ((P,T), (A, R, G, B)) in data.DrawText)
            {
                var p = P * _scale + _offset;
                ds.DrawText(T, p, Color.FromArgb(A, R, G, B));
            }
        }
        #endregion

        #region Canavas_Loaded  ===============================================
        private void DrawCanvas_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _isDrawCanvasLoaded = true;
            DrawCanvas.Loaded -= DrawCanvas_Loaded;
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


        #region RootCanvas_PointerMoved  ======================================
        private void RootCanvas_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_isRootCanvasLoaded)
            {
                SetGridPoint2(e);
                SetDrawPoint2(e);

                e.Handled = true;

                if (_pointerIsPressed)
                    Post(EventType.Drag);
                else
                    Post(EventType.Skim);
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

                Post(EventType.Tap);
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

                Post(EventType.End);
            }
        }
        #endregion

        #region HelperMethods  ================================================
        private void SetGridPoint1(PointerRoutedEventArgs e) => _selector.GridPoint1 = GridPoint(e);
        private void SetGridPoint2(PointerRoutedEventArgs e) => _selector.GridPoint2 = GridPoint(e);
        private void SetDrawPoint1(PointerRoutedEventArgs e) => _selector.DrawPoint1 = DrawPoint(e);
        private void SetDrawPoint2(PointerRoutedEventArgs e) => _selector.DrawPoint2 = DrawPoint(e);
        private Vector2 GridPoint(PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(RootGrid).Position;
            return new Vector2((float)p.X, (float)p.Y);
        }
        private Vector2 DrawPoint(PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(DrawCanvas).Position;
            var x = (p.X - _offset.X) / _scale;
            var y = (p.Y - _offset.Y) / _scale;
            return new Vector2((float)x, (float)y);
        }
        private (float top, float left, float width, float height) GetResizerParams()
        {
            var x1 = _selector.NodePoint1.X;
            var y1 = _selector.NodePoint1.Y;
            var x2 = _selector.NodePoint2.X;
            var y2 = _selector.NodePoint2.Y;


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
        enum EventType { Idle, Tap, End, Skim, Drag, TopHit, LeftHit, RightHit, BottomHit, TopLeftHit, TopRightHit, BottomLeftHit, BottomRightHit };
        private enum StateType
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

        StateType _state = StateType.Unknown;
        Dictionary<EventType, Action> Event_Action = new Dictionary<EventType, Action>();

        public (int Width, int Height) PreferredSize => throw new NotImplementedException();

        public IDataModel DataModel => throw new NotImplementedException();

        void Post(EventType evt) { if (Event_Action.TryGetValue(evt, out Action action)) action(); }

        bool SetState(StateType state)
        {
            if (_state == state) return false;
            _state = state;
            Debug.WriteLine($"State: {_state}");

            Event_Action.Clear();
            return true;
        }
        void SetEventAction(EventType evt, Action act)
        {
            Event_Action[evt] = act;
        }
        #endregion


        #region View_Mode  ====================================================

        #region SetViewIdle  ==================================================
        void SetViewIdle()
        {
            if (_isRootCanvasLoaded)
            {
                if (SetState(StateType.ViewIdle))
                {
                    HideSelectorGrid();
                    RestorePointerCursor();
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
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { anyHit = _selector.SkimHitTest(); });
            if (anyHit)
            {
                if (_selector.IsHitNode)
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
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { anyHit = _selector.TapHitTest(); });
            if (anyHit)
            {
                if (_selector.IsHitNode)
                {
                    //_selector.ShowPropertyPanel();
                    ShowResizerGrid();
                }
            }
            else
            {
                HideResizerGrid();
                HideAlignmentGrid();
                HideTootlip();
                SetViewOnVoidTap();
                //_selector.HidePropertyPanel();
            }
        }

        void SetViewOnVoidTap()
        {
            if (SetState(StateType.ViewOnVoidTap))
            {
                SetEventAction(EventType.End, SetViewIdle);
                SetEventAction(EventType.Drag, SetViewOnVoidDrag);
            }
        }
        void SetViewOnVoidDrag()
        {
            if (SetState(StateType.ViewOnVoidDrag))
            {
                ShowSelectorGrid();
                SetEventAction(EventType.End, RegionTraceEnd);
                SetEventAction(EventType.Drag, TracingRegion);
            }
        }
        void RegionTraceEnd()
        {
            ShowAlignmentGrid();
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
            if (SetState(StateType.ResizeTop))
            {
                SetEventAction(EventType.Drag, ResizeTopDrag);
                SetEventAction(EventType.End, ResizeEnd);
            }
        }
        void SetResizeLeftHit()
        {
            if (SetState(StateType.ResizeLeft))
            {
                SetEventAction(EventType.Drag, ResizeLeftDrag);
                SetEventAction(EventType.End, ResizeEnd);
            }
        }
        void SetResizeRightHit()
        {
            if (SetState(StateType.ResizeRight))
            {
                SetEventAction(EventType.Drag, ResizeRightDrag);
                SetEventAction(EventType.End, ResizeEnd);
            }
        }
        void SetResizeBottomHit()
        {
            if (SetState(StateType.ResizeBottom))
            {
                SetEventAction(EventType.Drag, ResizeBottomDrag);
                SetEventAction(EventType.End, ResizeEnd);
            }
        }
        void SetResizeTopLeftHit()
        {
            if (SetState(StateType.ResizeTopLeft))
            {
                SetEventAction(EventType.Drag, ResizeTopLeftDrag);
                SetEventAction(EventType.End, ResizeEnd);
            }
        }
        void SetResizeTopRightHit()
        {
            if (SetState(StateType.ResizeTopRight))
            {
                SetEventAction(EventType.Drag, ResizeTopRightDrag);
                SetEventAction(EventType.End, ResizeEnd);
            }
        }
        void SetResizeBottomLeftHit()
        {
            if (SetState(StateType.ResizeBottomLeft))
            {
                SetEventAction(EventType.Drag, ResizeBottomLeftDrag);
                SetEventAction(EventType.End, ResizeEnd);
            }
        }
        void SetResizeBottomRightHit()
        {
            if (SetState(StateType.ResizeBottomRight))
            {
                SetEventAction(EventType.Drag, ResizeBottomRightDrag);
                SetEventAction(EventType.End, ResizeEnd);
            }
        }
        void ResizeEnd()
        {
            _selector.ResizePropagate();
            _selector.RefreshDrawData();
            DrawCanvas.Invalidate();
            RestorePointerCursor();
            SetViewIdle();
        }
        void ResizeTopDrag()
        {
            _selector.ResizeTop();
            UpdateResizerGrid();
        }
        void ResizeLeftDrag()
        {
            _selector.ResizeLeft();
            UpdateResizerGrid();
        }
        void ResizeRightDrag()
        {
            _selector.ResizeRight();
            UpdateResizerGrid();
        }
        void ResizeBottomDrag()
        {
            _selector.ResizeBottom();
            UpdateResizerGrid();
        }
        void ResizeTopLeftDrag()
        {
            _selector.ResizeTopLeft();
            UpdateResizerGrid();
        }
        void ResizeTopRightDrag()
        {
            _selector.ResizeTopRight();
            UpdateResizerGrid();
        }
        void ResizeBottomLeftDrag()
        {
            _selector.ResizeBottomLeft();
            UpdateResizerGrid();
        }
        void ResizeBottomRightDrag()
        {
            _selector.ResizeBottomRight();
            UpdateResizerGrid();
        }
        #endregion

        #region SetViewOnNodeSkim  ============================================
        void SetView_OnNode_Skim()
        {
            if (SetState(StateType.ViewOnNodeSkim))
            {
                SetEventAction(EventType.Skim, View_OnNode_SkimHitTest);
            }
        }
        async void View_OnNode_SkimHitTest()
        {
            var anyHit = false;
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { anyHit = _selector.SkimHitTest(); });
            if (anyHit && _selector.IsHitNode)
            {

            }
        }
        #endregion

        #endregion

        #region Mode_Move  ====================================================
        void SetMoveIdle()
        {
            if (SetState(StateType.MoveIdle))
            {
                RestorePointerCursor();
                HideResizerGrid();
                HideTootlip();
                HideSelectorGrid();
                SetEventAction(EventType.Skim, MoveIdle_SkimHitTest);
                SetEventAction(EventType.Tap, MoveIdle_TapHitTest);
                SetEventAction(EventType.End, SetMoveIdle);
            }
        }
        async void MoveIdle_SkimHitTest()
        {
            var anyHit = false;
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { anyHit = _selector.SkimHitTest(); });
            if (anyHit)
            {
                if (_selector.IsHitRegion || _selector.IsHitNode)
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
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { anyHit = _selector.TapHitTest(); });
            if (anyHit)
            {
                if (_selector.IsHitRegion || _selector.IsHitNode)
                {
                    TrySetNewCursor(CoreCursorType.SizeAll);
                    if (_selector.IsHitRegion)
                        SetMoveRegionDrag();
                    else if (_selector.IsHitNode)
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
            if (SetState(StateType.MoveOnRegionDrag))
            {
                SetEventAction(EventType.Drag, MovingNode);
                SetEventAction(EventType.End, SetMoveIdle);
            }
        }
        async void MovingNode()
        {
            var ok = false;
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { ok = _selector.MoveNode(); });
            if (ok)
            {
                _selector.RefreshDrawData();
                DrawCanvas.Invalidate();
            }
        }
        void SetMoveRegionDrag()
        {
            if (SetState(StateType.MoveOnRegionDrag))
            {
                SetEventAction(EventType.Drag, MovingRegion);
                SetEventAction(EventType.End, SetMoveIdle);
            }
        }
        async void MovingRegion()
        {

        }
        #endregion

        #region Mode_Create  ==================================================
        void SetCreateIdle()
        {
            if (SetState(StateType.CreateIdle))
            {
                SetEventAction(EventType.Tap, CreateNewNode);
            }
        }
        async void CreateNewNode()
        {
            var ok = false;
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { ok = _selector.CreateNode(); });
            DrawCanvas.Invalidate();
            ViewSelect.IsChecked = true;
        }
        #endregion

        #region Mode_Link  ====================================================

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

            if (DrawCanvas != null)
            {
                DrawCanvas.RemoveFromVisualTree();
                DrawCanvas = null;
            }
        }
        #endregion

    }
}
