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

            SetEventAction(DrawEvent.Tap, () => { TestSelectorTap(); });
            SetEventAction(DrawEvent.CtrlTap, () => { TestSelectorCtrlTap(); });
            SetEventAction(DrawEvent.ShiftTap, () => { AugmentDrawState(DrawState.ShiftTapStart, DrawState.EventMask); });
            SetEventAction(DrawEvent.TapEnd, () => { TestSelectorTapEnd(); });


            SetEventAction(DrawEvent.Drag, () => { AugmentDrawState(DrawState.Dragging, DrawState.EventMask); });
            SetEventAction(DrawEvent.CtrlDrag, () => { AugmentDrawState(DrawState.CtrlDraging, DrawState.EventMask); });
            SetEventAction(DrawEvent.ShiftDrag, () => { AugmentDrawState(DrawState.ShiftDraging, DrawState.EventMask); });
            SetEventAction(DrawEvent.KeyUpArrow, () => { AugmentDrawState(DrawState.UpArrow, DrawState.EventMask); });
            SetEventAction(DrawEvent.KeyLeftArrow, () => { AugmentDrawState(DrawState.LeftArrow, DrawState.EventMask); });
            SetEventAction(DrawEvent.KeyDownArrow, () => { AugmentDrawState(DrawState.DownArrow, DrawState.EventMask); });
            SetEventAction(DrawEvent.KeyRightArrow, () => { AugmentDrawState(DrawState.RightArrow, DrawState.EventMask); });
            SetEventAction(DrawEvent.ContextMenu, () => { AugmentDrawState(DrawState.ContextMenu, DrawState.EventMask); });

            SetEventAction(DrawEvent.SetAddMode, () => { AugmentDrawState(DrawState.AddMode, DrawState.ModeMask); });
            SetEventAction(DrawEvent.SetViewMode, () => { AugmentDrawState(DrawState.ViewMode, DrawState.ModeMask); });
            SetEventAction(DrawEvent.SetEditMode, () => { AugmentDrawState(DrawState.EditMode, DrawState.ModeMask); });
            SetEventAction(DrawEvent.SetMoveMode, () => { AugmentDrawState(DrawState.MoveMode, DrawState.ModeMask); });
            SetEventAction(DrawEvent.SetCopyMode, () => { AugmentDrawState(DrawState.CopyMode, DrawState.ModeMask); });
            SetEventAction(DrawEvent.SetLinkMode, () => { AugmentDrawState(DrawState.LinkMode, DrawState.ModeMask); });
            SetEventAction(DrawEvent.SetUnlinkMode, () => { AugmentDrawState(DrawState.UnlinkMode, DrawState.ModeMask); });
            SetEventAction(DrawEvent.SetCreateMode, () => { AugmentDrawState(DrawState.CreateMode, DrawState.ModeMask); });
            SetEventAction(DrawEvent.SetDeleteMode, () => { AugmentDrawState(DrawState.DeleteMode, DrawState.ModeMask); });
            SetEventAction(DrawEvent.SetGravityMode, () => { AugmentDrawState(DrawState.GravityMode, DrawState.ModeMask); });
            SetEventAction(DrawEvent.SetOperateMode, () => { AugmentDrawState(DrawState.OperateMode, DrawState.ModeMask); });


            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnNode, EditOnNode);
            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnVoid | DrawState.TapStart, EditOnVoidTapped);
            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnNode | DrawState.Ending, EditOnNodeEnding);

            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnVoid | DrawState.CtrlTapStart, SelectorOnVoidCtrlTapped);
            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnVoid | DrawState.CtrlTapStart, SelectorOnVoidCtrlTapped);

            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnNode, MoveOnNode);

            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnNode | DrawState.TapStart, MoveOnNodeTapped);
            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnNode | DrawState.Dragging, MoveOnNodeDragging);
            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnNode | DrawState.Ending, MoveOnNodeEnding);

            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnNode | DrawState.UpArrow, MoveOnNodeUpArrow);
            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnNode | DrawState.LeftArrow, MoveOnNodeLeftArrow);
            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnNode | DrawState.DownArrow, MoveOnNodeDownArrow);
            SetDrawStateAction(DrawState.MoveMode | DrawState.NowOnNode | DrawState.RightArrow, MoveOnNodeRightArrow);

            SetDrawStateAction(DrawState.EditMode | DrawState.NowOnNode | DrawState.TapStart, EditOnNodeTapped);

            UpdateEditorData();
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

        #region Refresh/UpdateEditorData  =====================================
        internal void Refresh()
        {
            UpdateEditorData();
            PageModel.TriggerUIRefresh();
        }

        private void UpdateEditorData()
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
                SelectorOnVoidSkim();
        }
        #endregion

        #region Selector  =====================================================
        private void TestSelectorTap()
        {
            if (Selector.IsVoidHit)
                SelectorOnVoidTapped();
            else
                AugmentDrawState(DrawState.TapStart, DrawState.EventMask);
        }
        private void TestSelectorCtrlTap()
        {
            if (Selector.IsVoidHit)
                SelectorOnVoidTapped();
            else
                AugmentDrawState(DrawState.CtrlTapStart, DrawState.EventMask);
        }
        private void TestSelectorTapEnd()
        {
            if (_tracingSelector)
                SelectorOnVoidEnding();
            else
                AugmentDrawState(DrawState.Ending, DrawState.EventMask);
        }

        private void SelectorOnVoidSkim()
        {
            DrawCursor = DrawCursor.Arrow;
            HideDrawItems(DrawItem.ToolTip);
            if (ModelDelta != Graph.ModelDelta)
            {
                ModelDelta = Graph.ModelDelta;
                Graph.RebuildHitTestMap();
                Selector.UpdateModels();
            }
            var mode = DrawState & DrawState.ModeMask;
            switch (mode)
            {
                case DrawState.ViewMode: DrawCursor = DrawCursor.Arrow; break;
                case DrawState.EditMode: DrawCursor = DrawCursor.Edit; break;
                case DrawState.MoveMode: DrawCursor = DrawCursor.Move; break;
                case DrawState.LinkMode: DrawCursor = DrawCursor.Link; break;
                case DrawState.PinsMode: DrawCursor = DrawCursor.Arrow; break;
                case DrawState.CopyMode: DrawCursor = DrawCursor.Copy; break;
                case DrawState.UnlinkMode: DrawCursor = DrawCursor.UnLink; break;
                case DrawState.CreateMode: DrawCursor = DrawCursor.New; break;
                case DrawState.DeleteMode: DrawCursor = DrawCursor.Delete; break;
                case DrawState.GravityMode: DrawCursor = DrawCursor.Gravity; break;
                case DrawState.OperateMode: DrawCursor = DrawCursor.Operate; break;
            }
            AugmentDrawState(DrawState.NowOnVoid, DrawState.NowMask | DrawState.EventMask);
            PageModel.TriggerUIRefresh();
        }
        private void SelectorOnVoidTapped()
        {
            _tracingSelector = true;
            _selectToggleMode = false;
            Selector.Clear();
            DrawCursor = DrawCursor.Arrow;
            HideDrawItems(DrawItem.ToolTip | DrawItem.FlyTree);
            ShowDrawItems(DrawItem.Selector);
            PageModel.TriggerUIRefresh();
        }
        private void SelectorOnVoidCtrlTapped()
        {
            _tracingSelector = true;
            _selectToggleMode = true;
            DrawCursor = DrawCursor.Arrow;
            HideDrawItems(DrawItem.ToolTip | DrawItem.FlyTree);
            ShowDrawItems(DrawItem.Selector);
            PageModel.TriggerUIRefresh();
        }
        private void SelectorOnVoidEnding()
        {
            if (_tracingSelector)
            {
                _tracingSelector = false;
                Selector.HitTestRegion(_selectToggleMode, Editor.Point1, Editor.Point2);
            }
            HideDrawItems(DrawItem.Selector);
            Refresh();
        }
        private bool _tracingSelector;
        private bool _selectToggleMode;
        #endregion

        #region EditMode  =====================================================
        private void EditOnNode()
        {
            ToolTip_Text1 = Selector.HitNode.GetNameId();
            ToolTip_Text2 = Selector.HitNode.GetSummaryId();
            FlyOutPoint = Selector.HitNode.Center;
            DrawCursor = DrawCursor.Hand;
            ShowDrawItems(DrawItem.ToolTip);
            PageModel.TriggerUIRefresh();
        }
        private void EditOnNodeTapped()
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
        private void EditOnNodeEnding()
        {
            DrawCursor = DrawCursor.Hand;
            PageModel.TriggerUIRefresh();
        }
        #endregion

        #region MoveMode  =====================================================
        private void MoveOnNode()
        {
            ToolTip_Text1 = Selector.HitNode.GetNameId();
            ToolTip_Text2 = Selector.HitNode.GetSummaryId();
            FlyOutPoint = Selector.HitNode.Center;
            DrawCursor = DrawCursor.Hand;
            ShowDrawItems(DrawItem.ToolTip);
            PageModel.TriggerUIRefresh();
        }
        private void MoveOnNodeTapped()
        {
            Selector.SaveHitReference();
            HideDrawItems(DrawItem.ToolTip | DrawItem.FlyTree);
            DrawCursor = DrawCursor.SizeAll;
            Refresh();
        }
        private void MoveOnNodeDragging()
        {
            var v = Editor.PointDelta(true);
            Selector.Move(v);
            UpdateEditorData();
        }
        private void MoveOnNodeUpArrow() => MoveOnNodeDelta(Selector.HitNode, new Vector2(0, -1));
        private void MoveOnNodeDownArrow() => MoveOnNodeDelta(Selector.HitNode, new Vector2(0, 1));
        private void MoveOnNodeLeftArrow() => MoveOnNodeDelta(Selector.HitNode, new Vector2(-1, 0));
        private void MoveOnNodeRightArrow() => MoveOnNodeDelta(Selector.HitNode, new Vector2(1, 0));
        private void MoveOnNodeDelta(Node node, Vector2 delta)
        {
            Selector.Move(delta);
            UpdateEditorData();
            PageModel.TriggerUIRefresh();
        }
        private void MoveOnNodeEnding()
        {
            DrawCursor = DrawCursor.Hand;
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

