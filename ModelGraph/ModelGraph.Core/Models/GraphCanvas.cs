using System;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class GraphCanvas : DrawCanvas, IDrawCanvas
    {
        private readonly GraphModel _model;
        private readonly Graph _graph;

        #region Constructor  ==================================================
        internal GraphCanvas(GraphModel model, Graph graph)
        {
            _model = model;
            _graph = graph;

            RefreshDrawData();
        }
        #endregion

        #region CreateDrawData  ===============================================
        public void RefreshDrawData()
        {
            ClearDrawData();
            foreach (var n in _graph.Nodes)
            {
                var c = new Vector2(n.X, n.Y);
                var r = n.Radius;
                _drawCircles.Add(((c, r), (Stroke.IsFilled, 1), (255, 255, 0, 255)));
            }
            foreach (var e in _graph.Edges)
            {
                var p1 = new Vector2(e.Node1.X, e.Node1.Y);
                var p2 = new Vector2(e.Node2.X, e.Node2.Y);
                var v = new Vector2[] { p1, p2 };
                _drawLines.Add((v, (Stroke.IsSimple, 2), (255, 0, 255, 255)));
            }
        }
        #endregion


        #region HitTest  ======================================================
        public Extent DrawingExtent => _graph.ResetExtent();


        public bool DragHitTest()
        {
            return false;
        }
        public bool EndHitTest()
        {
            return false;
        }
        public bool RegionNodeHitTest()
        {
            return false;
        }
        public bool SkimHitTest()
        {
            _skimHitNode = null;
            Hit = HitType.Node;
            ToolTip_Text1 = string.Empty;
            var (ok, node) = HitNodeTest();
            if (ok)
            {
                _skimHitNode = node;
                ToolTip_Text1 = node.GetNameId();
            }
            return ok;
        }

        public bool TapHitTest()
        {
            _tapHitNode = null;
            Hit = HitType.Node;
            var (ok, node) = HitNodeTest();
            if (ok)
            {
                _tapHitNode = node;
            }
            return ok;
        }

        private (bool,Node) HitNodeTest()
        {
            var p = DrawPoint1;
            foreach (var n in _graph.Nodes)
            {
                if (n.HitTest((p.X, p.Y))) return (true, n);
            }
            return (false, null);
        }
        private Node _tapHitNode;
        private Node _skimHitNode;
        #endregion

        #region Move  =========================================================
        public bool MoveNode()
        {
            if (_tapHitNode is null) return false;
            var delta = DrawPointDelta(true);
            _tapHitNode.X += delta.X;
            _tapHitNode.Y += delta.Y;
            return true;
        }

        public bool MoveRegion()
        {
            return false;
        }
        #endregion

        #region Resize  =======================================================
        public void ResizeBottom()
        {
        }

        public void ResizeBottomLeft()
        {
        }

        public void ResizeBottomRight()
        {
        }

        public void ResizeLeft()
        {
        }

        public void ResizePropagate()
        {
        }

        public void ResizeRight()
        {
        }

        public void ResizeTop()
        {
        }

        public void ResizeTopLeft()
        {
        }

        public void ResizeTopRight()
        {
        }
        #endregion

        #region CreateNode  ===================================================
        public bool CreateNode()
        {
            return false;
        }
        public void HidePropertyPanel()
        {
        }
        public void ShowPropertyPanel()
        {
        }
        #endregion
    }
}
