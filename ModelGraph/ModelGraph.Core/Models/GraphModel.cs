using System.Collections.Generic;
using System.Numerics;

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

            SetEventAction(DrawEvent.Skim, SkimEventAction);
            SetEventAction(DrawEvent.Tap, TapEventAction);
            SetDrawStateAction(DrawState.ViewOnVoid, ViewOnVoidAction);
            SetDrawStateAction(DrawState.ViewOnNode, ViewOnNodeAction);
            SetDrawStateAction(DrawState.ViewOnNodeTapped, ViewOnNodeTappedAction);
            SetDrawStateAction(DrawState.ViewOnVoidTapped, ViewOnVoidTappedAction);

            RefreshEditorData();
        }
        #endregion

        #region RefreshDrawData  ==============================================
        private void RefreshDrawData()
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

        #region DrawEvents  ===================================================
        private void SkimEventAction()
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
        private void TapEventAction()
        {
            AugmentDrawState(DrawState.Tapped, DrawState.EventMask);
        }
        private Node _hitNode;
        private Edge _hitEdge;
        private List<Node> _regionNodes = new List<Node>();
        #endregion

        #region DrawStates  ===================================================

        #region ViewMode  =====================================================
        private void ViewOnVoidAction()
        {
            DrawCursor = DrawCursor.Arrow;
            HideDrawItems(DrawItem.ToolTip);
            PageModel.TriggerUIRefresh();
        }
        private void ViewOnNodeAction()
        {
            ToolTip_Text1 = _hitNode.GetNameId();
            ToolTip_Text2 = _hitNode.GetSummaryId();
            var (x, y) = _hitNode.Center;
            FlyOutPoint = new Vector2(x, y);
            DrawCursor = DrawCursor.Hand;
            ShowDrawItems(DrawItem.ToolTip);
            HideDrawItems(DrawItem.FlyTree);
            PageModel.TriggerUIRefresh();
        }
        private void ViewOnVoidTappedAction()
        {
            DrawCursor = DrawCursor.Arrow;
            HideDrawItems(DrawItem.ToolTip | DrawItem.FlyTree);
            PageModel.TriggerUIRefresh();
        }
        private void ViewOnNodeTappedAction()
        {
            DrawCursor = DrawCursor.Arrow;
            HideDrawItems(DrawItem.ToolTip);
            ShowDrawItems(DrawItem.FlyTree);
            if (FlyTreeModel is TreeModel tm)
            {
                tm.SetHeaderModel((m) => { new Model_6DA_HitNode(m, _hitNode); });
                FlyOutSize = new Vector2(240, 200);
            }
            PageModel.TriggerUIRefresh();
        }

        private void ClearRegion() => _regionNodes.Clear();
        private bool IsValidRegion()
        {
            var r1 = RegionPoint1;
            var r2 = RegionPoint2;
            _regionNodes.Clear();
            foreach (var n in Graph.Nodes)
            {
                if (n.X < r1.X) continue;
                if (n.Y < r1.Y) continue;
                if (n.X > r2.X) continue;
                if (n.Y > r2.Y) continue;
                _regionNodes.Add(n);
            }
            return _regionNodes.Count > 0;
        }
        private bool RegionHitTest(Vector2 p)
        {
            var r1 = RegionPoint1;
            var r2 = RegionPoint2;
            if (p.X < r1.X) return false;
            if (p.Y < r1.Y) return false;
            if (p.X > r2.X) return false;
            if (p.Y > r2.Y) return false;
            return true;
        }

        private (bool, Node) HitNodeTest(Vector2 p)
        {
            foreach (var n in Graph.Nodes)
            {
                if (n.HitTest((p.X, p.Y))) return (true, n);
            }
            return (false, null);
        }
        #endregion

        #region MoveMode  =====================================================
        private void InitMoveMode()
        {
        }
        private bool MoveNode()
        {
            if (_hitNode is null) return false;
            var delta = EditorData.PointDelta(true);
            _hitNode.X += delta.X;
            _hitNode.Y += delta.Y;
            return true;
        }

        private bool MoveRegion()
        {
            var delta = EditorData.PointDelta(true);
            foreach (var n in _regionNodes)
            {
                n.X += delta.X;
                n.Y += delta.Y;
            }
            return true;
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

        #endregion
    }
}

