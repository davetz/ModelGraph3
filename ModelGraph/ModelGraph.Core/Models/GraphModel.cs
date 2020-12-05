using System.Collections.Generic;
using System.Numerics;
using Windows.Storage;

namespace ModelGraph.Core
{
    public class GraphModel : DrawModel
    {
        internal readonly Graph Graph;
        internal readonly Selector Selector;

        #region Constructor  ==================================================
        internal GraphModel(PageModel owner, Graph graph) : base(owner)
        {
            Graph = graph;
            Selector = new Selector(graph);

            FlyTreeModel = new TreeModel(owner, null);
            Editor.GetExtent = graph.ResetExtent;

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
            SetEventAction(DrawEvent.ContextMenu, () => { AugmentDrawState(DrawState.ContextMenu, DrawState.EventMask); });

            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnVoid, SelectorOnVoid);
            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnNode, EditOnNode);
            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnVoid | DrawState.Tapped, EditOnVoidTapped);
            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnNode | DrawState.Dragging, EditOnNodeDragging);
            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnNode | DrawState.Ending, EditOnNodeEnding);


            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnVoid | DrawState.Tapped, SelectorTapped);
            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnVoid | DrawState.CtrlTapped, SelectorCtrlTapped);
            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnVoid | DrawState.Ending, SelectorEnding);

            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnNode | DrawState.Tapped, EditOnNodeTapped);

            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnNode | DrawState.UpArrow, EditOnNodeUpArrow);
            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnNode | DrawState.LeftArrow, EditOnNodeLeftArrow);
            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnNode | DrawState.DownArrow, EditOnNodeDownArrow);
            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnNode | DrawState.RightArrow, EditOnNodeRightArrow);

            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnNode | DrawState.ContextMenu, EditOnNodeContextMenu);

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

        #region FullRefresh/RefreshEditorData  ================================
        internal void FullRefresh()
        {
            if (ModelDelta != Graph.ModelDelta)
            {
                ModelDelta = Graph.ModelDelta;
                Graph.RebuildHitTestMap();
            }
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
                var points = new Vector2[] { n.Center, n.Radius };
                var k = n.Symbol - 2;
                if (k < 0 || k >= Graph.Owner.SymbolCount)
                {
                    if (n.IsNodePoint)
                    {
                        Editor.AddParms((points, (ShapeType.Circle, StrokeType.Filled, 1), (255, 255, 0, 255)));
                    }
                    else
                    {
                        Editor.AddParms((points, (ShapeType.CenterRect, StrokeType.Filled, 1), (255, 255, 0, 255)));
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
                Editor.AddParms((points, (ShapeType.CenterRect, StrokeType.Filled, 1), (100, 255, 200, 200)));
            }
            #endregion

            #region AddHitTestMapSectors  =====================================
            //Graph.HitTestMap.AddDrawParms(Editor);  //debug HitTestMap
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
            Picker2.AddParms((new Vector2[] { new Vector2(x, z), new Vector2(x, x) }, (ShapeType.CenterRect, StrokeType.Filled, 0), (63, 255, 255, 255)));
        }
        #endregion

        #region SkimHitTest  ==================================================
        private void SkimHitTest()
        {
            Selector.HitTestPoint(Editor.Point2);
            if (Selector.IsNodeHit && Selector.HitNode != Selector.PrevNode)
                AugmentDrawState(DrawState.NowOnNode, DrawState.NowMask | DrawState.EventMask);
            else if (Selector.IsVoidHit)
                AugmentDrawState(DrawState.NowOnVoid, DrawState.NowMask | DrawState.EventMask);
        }
        #endregion

        #region Selector  =====================================================
        private void SelectorOnVoid()
        {
            DrawCursor = DrawCursor.Arrow;
            HideDrawItems(DrawItem.ToolTip);
            if (ModelDelta != Graph.ModelDelta)
                Selector.UpdateModels();
            else
                PageModel.TriggerUIRefresh();
        }
        private void SelectorTapped()
        {
            _tracingSelector = true;
            _selectToggleMode = false;
            Selector.Clear();
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
        private void SelectorEnding()
        {
            if (_tracingSelector)
            {
                _tracingSelector = false;
                Selector.HitTestRegion(_selectToggleMode, Editor.Point1, Editor.Point2);
            }
            HideDrawItems(DrawItem.Selector);
            FullRefresh();
        }
        private bool _tracingSelector;
        private bool _selectToggleMode;
        #endregion

        #region EditMode  =====================================================
        private void EditOnNodeTapped()
        {
            Selector.SaveHitReference();
            HideDrawItems(DrawItem.ToolTip | DrawItem.FlyTree);
            DrawCursor = DrawCursor.SizeAll;
            FullRefresh();
        }
        private void EditOnNode()
        {
            ToolTip_Text1 = Selector.HitNode.GetNameId();
            ToolTip_Text2 = Selector.HitNode.GetSummaryId();
            FlyOutPoint = Selector.HitNode.Center;
            DrawCursor = DrawCursor.Hand;
            ShowDrawItems(DrawItem.ToolTip);
            PageModel.TriggerUIRefresh();
        }
        private void EditOnNodeContextMenu()
        {
            DrawCursor = DrawCursor.Arrow;
            HideDrawItems(DrawItem.ToolTip);
            if (FlyTreeModel is TreeModel tm)
            {
                Selector.SaveHitReference();
                DrawCursor = DrawCursor.Arrow;
                HideDrawItems(DrawItem.ToolTip);
                ShowDrawItems(DrawItem.FlyTree);
                FlyOutPoint = Selector.HitNode.Center;
                if (Selector.Nodes.Count > 1)
                {
                    tm.SetHeaderModel((m) => { new Model_6DB_MoveNodeMenu(m, this); });
                    FlyOutSize = new Vector2(280, 200);
                }
                else
                {
                    tm.SetHeaderModel((m) => { new Model_6DA_HitNode(m, Selector.HitNode); });
                    FlyOutSize = new Vector2(240, 200);
                }
            }

            PageModel.TriggerUIRefresh();
        }
        private void EditOnVoidTapped()
        {
            DrawCursor = DrawCursor.Arrow;
            HideDrawItems(DrawItem.ToolTip | DrawItem.FlyTree);
            PageModel.TriggerUIRefresh();
        }
        private void EditOnNodeDragging()
        {
            var v = Editor.PointDelta(true);
            Selector.Move(v);
            RefreshEditorData();
        }

        private void EditOnNodeEnding()
        {
            DrawCursor = DrawCursor.Hand;
            PageModel.TriggerUIRefresh();
        }
        private void EditOnNodeUpArrow() => MoveOnNodeDelta(Selector.HitNode, new Vector2(0, -1));
        private void EditOnNodeDownArrow() => MoveOnNodeDelta(Selector.HitNode, new Vector2(0, 1));
        private void EditOnNodeLeftArrow() => MoveOnNodeDelta(Selector.HitNode, new Vector2(-1, 0));
        private void EditOnNodeRightArrow() => MoveOnNodeDelta(Selector.HitNode, new Vector2(1, 0));
        private void MoveOnNodeDelta(Node node, Vector2 delta)
        {
            ModelDelta++;
            Selector.Move(delta);
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

