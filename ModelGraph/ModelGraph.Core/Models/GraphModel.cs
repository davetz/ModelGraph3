using System.Collections.Generic;
using System.Numerics;
using Windows.Storage;

namespace ModelGraph.Core
{
    public class GraphModel : DrawModel
    {
        internal readonly Graph Graph;
        private Selector Selector => Graph.Selector;

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

            #region AddEdgeParms  =============================================
            for (int i = 0; i < Graph.EdgeCount; i++)
            {
                var edge = Graph.Edges[i];
                var points = edge.Points;
                var ipc = edge.LineColor;
                if (ipc == 0)
                {
                    var i1 = edge.Node1.Color;
                    var i2 = edge.Node2.Color;
                    ipc = (i1 > i2) ? i1 : i2;
                }
                Editor.AddParms((points, (ShapeType.JointedLines, StrokeType.Simple, 1), Graph.Owner.Color.ARGBList[ipc]));
            }
            #endregion

            #region AddNodeParms  =============================================
            var scale = Graph.Owner.SymbolSize;
            foreach (var n in Graph.Nodes)
            {
                var points = new Vector2[] { new Vector2(n.X, n.Y), new Vector2(n.DX, n.DY) };
                var k = n.Symbol - 2;
                if (k < 0)
                {
                    if (n.IsNodePoint)
                    {
                        Editor.AddParms((points, (ShapeType.Circle, StrokeType.Filled, 1), (255, 255, 0, 255)));
                    }
                    else
                    {
                        Editor.AddParms((points, (ShapeType.Rectangle, StrokeType.Filled, 1), (255, 255, 0, 255)));
                    }
                }
                else
                {
                    var sym = Graph.Owner.Symbols[k];
                    var shapes = sym.GetShapes();
                    foreach (var s in shapes)
                    {
                        s.AddDrawData(Editor, scale, points[0], n.FlipState);
                    }
                }
            }
            #endregion

            #region AddRegionParms  ===========================================
            var m = 4;
            foreach (var n in Selector.Nodes)
            {
                var points = new Vector2[] { n.Center, new Vector2(n.DX + m, n.DY + m) };
                Editor.AddParms((points, (ShapeType.Rectangle, StrokeType.Filled, 1), (100, 255, 200, 200)));
            }
            #endregion
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
            Selector.HitTest(Editor.Point2);
            if (Selector.IsNodeHit && Selector.HitNode != Selector.PrevNode)
                AugmentDrawState(DrawState.NowOnNode, DrawState.NowMask | DrawState.EventMask);
            else if (Selector.IsVoidHit)
                AugmentDrawState(DrawState.NowOnVoid, DrawState.NowMask | DrawState.EventMask);
        }
        private (bool, Node) HitNodeTest(Vector2 p)
        {
            foreach (var n in Graph.Nodes)
            {
                if (n.HitTest(p)) return (true, n);
            }
            return (false, null);
        }
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
            Selector.Clear();
            Selector.SetPoint1(Editor.Point1);
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
                Selector.SetPoint2(Editor.Point2);
                Selector.TryAdd(_selectToggleMode);
            }
            HideDrawItems(DrawItem.Selector);
            RefreshDrawData();
        }
        private bool _tracingSelector;
        private bool _selectToggleMode;
        #endregion

        #region ViewMode  =====================================================
        private void ViewOnNode()
        {
            ToolTip_Text1 = Selector.HitNode.GetNameId();
            ToolTip_Text2 = Selector.HitNode.GetSummaryId();
            FlyOutPoint = Selector.HitNode.Center;
            DrawCursor = DrawCursor.Hand;
            ShowDrawItems(DrawItem.ToolTip);
            PageModel.TriggerUIRefresh();
        }
        private void ViewOnNodeTapped()
        {
            DrawCursor = DrawCursor.Arrow;
            HideDrawItems(DrawItem.ToolTip);
            ShowDrawItems(DrawItem.FlyTree);
            FlyOutPoint = Selector.HitNode.Center;
            if (FlyTreeModel is TreeModel tm)
            {
                tm.SetHeaderModel((m) => { new Model_6DA_HitNode(m, Selector.HitNode); });
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
            Selector.Move(v);
            RefreshEditorData();
        }

        private void MoveOnNodeEnding()
        {
            DrawCursor = DrawCursor.Arrow;
            PageModel.TriggerUIRefresh();
        }
        private void MoveOnNodeUpArrow() => MoveOnNode(Selector.HitNode, new Vector2(0, -1));
        private void MoveOnNodeDownArrow() => MoveOnNode(Selector.HitNode, new Vector2(0, 1));
        private void MoveOnNodeLeftArrow() => MoveOnNode(Selector.HitNode, new Vector2(-1, 0));
        private void MoveOnNodeRightArrow() => MoveOnNode(Selector.HitNode, new Vector2(1, 0));
        private void MoveOnNode(Node node, Vector2 ds)
        {
            Selector.Move(ds);
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

