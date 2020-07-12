﻿using ModelGraph.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace ModelGraph.Controls
{
    public sealed partial class ModelGraphControl
    {
        #region Parameters  ===================================================
        internal enum EventId
        {
            End,
            Drag,
            Hover,
            Wheel,
            Arrow,
            Cancel,
            Begin1,
            Begin3,
            Execute,
            ShortCut,
        }
        internal enum ActionState
        {
            Invalid,

            IdleOnVoid,
            AutoPanning,

            IdleOnNode,
            MovingNode,

            ResizingNode,
            IdleOnNodeResize,

            IdleOnEdge,

            IdleOnRegion,
            MovingRegion,
            TracingRegion,
        }
        private bool SetActionState(ActionState state)
        {
            if (_actionState == state)
            {
                return false;
            }
            _eventAction.Clear();

            Debug.WriteLine($"{state}");
            _actionState = state;
            return true;
        }
        private Dictionary<EventId, Action> _eventAction = new Dictionary<EventId, Action>();
        private ActionState _actionState;
        private bool _enableHitTest;

        private Extent _rootDelta = new Extent(); // rolling point1, point2 delta (RoodCanvas)
        private Extent _drawDelta = new Extent(); // rolling point1, point2 delta (EditorCanvas)
        private Extent _dragDelta = new Extent(); // rolling point1, point2 delta (EditorCanvas)

        private Extent _rootRef = new Extent(); // point1 pointerPressed, point2 = poinnterMoved, pointerReleased (RoodCanvas)
        private Extent _drawRef = new Extent(); // point1 pointerPressed, point2 = poinnterMoved, pointerReleased (EditorCanvas)

        private Modifier _modifier;
        private (float X, float Y) _arrowDelta;
        private int _wheelDelta;

        private Selector _selector;
        #endregion

        #region OnVoid  =======================================================

        #region SetIdleOnVoid  ================================================
        private void SetIdleOnVoid()
        {
            if (!SetActionState(ActionState.IdleOnVoid)) return;

            HideTootlip();
            //DisableAutoPan();
            _enableHitTest = true;
            RestorePointerCursor();

            _eventAction[EventId.End] = () => { RemoveSelectors(); };
            _eventAction[EventId.Drag] = () => { if (_drawRef.Length > 3) { _enableHitTest = false; _selector.StartPoint(_drawRef.Point1); SetTracingRegion(); } };
            _eventAction[EventId.Hover] = IdleHitTest;
            _eventAction[EventId.Wheel] = WheelPanZoom;
            _eventAction[EventId.Cancel] = () => { RemoveSelectors(); };
            _eventAction[EventId.Begin1] = () => { if (_modifier == Modifier.Shift) { SetAutoPanning(); } };
            _eventAction[EventId.Execute] = () => { RemoveSelectors(); _ignorePointerMoved = true; ZoomToExtent(_graph.Extent); };
        }
        #endregion

        #region SetAutoPanning  ===============================================
        private void SetAutoPanning()
        {
            if (!SetActionState(ActionState.AutoPanning))
            {
                return;
            }

            //HideTootlip();
            _enableHitTest = false;
            //EnableAutoPan();
            RestorePointerCursor();

            _eventAction[EventId.End] = SetIdleOnVoid;
            //DragAction = PositionAutoPan;
            _eventAction[EventId.Cancel] = SetIdleOnVoid;
        }
        #endregion

        #region WheelPanZoom  =================================================
        private void WheelPanZoom()
        {
            if (_selector.IsVoidHit)
            {
                // Scroll towards and away from the mouse location when using the mouse wheel
                if ((_modifier & Modifier.Ctrl) != 0)
                {
                    if (_wheelDelta > 0) { ZoomIn(); }
                    else { ZoomOut(); }
                }
                else if ((_modifier & Modifier.Shift) != 0)
                {
                    if (_wheelDelta > 0) { PanLeft(); }
                    else { PanRight(); }
                }
                else
                {
                    if (_wheelDelta > 0) { PanUP(); }
                    else { PanDown(); }
                }
            }
        }
        #endregion

        #endregion

        #region OnNode  =======================================================

        #region SetIdleOnNode  ================================================
        private void SetIdleOnNode()
        {
            SetActionState(ActionState.IdleOnNode);

            _enableHitTest = true;

            ShowNodeTooltip(_selector.HitNode);
            TrySetNewCursor(CoreCursorType.Hand);

            _eventAction[EventId.Hover] = IdleHitTest;
            _eventAction[EventId.Arrow] = () => { Move(_arrowDelta); PostRefresh(); };
            _eventAction[EventId.Begin1] = SetMovingNode;
            //ShortCutAction = IdleOnNodeShortCut;
        }
        #endregion

        #region SetIdleOnNodeResize  ==========================================
        private void SetIdleOnNodeResize()
        {
            SetActionState(ActionState.IdleOnNodeResize);

            //HideTootlip();
            _enableHitTest = true;
            var resizer = _selector.Resizer;

            if (resizer == ResizerType.Left || resizer == ResizerType.Right)
                TrySetNewCursor(CoreCursorType.SizeWestEast);
            else if (resizer == ResizerType.Top || resizer == ResizerType.Bottom)
                TrySetNewCursor(CoreCursorType.SizeNorthSouth);
            else
                SetIdleOnNode();

            _eventAction[EventId.End] = () => { SetIdleOnVoid(); };
            _eventAction[EventId.Hover] = IdleHitTest;
            _eventAction[EventId.Begin1] = SetResizingNode;
        }
        #endregion

        #region SetResizingNode  ==============================================
        private void SetResizingNode()
        {
            if (!SetActionState(ActionState.ResizingNode)) return;
            var resizer = _selector.Resizer;
            if (resizer == ResizerType.None)
            {
                SetIdleOnNode();
                return;
            }

            double dx = 0, dy = 0;

            //HideTootlip();
            _enableHitTest = false;

            _eventAction[EventId.End] = () => { PostRefresh(); SetIdleOnVoid(); };
            _eventAction[EventId.Hover] = () => { SetIdleOnVoid(); };
            _eventAction[EventId.Drag] = () => { TryResize(_dragDelta.Delta); _dragDelta.Record(_drawRef.Point2); };
            //_eventAction[EventId.Arrow] = () => { TryResize(_arrowDelta); PostRefresh(); };

            void TryResize((float X, float Y) delta)
            {
                dx += _dragDelta.Delta.X;
                dy += _dragDelta.Delta.Y;
                var x = (int)dx;
                var y = (int)dy;
                if (x > 1 || x < -1 || y > 1 || y < -1)
                {
                    dx -= x;
                    dy -= y;
                    Resize((x, y), resizer);
                }
            }
        }
        #endregion

        #region SetMovingNode  ================================================
        private void SetMovingNode()
        {
            if (!SetActionState(ActionState.MovingNode))
            {
                return;
            }

            HideTootlip();
            TrySetNewCursor(CoreCursorType.SizeAll);
            _enableHitTest = false;

            _eventAction[EventId.End] = () => { SetIdleOnNode(); PostRefresh(); };
            _eventAction[EventId.Drag] = () => {  Move(_dragDelta.Delta); _dragDelta.Record(_drawRef.Point2); };
            _eventAction[EventId.Arrow] = () => { Move(_arrowDelta); PostRefresh(); };
        }
        #endregion

        #endregion

        #region OnEdge  =======================================================

        #region SetIdleOnEdge  ================================================
        private void SetIdleOnEdge()
        {
            if (!SetActionState(ActionState.IdleOnEdge))
            {
                return;
            }

            ShowEdgeTooltip(_selector.HitEdge);
            TrySetNewCursor(CoreCursorType.Hand);
            _enableHitTest = true;

            _eventAction[EventId.Hover] = IdleHitTest;
        }
        #endregion

        #endregion

        #region OnRegion  =====================================================

        #region SetIdleOnRegion  ==============================================
        private void SetIdleOnRegion()
        {
            if (!SetActionState(ActionState.IdleOnRegion))
            {
                return;
            }

            HideTootlip();
            TrySetNewCursor(CoreCursorType.Hand);
            _enableHitTest = true;

            _eventAction[EventId.Hover] = IdleHitTest;
            _eventAction[EventId.Arrow] = () => { Move(_arrowDelta); UpdateRegionExtents(); PostRefresh(); };
            _eventAction[EventId.Cancel] = () => { RemoveSelectors(); SetIdleOnVoid(); };
            _eventAction[EventId.Begin1] = SetMovingRegion;
            _eventAction[EventId.Execute] = () => { var e = Extent.Create(_selector.Nodes, 16); _ignorePointerMoved = true; ZoomToExtent(e); };
        }
        #endregion

        #region SetMovingRegion  ==============================================
        private void SetMovingRegion()
        {
            if (!SetActionState(ActionState.MovingRegion))
            {
                return;
            }

            _enableHitTest = false;
            TrySetNewCursor(CoreCursorType.SizeAll);

            _eventAction[EventId.End] = () => { UpdateRegionExtents(); SetIdleOnRegion(); PostRefresh(); };
            _eventAction[EventId.Drag] = () => { Move(_dragDelta.Delta); _dragDelta.Record(_drawRef.Point2); };
            _eventAction[EventId.Arrow] = () => { Move(_arrowDelta); };
            _eventAction[EventId.Cancel] = () => { RemoveSelectors(); SetIdleOnRegion(); }; ;
        }
        #endregion

        #region SetTracingRegion  =============================================
        private void SetTracingRegion()
        {
            if (!SetActionState(ActionState.TracingRegion))
            {
                return;
            }

            // Cursor = Cursors.Pen;
            _enableHitTest = false;
            
            _eventAction[EventId.End] = () => { CloseRegion();  };
            _eventAction[EventId.Drag] = () => { _selector.NextPoint(_drawRef.Point2); };
            _eventAction[EventId.Cancel] = () => { RemoveSelectors(); SetIdleOnVoid(); }; ;
        }
        private void TraceRegion()
        {
            if (_selector.Extent.HasArea) return;

            _selector.NextPoint(_drawRef.Point2);
            EditorCanvas.Invalidate();
        }
        private async void CloseRegion()
        {
            _selector.NextPoint(_drawRef.Point2);
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, (Windows.UI.Core.DispatchedHandler)(() => { _selector.TryAdd(); }));
            _selector.Extent.Clear();

            EditorCanvas.Invalidate();
            SetIdleOnVoid();
        }
        #endregion

        #endregion

        #region SelectorAction  ===============================================
        private async void Resize((float X, float Y) delta, ResizerType resizer)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => _selector.Resize(delta, resizer));
        }

        private async void Move((float X, float Y) delta)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => _selector.Move(delta));
        }
        private async void RotateLeft45()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => _selector.RotateLeft45());
            PostRefresh();
        }
        private async void RotateRight45()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => _selector.RotateRight45());
            PostRefresh();
        }
        private async void RotateLeft90()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => _selector.RotateLeft90());
            PostRefresh();
        }
        private async void RotateRight90()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => _selector.RotateRight90());
            PostRefresh();
        }
        private async void HitTest((float X, float Y) point)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => _selector.HitTest(point));
        }
        private async void AlignVert()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => _selector.AlignVert());
            PostRefresh();
        }
        private async void AlignHorz()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => _selector.AlignHorz());
            PostRefresh();
        }
        private async void FlipVert()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => _selector.FlipVert());
            PostRefresh();
        }
        private async void FlipHorz()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>  _selector.FlipHorz());
            PostRefresh();
        }
        private async void GravityInside()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => _selector.GravityInside());
            PostRefresh();
        }
        private async void GravityDisperse()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => _selector.GravityDisperse());
            PostRefresh();
        }
        private async void UpdateRegionExtents()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => _selector.UpdateExtents());
        }
        #endregion

        #region PostRefresh  ==================================================
        async void PostRefresh()
        {
            HideTootlip();
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { _graph.AdjustGraph(_selector); }); 
            //IModel.PostRefresh();
            UpdateUndoRedoControls();
            _selector.EnableSnapshot();
        }
        #endregion

        #region IdleHitTest  ==================================================
        private void IdleHitTest()
        {
            if (_selector.IsVoidHit)
            {
                SetIdleOnVoid();
            }
            else if (_selector.IsRegionHit)
            {
                SetIdleOnRegion();
            }
            else if (_selector.IsNodeHit)
            {
                if (_selector.Resizer != ResizerType.None)
                    SetIdleOnNodeResize();
                else
                    SetIdleOnNode();
            }

            else if (_selector.IsEdgeHit)
            {
                SetIdleOnEdge();
            }
        }
        #endregion

        #region PointerCursor  ================================================
        private void RestorePointerCursor() =>  TrySetNewCursor(CoreCursorType.Arrow);
        private void TrySetNewCursor(CoreCursorType cursorType)
        {
            if (_currentCusorType == cursorType) return;
            if (_cursors.TryGetValue(cursorType, out CoreCursor newCursor))
            {
                _currentCusorType = cursorType;
                Window.Current.CoreWindow.PointerCursor = newCursor;
            }
        }
        private CoreCursorType _currentCusorType;
        readonly Dictionary<CoreCursorType, CoreCursor> _cursors = new Dictionary<CoreCursorType, CoreCursor>()
        {
            [CoreCursorType.Pin] = new CoreCursor(CoreCursorType.Pin, 0),
            [CoreCursorType.Hand] = new CoreCursor(CoreCursorType.Hand, 0),
            [CoreCursorType.Wait] = new CoreCursor(CoreCursorType.Wait, 0),
            [CoreCursorType.Help] = new CoreCursor(CoreCursorType.Help, 0),
            [CoreCursorType.Arrow] = new CoreCursor(CoreCursorType.Arrow, 0),
            [CoreCursorType.IBeam] = new CoreCursor(CoreCursorType.IBeam, 0),
            [CoreCursorType.Cross] = new CoreCursor(CoreCursorType.Cross, 0),
            [CoreCursorType.Person] = new CoreCursor(CoreCursorType.Person, 0),
            [CoreCursorType.UpArrow] = new CoreCursor(CoreCursorType.UpArrow, 0),
            [CoreCursorType.SizeAll] = new CoreCursor(CoreCursorType.SizeAll, 0),
            [CoreCursorType.UniversalNo] = new CoreCursor(CoreCursorType.UniversalNo, 0),
            [CoreCursorType.SizeWestEast] = new CoreCursor(CoreCursorType.SizeWestEast, 0),
            [CoreCursorType.SizeNorthSouth] = new CoreCursor(CoreCursorType.SizeNorthSouth, 0),
            [CoreCursorType.SizeNortheastSouthwest] = new CoreCursor(CoreCursorType.SizeNortheastSouthwest, 0),
            [CoreCursorType.SizeNorthwestSoutheast] = new CoreCursor(CoreCursorType.SizeNorthwestSoutheast, 0),
        }; 
        #endregion

        #region Commands  =====================================================
        #region Pan
        //=====================================================================
        const double scrollDelta = 120;

        internal void PanUP() { ScrollVerticalDelta(-scrollDelta); }
        internal void PanDown() { ScrollVerticalDelta(scrollDelta); }
        internal void PanLeft() { ScrollHorizontalDelta(-scrollDelta); }
        internal void PanRight() { ScrollHorizontalDelta(scrollDelta); }

        internal bool CanPanUp() { return true; }
        internal bool CanPanDown() { return true; }
        internal bool CanPanLeft() { return true; }
        internal bool CanPanRight() { return true; }
        //=====================================================================
        #endregion

        #region Zoom
        //=====================================================================

        internal bool CanZoomIn() { return true; }
        internal bool CanZoomOut() { return true; }
        internal bool CanZoomToObject() { return true; }
        internal bool CanZoomToExtent(Extent extent) { return true; }
        //=====================================================================
        #endregion

        #region Cut
        //=====================================================================
        internal void Cut()
        {
        }
        internal bool CanCut()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region Copy
        //=====================================================================
        internal void Copy()
        {
        }
        internal bool CanCopy()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region Undo
        //=====================================================================
        internal void Undo()
        {
        }
        internal bool CanUndo()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region Redo
        //=====================================================================
        internal void Redo()
        {
        }
        internal bool CanRedo()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region Find
        //=====================================================================
        internal bool Find(string item)
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region Paste
        //=====================================================================
        internal void Paste()
        {
        }
        internal bool CanPaste()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region Delete
        //=====================================================================
        internal void Delete()
        {
        }
        internal bool CanDelete()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region JumpTo
        //=====================================================================
        internal void JumpTo()
        {
        }
        internal bool CanJumpTo()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region Export
        //=====================================================================
        internal void Export()
        {
        }
        internal bool CanExport()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region Import
        //=====================================================================
        internal void Import()
        {
        }
        internal bool CanImport()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region SetName
        //=====================================================================
        internal void SetName(string name)
        {
        }
        internal bool CanSetName(string name)
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region SelectAll
        //=====================================================================
        internal void SelectAll()
        {
        }
        internal bool CanSelectAll()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region Disconnect
        //=====================================================================
        internal void Disconnect()
        {
        }
        internal bool CanDisconnect()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region PanToOther
        //=====================================================================
        internal void PanToOther()
        {
        }
        internal bool CanPanTo()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region AutoLayout
        //=====================================================================
        internal void AutoLayout()
        {
        }

        internal bool CanAutoLayout()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region PixelNudge
        //=====================================================================
        internal void PixelNudgeUp()
        {
            PixelNudge(new Vector2(0, 1));
        }
        internal void PixelNudgeDown()
        {
            PixelNudge(new Vector2(0, -1));
        }
        internal void PixelNudgeLeftOne()
        {
            PixelNudge(new Vector2(1, 0));
        }
        internal void PixelNudgeRightOne()
        {
            PixelNudge(new Vector2(-1, 0));
        }
        internal void PixelNudgeUpTend()
        {
            PixelNudge(new Vector2(0, 10));
        }
        internal void PixelNudgeDownTen()
        {
            PixelNudge(new Vector2(0, -10));
        }
        internal void PixelNudgeLeftTen()
        {
            PixelNudge(new Vector2(10, 0));
        }
        internal void PixelNudgeRightTen()
        {
            PixelNudge(new Vector2(-10, 0));
        }

        private void PixelNudge(Vector2 delta)
        {
        }

        private bool CanPixelNudge()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region QueryRoot
        //=====================================================================
        internal void QueryRoot()
        {
        }
        internal bool CanQueryRoot()
        {
            return true;
        }
        //=====================================================================
        #endregion

        #region GoForward
        //=====================================================================
        internal void GoForward()
        {
        }
        internal bool CanGoForward()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region GoBackward
        //=====================================================================
        internal void GoBackward()
        {
        }
        internal bool CanGoBackward()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region SetBarWidth
        //=====================================================================
        internal void SetBarWidth()
        {
        }
        internal bool CanSetBarWidth()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region SetResizing
        //=====================================================================
        internal void SetResizing()
        {
        }
        internal bool CanSetResizing()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region SetLabeling
        //=====================================================================
        internal void SetLabeling()
        {
        }
        internal bool CanSetLabeling()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region CenterInView
        //=====================================================================
        internal void CenterInView()
        {
        }
        internal bool CanCenterInView()
        {
            return true;
        }
        //=====================================================================
        #endregion

        #region Set<Central,Vertical,Horizontal>
        //=====================================================================
        internal void SetCentral()
        {
        }
        internal bool CanSetCentral()
        {
            return false;
        }

        internal void SetVertical()
        {
        }
        internal bool CanSetVertical()
        {
            return false;
        }

        internal void SetHorizontal()
        {
        }
        internal bool CanSetHorizontal()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region SetAxisStyle
        //=====================================================================
        internal void SetAxisStyle()
        {
        }
        internal bool CanSetAxisStyle()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region SetAlignment
        //=====================================================================
        internal void SetAlignment()
        {
        }
        internal bool CanSetAlignment()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region SetRegionNode
        //=====================================================================
        internal void SetRegionNode()
        {
        }
        internal bool CanSetRegionNode()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region SetOrientation
        //=====================================================================
        internal void SetOrientation()
        {
        }
        internal bool CanSetOrientation()
        {
            return false;
        }
        //=====================================================================
        #endregion

        #region BendPoint
        //===================================================
        internal void AddBendPoint()
        {
        }
        internal bool CanAddBendPoint()
        {
            return false;
        }
        internal void RemoveBendPoint()
        {
        }
        internal bool CanRemoveBendPoint()
        {
            return false;
        }
        internal void RemoveAllBendPoints()
        {
        }
        internal bool CanRemoveAllBendPoints()
        {
            return false;
        }
        //=====================================================================
        #endregion
        #endregion
    }
}
