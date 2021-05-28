using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public class GraphModel : DrawModel
    {
        internal readonly Graph Graph;
        internal readonly GraphSelector Selector;

        #region DrawMode, DrawState  ==========================================
        // Name the drawing modes and states for this model
        public override List<IdKey> GetModeIdKeys() => new List<IdKey>()
        {
            IdKey.ViewMode,
            IdKey.EditMode,
            IdKey.MoveMode,
            IdKey.CopyMode,
            IdKey.PasteMode,
            IdKey.LinkMode,
            IdKey.UnlinkMode,
            IdKey.DeleteMode,
            IdKey.ReshapeMode,
            IdKey.GravityMode,
            IdKey.OperateMode,
        };
        private enum DrawMode : byte
        {
            View,
            Edit,
            Move,
            Copy,
            Paste,
            Link,
            UnLink,
            Delete,
            Reshape,
            Gravity,
            Operate,
        }
        private enum DrawState : byte
        {
            OnVoid, //pointer is on an empty space on the drawing

            OnNode,
            OnNodeEastGrip,
            OnNodeWestGrip,
            OnNodeNorthGrip,
            OnNodeSouthGrip,
            OnNodeNorthEastGrip,
            OnNodeNorthWestGrip,
            OnNodeSouthEastGrip,
            OnNodeSouthWestGrip,

            OnEdge,
            OnEdgeSide1,
            OnEdgeSide2,
            OnEdgeTerm1,
            OnEdgeTerm2,
            OnEdgePoint,
            OnEdgeMidpoint,  
        }
        #endregion

        #region SetModeStateActions  ==========================================
        private void SetModeStateEventActions()
        {
            SetModeStateEventAction((byte)DrawMode.View, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Edit, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Move, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Copy, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Paste, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Link, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.UnLink, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Delete, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Reshape, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Gravity, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);
            SetModeStateEventAction((byte)DrawMode.Operate, (byte)DrawState.OnVoid, DrawEvent.Pseudo, PageModel.TriggerUIRefresh);


            SetModeStateEventAction((byte)DrawMode.View, (byte)DrawState.OnVoid, DrawEvent.Skim, ViewSkim);
            SetModeStateEventAction((byte)DrawMode.View, (byte)DrawState.OnNode, DrawEvent.Skim, ViewSkim);
            SetModeStateEventAction((byte)DrawMode.View, (byte)DrawState.OnEdge, DrawEvent.Skim, ViewSkim);

            SetModeStateEventAction((byte)DrawMode.View, (byte)DrawState.OnVoid, DrawEvent.Tap, SelectorOnVoidTap);
            SetModeStateEventAction((byte)DrawMode.View, (byte)DrawState.OnVoid, DrawEvent.TapEnd, SelectorOnVoidTapEnd);
            SetModeStateEventAction((byte)DrawMode.View, (byte)DrawState.OnVoid, DrawEvent.CtrlTap, SelectorOnVoidCtrlTap);

            SetModeStateEventAction((byte)DrawMode.Edit, (byte)DrawState.OnVoid, DrawEvent.Skim, ViewSkim);
            SetModeStateEventAction((byte)DrawMode.Edit, (byte)DrawState.OnNode, DrawEvent.Skim, ViewSkim);
            SetModeStateEventAction((byte)DrawMode.Edit, (byte)DrawState.OnEdge, DrawEvent.Skim, ViewSkim);

            SetModeStateEventAction((byte)DrawMode.Edit, (byte)DrawState.OnVoid, DrawEvent.Tap, SelectorOnVoidTap);
            SetModeStateEventAction((byte)DrawMode.Edit, (byte)DrawState.OnVoid, DrawEvent.TapEnd, SelectorOnVoidTapEnd);
            SetModeStateEventAction((byte)DrawMode.Edit, (byte)DrawState.OnVoid, DrawEvent.CtrlTap, SelectorOnVoidCtrlTap);

            SetModeStateEventAction((byte)DrawMode.Edit, (byte)DrawState.OnNode, DrawEvent.Tap, EditOnNodeTap);

            SetModeStateEventAction((byte)DrawMode.Move, (byte)DrawState.OnVoid, DrawEvent.Skim, MoveSkim);
            SetModeStateEventAction((byte)DrawMode.Move, (byte)DrawState.OnNode, DrawEvent.Skim, MoveSkim);
            SetModeStateEventAction((byte)DrawMode.Move, (byte)DrawState.OnEdge, DrawEvent.Skim, MoveSkim);

            SetModeStateEventAction((byte)DrawMode.Move, (byte)DrawState.OnVoid, DrawEvent.Tap, SelectorOnVoidTap);
            SetModeStateEventAction((byte)DrawMode.Move, (byte)DrawState.OnVoid, DrawEvent.TapEnd, SelectorOnVoidTapEnd);
            SetModeStateEventAction((byte)DrawMode.Move, (byte)DrawState.OnVoid, DrawEvent.CtrlTap, SelectorOnVoidCtrlTap);

            SetModeStateEventAction((byte)DrawMode.Move, (byte)DrawState.OnNode, DrawEvent.UpArrowKey, MoveOnNodeUpArrow);
            SetModeStateEventAction((byte)DrawMode.Move, (byte)DrawState.OnNode, DrawEvent.DownArrowKey, MoveOnNodeDownArrow);
            SetModeStateEventAction((byte)DrawMode.Move, (byte)DrawState.OnNode, DrawEvent.LeftArrowKey, MoveOnNodeLeftArrow);
            SetModeStateEventAction((byte)DrawMode.Move, (byte)DrawState.OnNode, DrawEvent.RightArrowKey, MoveOnNodeRightArrow);

            SetModeStateEventAction((byte)DrawMode.Move, (byte)DrawState.OnNode, DrawEvent.Tap, MoveOnNodeTap);
            SetModeStateEventAction((byte)DrawMode.Move, (byte)DrawState.OnNode, DrawEvent.Drag, MoveOnNodeDrag);

            SetModeStateEventAction((byte)DrawMode.Link, (byte)DrawState.OnVoid, DrawEvent.Skim, LinkSkim);
            SetModeStateEventAction((byte)DrawMode.Link, (byte)DrawState.OnNode, DrawEvent.Skim, LinkSkim);
            SetModeStateEventAction((byte)DrawMode.Link, (byte)DrawState.OnEdge, DrawEvent.Skim, LinkSkim);


            SetModeStateEventAction((byte)DrawMode.UnLink, (byte)DrawState.OnVoid, DrawEvent.Skim, UnLinkSkim);
            SetModeStateEventAction((byte)DrawMode.UnLink, (byte)DrawState.OnNode, DrawEvent.Skim, UnLinkSkim);
            SetModeStateEventAction((byte)DrawMode.UnLink, (byte)DrawState.OnEdge, DrawEvent.Skim, UnLinkSkim);
        }
        #endregion

        #region SetModeStateCursors  ==========================================
        private void SetModeStateCursors()
        {
            SetModeStateCursor((byte)DrawMode.Edit, (byte)DrawState.OnVoid, DrawCursor.Aim);
            SetModeStateCursor((byte)DrawMode.Move, (byte)DrawState.OnVoid, DrawCursor.Aim);
            SetModeStateCursor((byte)DrawMode.Copy, (byte)DrawState.OnVoid, DrawCursor.Aim);
            SetModeStateCursor((byte)DrawMode.Paste, (byte)DrawState.OnVoid, DrawCursor.New);
            SetModeStateCursor((byte)DrawMode.Link, (byte)DrawState.OnVoid, DrawCursor.Aim);
            SetModeStateCursor((byte)DrawMode.UnLink, (byte)DrawState.OnVoid, DrawCursor.Aim);
            SetModeStateCursor((byte)DrawMode.Delete, (byte)DrawState.OnNode, DrawCursor.Aim);
            SetModeStateCursor((byte)DrawMode.Reshape, (byte)DrawState.OnVoid, DrawCursor.Aim);
            SetModeStateCursor((byte)DrawMode.Gravity, (byte)DrawState.OnVoid, DrawCursor.Aim);
            SetModeStateCursor((byte)DrawMode.Operate, (byte)DrawState.OnVoid, DrawCursor.Aim);

            SetModeStateCursor((byte)DrawMode.Edit, (byte)DrawState.OnNode, DrawCursor.Edit);
            SetModeStateCursor((byte)DrawMode.Edit, (byte)DrawState.OnEdge, DrawCursor.Edit);
            SetModeStateCursor((byte)DrawMode.Move, (byte)DrawState.OnNode, DrawCursor.Move);
            SetModeStateCursor((byte)DrawMode.Copy, (byte)DrawState.OnNode, DrawCursor.Copy);
            SetModeStateCursor((byte)DrawMode.Link, (byte)DrawState.OnNode, DrawCursor.Link);
            SetModeStateCursor((byte)DrawMode.UnLink, (byte)DrawState.OnNode, DrawCursor.UnLink);
            SetModeStateCursor((byte)DrawMode.UnLink, (byte)DrawState.OnEdge, DrawCursor.UnLink);
            SetModeStateCursor((byte)DrawMode.Delete, (byte)DrawState.OnNode, DrawCursor.Delete);
            SetModeStateCursor((byte)DrawMode.Reshape, (byte)DrawState.OnNode, DrawCursor.Reshape);
            SetModeStateCursor((byte)DrawMode.Gravity, (byte)DrawState.OnNode, DrawCursor.Gravity);
            SetModeStateCursor((byte)DrawMode.Gravity, (byte)DrawState.OnEdge, DrawCursor.Gravity);
            SetModeStateCursor((byte)DrawMode.Operate, (byte)DrawState.OnNode, DrawCursor.Operate);
        }
        #endregion

        #region Constructor  ==================================================
        internal GraphModel(PageModel owner, Graph graph) : base(owner)
        {
            Graph = graph;
            Selector = new GraphSelector(graph);

            EditorWidth = 300;

            Overview = Editor; // enable overview
            OverviewWidth = 100; // inital width

            SetModeNames(typeof(DrawMode));
            SetStateNames(typeof(DrawState));
            SetModeStateEventActions();
            SetModeStateCursors();
            ShowDrawItems(DrawItem.Overview);

            FlyTreeModel = new TreeModel(owner, null);
            Editor.GetExtent = graph.ResetExtent;

            HasUndoRedo = true;

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
            if ((VisibleDrawItems & DrawItem.FlyTree) != 0) FlyTreeDelta++;
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
            var y = Picker2Data.Point1.Y;
            var w = Picker2Data.Extent.Width;
            var x = w / 2;
            var z = (y / w) * w + x;
            Picker2.Clear();
            Picker2.AddParms((new Vector2[] { new Vector2(x, z), new Vector2(x, x) }, (ShapeType.CenterRect, StrokeType.Filled, 0), (63, 255, 255, 255)));
        }
        #endregion

        #region Selector  =====================================================
        private void SelectorClear()
        {
            if (Selector.IsVoidHit)
            {
                _tracingSelector = false;
                _selectToggleMode = false;
                Selector.Clear();
                HideDrawItems(DrawItem.Selector | DrawItem.ToolTip | DrawItem.FlyTree);
            }
        }
        private void SelectorOnVoidTap()
        {
            _tracingSelector = true;
            _selectToggleMode = false;
            Selector.Clear();
            HideDrawItems(DrawItem.ToolTip | DrawItem.FlyTree);
            ShowDrawItems(DrawItem.Selector);
            PageModel.TriggerUIRefresh();
        }
        private void SelectorOnVoidCtrlTap()
        {
            _tracingSelector = true;
            _selectToggleMode = true;
            HideDrawItems(DrawItem.ToolTip | DrawItem.FlyTree);
            ShowDrawItems(DrawItem.Selector);
            PageModel.TriggerUIRefresh();
        }
        private void SelectorOnVoidTapEnd()
        {
            if (_tracingSelector)
            {
                _tracingSelector = false;
                Selector.HitTestRegion(_selectToggleMode, EditData.Point1, EditData.Point2);
            }
            HideDrawItems(DrawItem.Selector);
            UpdateEditorData();
        }
        private bool _tracingSelector;
        private bool _selectToggleMode;
        #endregion

        #region ViewMode  =====================================================
        private void ViewSkim()
        {
            Selector.HitTestPoint(EditData.Point2);
            if (Selector.IsVoidHit)
            {
                HideDrawItems(DrawItem.ToolTip);
                if (ModelDelta != Graph.ModelDelta)
                {
                    ModelDelta = Graph.ModelDelta;
                    Graph.RebuildHitTestMap();
                    Selector.UpdateModels();
                }
                SetDrawState((byte)DrawState.OnVoid);
            }
            else if (Selector.IsNodeHit && Selector.HitNode != Selector.PrevNode)
            {
                ToolTip_Text1 = Selector.HitNode.GetNameId();
                ToolTip_Text2 = Selector.HitNode.GetSummaryId();
                FlyOutPoint = Selector.HitNode.Center;
                ShowDrawItems(DrawItem.ToolTip);
                ToolTipChanged();
                SetDrawState((byte)DrawState.OnNode);
                PageModel.TriggerUIRefresh();

            }
            else if (Selector.IsEdgeHit && Selector.HitEdge != Selector.PrevEdge)
            {
                ToolTip_Text1 = Selector.HitEdge.GetNameId();
                ToolTip_Text2 = Selector.HitEdge.GetSummaryId();
                FlyOutPoint = EditData.Point2;
                ShowDrawItems(DrawItem.ToolTip);
                ToolTipChanged();
                SetDrawState((byte)DrawState.OnEdge);
                PageModel.TriggerUIRefresh();
            }
        }
        #endregion

        #region EditMode  =====================================================
        private void EditOnNodeTap()
        {
            FlyTreeDelta++;
            HideDrawItems(DrawItem.ToolTip);
            if (FlyTreeModel is TreeModel tm)
            {
                Selector.SaveHitReference();
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
        }
        #endregion

        #region MoveMode  =====================================================
        private void MoveSkim()
        {
            Selector.HitTestPoint(EditData.Point2);
            if (Selector.IsVoidHit)
            {
                HideDrawItems(DrawItem.ToolTip);
                if (ModelDelta != Graph.ModelDelta)
                {
                    ModelDelta = Graph.ModelDelta;
                    Graph.RebuildHitTestMap();
                    Selector.UpdateModels();
                }
                SetDrawState((byte)DrawState.OnVoid);
            }
            else if (Selector.IsNodeHit && Selector.HitNode != Selector.PrevNode)
            {
                ToolTip_Text1 = Selector.HitNode.GetNameId();
                ToolTip_Text2 = Selector.HitNode.GetSummaryId();
                FlyOutPoint = Selector.HitNode.Center;
                ShowDrawItems(DrawItem.ToolTip);
                ToolTipChanged();
                SetDrawState((byte)DrawState.OnNode);
                PageModel.TriggerUIRefresh();

            }
        }
        private void MoveOnNodeTap()
        {
            Selector.SaveHitReference();
            HideDrawItems(DrawItem.ToolTip | DrawItem.FlyTree);
            Refresh();
        }
        private void MoveOnNodeDrag()
        {
            var v = EditData.PointDelta(true);
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
        }
        #endregion

        #region LinkMode  =====================================================
        private void LinkSkim()
        {
            Selector.HitTestPoint(EditData.Point2);
            if (Selector.IsVoidHit)
            {
                HideDrawItems(DrawItem.ToolTip);
                if (ModelDelta != Graph.ModelDelta)
                {
                    ModelDelta = Graph.ModelDelta;
                    Graph.RebuildHitTestMap();
                    Selector.UpdateModels();
                }
                SetDrawState((byte)DrawState.OnVoid);
            }
            else if (Selector.IsNodeHit && Selector.HitNode != Selector.PrevNode)
            {
                ToolTip_Text1 = Selector.HitNode.GetNameId();
                ToolTip_Text2 = Selector.HitNode.GetSummaryId();
                FlyOutPoint = Selector.HitNode.Center;
                ShowDrawItems(DrawItem.ToolTip);
                ToolTipChanged();
                SetDrawState((byte)DrawState.OnNode);
                PageModel.TriggerUIRefresh();

            }
            else if (Selector.IsEdgeHit && Selector.HitEdge != Selector.PrevEdge)
            {
                ToolTip_Text1 = Selector.HitEdge.GetNameId();
                ToolTip_Text2 = Selector.HitEdge.GetSummaryId();
                FlyOutPoint = EditData.Point2;
                ShowDrawItems(DrawItem.ToolTip);
                ToolTipChanged();
                SetDrawState((byte)DrawState.OnEdge);
                PageModel.TriggerUIRefresh();
            }
        }
        #endregion

        #region UnLinkMode  ===================================================
        private void UnLinkSkim()
        {
            Selector.HitTestPoint(EditData.Point2);
            if (Selector.IsVoidHit)
            {
                HideDrawItems(DrawItem.ToolTip);
                if (ModelDelta != Graph.ModelDelta)
                {
                    ModelDelta = Graph.ModelDelta;
                    Graph.RebuildHitTestMap();
                    Selector.UpdateModels();
                }
                SetDrawState((byte)DrawState.OnVoid);
            }
            else if (Selector.IsNodeHit && Selector.HitNode != Selector.PrevNode)
            {
                ToolTip_Text1 = Selector.HitNode.GetNameId();
                ToolTip_Text2 = Selector.HitNode.GetSummaryId();
                FlyOutPoint = Selector.HitNode.Center;
                ShowDrawItems(DrawItem.ToolTip);
                ToolTipChanged();
                SetDrawState((byte)DrawState.OnNode);
                PageModel.TriggerUIRefresh();

            }
            else if (Selector.IsEdgeHit && Selector.HitEdge != Selector.PrevEdge)
            {
                ToolTip_Text1 = Selector.HitEdge.GetNameId();
                ToolTip_Text2 = Selector.HitEdge.GetSummaryId();
                FlyOutPoint = EditData.Point2;
                ShowDrawItems(DrawItem.ToolTip);
                ToolTipChanged();
                SetDrawState((byte)DrawState.OnEdge);
                PageModel.TriggerUIRefresh();
            }
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

