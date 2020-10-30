using System.Collections.Generic;
using System.Numerics;
using Windows.Storage;

namespace ModelGraph.Core
{
    public class GraphModel : DrawModel
    {
        internal readonly Graph Graph;

        #region Constructor  ==================================================
        internal GraphModel(PageModel owner, Graph graph) : base(owner)
        {
            Graph = graph;

            FlyTreeModel = new TreeModel(owner, null);
            Editor.GetExtent = graph.ResetExtent;
            ShowDrawItems(DrawItem.Overview);

            SetEventAction(DrawEvent.Skim, SkimHitTest);

            SetEventAction(DrawEvent.Tap, () => { AugmentDrawState(DrawState.Tapped, DrawState.EventMask); });
            SetEventAction(DrawEvent.CtrlTap, () => { AugmentDrawState(DrawState.CtrlTapped, DrawState.EventMask); });
            SetEventAction(DrawEvent.ShiftTap, () => { AugmentDrawState(DrawState.ShiftTapped, DrawState.EventMask); });
            SetEventAction(DrawEvent.TapEnd, () => { AugmentDrawState(DrawState.Ending, DrawState.EventMask); });
            SetEventAction(DrawEvent.Drag, () => { AugmentDrawState(DrawState.Dragging, DrawState.EventMask); });
            SetEventAction(DrawEvent.CtrlDrag, () => { AugmentDrawState(DrawState.CtrlDraging, DrawState.EventMask); });
            SetEventAction(DrawEvent.ShiftDrag, () => { AugmentDrawState(DrawState.ShiftDraging, DrawState.EventMask); });
            SetEventAction(DrawEvent.KeyUpArrow, () => { AugmentDrawState(DrawState.UpArrow, DrawState.EventMask); });
            SetEventAction(DrawEvent.KeyLeftArrow, () => { AugmentDrawState(DrawState.LeftArrow, DrawState.EventMask); });
            SetEventAction(DrawEvent.KeyDownArrow, () => { AugmentDrawState(DrawState.DownArrow, DrawState.EventMask); });
            SetEventAction(DrawEvent.KeyRightArrow, () => { AugmentDrawState(DrawState.RightArrow, DrawState.EventMask); });

            SetDrawStateAction(DrawState.ViewMode | DrawState.NowOnVoid, SelectorOnVoid);
            SetDrawStateAction(DrawState.ViewMode | DrawState.NowOnNode, ViewOnNode);
            SetDrawStateAction(DrawState.ViewMode | DrawState.NowOnNode | DrawState.Tapped, ViewOnNodeTapped);
            SetDrawStateAction(DrawState.ViewMode | DrawState.NowOnVoid | DrawState.Tapped, ViewOnVoidTapped);

            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnVoid | DrawState.Tapped, SelectorTapped);
            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnVoid | DrawState.CtrlTapped, SelectorCtrlTapped);
            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnVoid | DrawState.Ending, SelectorEnding);

            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnVoid, MoveOnVoid);
            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnNode, MoveOnNode);
            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnNode | DrawState.Tapped, MoveOnNodeTapped);
            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnNode | DrawState.Dragging, MoveOnNodeDragging);
            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnNode | DrawState.Ending, MoveOnNodeEnding);

            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnNode | DrawState.UpArrow, MoveOnNodeUpArrow);
            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnNode | DrawState.LeftArrow, MoveOnNodeLeftArrow);
            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnNode | DrawState.DownArrow, MoveOnNodeDownArrow);
            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnNode | DrawState.RightArrow, MoveOnNodeRightArrow);

            RefreshEditorData();
        }
        public override void Release()
        {
            var root = Owner.Owner;

            var isOnlyInstance = true;
            foreach (var pm in root.Items) { if (pm.LeadModel is GraphModel gm && gm != this && gm.Graph == Graph) isOnlyInstance = false; }
            if (isOnlyInstance) Graph.Owner.Remove(Graph);

            Owner.Release();
            root.PostRefresh();
        }
        #endregion

        #region RefreshDrawData  ==============================================
        internal void RefreshDrawData()
        {
            RefreshEditorData();
            PageModel.TriggerUIRefresh();
        }

        private void RefreshEditorData()
        {
            Editor.Clear();
            foreach (var e in Graph.Edges)
            {
                var points = new Vector2[] { new Vector2(e.Node1.X, e.Node1.Y), new Vector2(e.Node2.X, e.Node2.Y) };
                Editor.AddParms((points, (ShapeType.Line, StrokeType.Simple, 2), (255, 0, 255, 255)));
            }
            foreach (var n in Graph.Nodes)
            {
                var points = new Vector2[] { new Vector2(n.X, n.Y), new Vector2(n.DX, n.DY) };
                if (SelectedNodes.Contains(n))
                    Editor.AddParms((points, (ShapeType.Rectangle, StrokeType.Filled, 1), (255, 0, 255, 0)));
                else
                    Editor.AddParms((points, (ShapeType.Rectangle, StrokeType.Filled, 1), (255, 255, 0, 255)));
            }
        }
        #endregion

        #region PickerEvents  =================================================
        private void Picker2Select()
        {
            var y = Picker2.Point1.Y;
            var w = Picker2.Extent.Width;
            var x = w / 2;
            var z = (y / w) * w + x;
            Picker2.Clear();
            Picker2.AddParms((new Vector2[] { new Vector2(x, z), new Vector2(x, x) }, (ShapeType.Rectangle, StrokeType.Filled, 0), (63, 255, 255, 255)));
        }
        #endregion

        #region SkimHitTest  ==================================================
        private void SkimHitTest()
        {
            var (ok, node) = HitNodeTest(Editor.Point2);
            if (ok)
            {
                if (node != _hitNode)
                {
                    _hitNode = node;
                    AugmentDrawState(DrawState.NowOnNode, DrawState.NowMask | DrawState.EventMask);
                }
            }
            else
            {
                _hitNode = null;
                AugmentDrawState(DrawState.NowOnVoid, DrawState.NowMask | DrawState.EventMask);
            }
        }
        private (bool, Node) HitNodeTest(Vector2 p)
        {
            foreach (var n in Graph.Nodes)
            {
                if (n.HitTest((p.X, p.Y))) return (true, n);
            }
            return (false, null);
        }

        private Node _hitNode;
        private Edge _hitEdge;
        #endregion

        #region Selector  =====================================================
        private void SelectorOnVoid()
        {
            DrawCursor = DrawCursor.Arrow;
            HideDrawItems(DrawItem.ToolTip);
            PageModel.TriggerUIRefresh();
        }
        private void SelectorTapped()
        {
            _tracingSelector = true;
            _selectToggleMode = false;
            SelectedNodes.Clear();
            DrawCursor = DrawCursor.Arrow;
            HideDrawItems(DrawItem.ToolTip | DrawItem.FlyTree);
            ShowDrawItems(DrawItem.Selector);
            PageModel.TriggerUIRefresh();
        }
        private void SelectorCtrlTapped()
        {
            _tracingSelector = true;
            _selectToggleMode = true;
            DrawCursor = DrawCursor.Arrow;
            HideDrawItems(DrawItem.ToolTip | DrawItem.FlyTree);
            ShowDrawItems(DrawItem.Selector);
            PageModel.TriggerUIRefresh();
        }
        private void SelectorOnVoidDraging()
        {
        }
        private void SelectorEnding()
        {
            if (_tracingSelector)
            {
                _tracingSelector = false;
                var r1 = Editor.Point1;
                var r2 = Editor.Point2;
                foreach (var n in Graph.Nodes)
                {
                    if (n.X < r1.X) continue;
                    if (n.Y < r1.Y) continue;
                    if (n.X > r2.X) continue;
                    if (n.Y > r2.Y) continue;
                    if (_selectToggleMode)
                    {
                        if (SelectedNodes.Contains(n))
                            SelectedNodes.Remove(n);
                        else
                            SelectedNodes.Add(n);
                    }
                    else
                        SelectedNodes.Add(n);
                }
            }
            HideDrawItems(DrawItem.Selector);
            RefreshDrawData();
        }
        private bool _tracingSelector;
        private bool _selectToggleMode;
        private readonly HashSet<Node> SelectedNodes = new HashSet<Node>();
        #endregion

        #region ViewMode  =====================================================
        private void ViewOnNode()
        {
            ToolTip_Text1 = _hitNode.GetNameId();
            ToolTip_Text2 = _hitNode.GetSummaryId();
            var (x, y) = _hitNode.Center;
            FlyOutPoint = new Vector2(x, y);
            DrawCursor = DrawCursor.Hand;
            ShowDrawItems(DrawItem.ToolTip);
            PageModel.TriggerUIRefresh();
        }
        private void ViewOnNodeTapped()
        {
            DrawCursor = DrawCursor.Arrow;
            HideDrawItems(DrawItem.ToolTip);
            ShowDrawItems(DrawItem.FlyTree);
            var (x, y) = _hitNode.Center;
            FlyOutPoint = new Vector2(x, y);
            if (FlyTreeModel is TreeModel tm)
            {
                tm.SetHeaderModel((m) => { new Model_6DA_HitNode(m, _hitNode); });
                FlyOutSize = new Vector2(240, 200);
            }
            PageModel.TriggerUIRefresh();
        }
        private void ViewOnVoidTapped()
        {
            DrawCursor = DrawCursor.Arrow;
            HideDrawItems(DrawItem.ToolTip | DrawItem.FlyTree);
            PageModel.TriggerUIRefresh();
        }
        #endregion

        #region MoveMode  =====================================================
        private void MoveOnVoid()
        {
            DrawCursor = DrawCursor.Arrow;
            PageModel.TriggerUIRefresh();
        }
        private void MoveOnNode()
        {
            DrawCursor = DrawCursor.Hand;
            PageModel.TriggerUIRefresh();
        }
        private void MoveOnNodeTapped()
        {
            DrawCursor = DrawCursor.SizeAll;
            RefreshDrawData();
        }
        private void MoveOnNodeDragging()
        {
            var v = Editor.PointDelta(true);
            if (SelectedNodes.Contains(_hitNode))
            {
                foreach (var n in SelectedNodes)
                {
                    n.Move((v.X,v.Y));
                }
            }
            else
                _hitNode.Move((v.X, v.Y));
            
            RefreshEditorData();
        }
        private void MoveOnNodeEnding()
        {
            DrawCursor = DrawCursor.Arrow;
            PageModel.TriggerUIRefresh();
        }
        private void MoveOnNodeUpArrow() => MoveOnNode(_hitNode, (0, -1));
        private void MoveOnNodeDownArrow() => MoveOnNode(_hitNode, (0, 1));
        private void MoveOnNodeLeftArrow() => MoveOnNode(_hitNode, (-1, 0));
        private void MoveOnNodeRightArrow() => MoveOnNode(_hitNode, (1, 0));
        private void MoveOnNode(Node node, (float,float) ds)
        {
            if (SelectedNodes.Contains(node))
            {
                foreach (var n in SelectedNodes)
                {
                    n.Move(ds);
                }
            }
            else
                node.Move(ds);

            RefreshEditorData();
            PageModel.TriggerUIRefresh();
        }
        #endregion

        #region LinkMode  =====================================================
        private void InitLinkMode()
        {
        }
        #endregion

        #region CopyMode  =====================================================
        private void InitCopyMode()
        {
        }
        #endregion

        #region CreateMode  ===================================================
        private void InitCreateMode()
        {
        }
        #endregion

        #region OperateMode  ==================================================
        private void InitOperateMode()
        {
        }
        #endregion
    }
}

