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
            IdKey.EditMode
        };
        private enum DrawMode : byte
        {
            View = 0,
            Edit = 1,
        }
        private enum DrawState : byte
        {
            Idle = 0,
        }
        #endregion

        #region InitModeStateActions  =========================================
        private void InitModeStateEventActions()
        {
            DefineModeStateEventAction((byte)DrawMode.View, (byte)DrawState.Idle, DrawEvent.ExecuteAction, PageModel.TriggerUIRefresh);
            DefineModeStateEventAction((byte)DrawMode.Edit, (byte)DrawState.Idle, DrawEvent.ExecuteAction, PageModel.TriggerUIRefresh);

        }
        #endregion

        #region Constructor  ==================================================
        internal GraphModel(PageModel owner, Graph graph) : base(owner)
        {
            Graph = graph;
            Selector = new GraphSelector(graph);

            InitModeNames(typeof(DrawMode));
            InitStateNames(typeof(DrawState));
            InitModeStateEventActions();
            ShowDrawItems(DrawItem.Overview);

            FlyTreeModel = new TreeModel(owner, null);
            Editor.GetExtent = graph.ResetExtent;

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
            if (Selector.IsVoidHit)
            {
                HideDrawItems(DrawItem.ToolTip);
                if (ModelDelta != Graph.ModelDelta)
                {
                    ModelDelta = Graph.ModelDelta;
                    Graph.RebuildHitTestMap();
                    Selector.UpdateModels();
                }
                //ModifyDrawState(DrawState.NowOnVoid, DrawState.NowMask | DrawState.EventMask); //need both masks!
            }
            else if (Selector.IsNodeHit && Selector.HitNode != Selector.PrevNode)
            {
                ToolTip_Text1 = Selector.HitNode.GetNameId();
                ToolTip_Text2 = Selector.HitNode.GetSummaryId();
                FlyOutPoint = Selector.HitNode.Center;
                ShowDrawItems(DrawItem.ToolTip);
                ToolTipChanged();
                //ModifyDrawState(DrawState.NowOnNode, DrawState.NowMask | DrawState.EventMask); //need both masks!
                PageModel.TriggerUIRefresh();

            }
            else if (Selector.IsEdgeHit && Selector.HitEdge != Selector.PrevEdge)
            {
                ToolTip_Text1 = Selector.HitEdge.GetNameId();
                ToolTip_Text2 = Selector.HitEdge.GetSummaryId();
                FlyOutPoint = Editor.Point2;
                ShowDrawItems(DrawItem.ToolTip);
                ToolTipChanged();
                //ModifyDrawState(DrawState.NowOnEdge, DrawState.NowMask | DrawState.EventMask); //need both masks!                
                PageModel.TriggerUIRefresh();
            }
        }
        #endregion

        #region Selector  =====================================================
        private bool HasSelector => false; // (DrawState & DrawState.HasSelector) != 0;
        private void TestSelectorTap()
        {
            if (Selector.IsVoidHit && HasSelector)
                SelectorOnVoidTapped();
            else
            {
                //ModifyDrawState(DrawState.Tapped, DrawState.EventMask);
                SelectorClear();
            }
        }
        private void TestSelectorCtrlTap()
        {
            if (Selector.IsVoidHit && HasSelector)
                SelectorOnVoidCtrlTapped();
            else
            {
                //ModifyDrawState(DrawState.CtrlTapped, DrawState.EventMask);
                SelectorClear();
            }
        }
        private void TestSelectorTapEnd()
        {
            if (_tracingSelector)
                SelectorOnVoidEnding();
            //else
                //ModifyDrawState(DrawState.TapDragEnd, DrawState.EventMask);
        }
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
        private void SelectorOnVoidTapped()
        {
            _tracingSelector = true;
            _selectToggleMode = false;
            Selector.Clear();
            HideDrawItems(DrawItem.ToolTip | DrawItem.FlyTree);
            ShowDrawItems(DrawItem.Selector);
        }
        private void SelectorOnVoidCtrlTapped()
        {
            _tracingSelector = true;
            _selectToggleMode = true;
            HideDrawItems(DrawItem.ToolTip | DrawItem.FlyTree);
            ShowDrawItems(DrawItem.Selector);
        }
        private void SelectorOnVoidEnding()
        {
            if (_tracingSelector)
            {
                _tracingSelector = false;
                Selector.HitTestRegion(_selectToggleMode, Editor.Point1, Editor.Point2);
            }
            HideDrawItems(DrawItem.Selector);
            UpdateEditorData();
        }
        private bool _tracingSelector;
        private bool _selectToggleMode;
        #endregion

        #region EditMode  =====================================================
        private void EditOnNodeTapped()
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
        private void EditOnVoidTapped()
        {
            HideDrawItems(DrawItem.ToolTip | DrawItem.FlyTree);
        }
        #endregion

        #region MoveMode  =====================================================
        private void MoveOnNodeTapped()
        {
            Selector.SaveHitReference();
            HideDrawItems(DrawItem.ToolTip | DrawItem.FlyTree);
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

