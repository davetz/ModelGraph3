using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class GraphCanvas : CanvasModel, ICanvasModel
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
            Editor.Clear();
            foreach (var n in _graph.Nodes)
            {
                var c = new Vector2(n.X, n.Y);
                var r = n.Radius;
                Editor.AddCircle(((c, r), (Stroke.IsFilled, 1), (255, 255, 0, 255)));
            }
            foreach (var e in _graph.Edges)
            {
                var p1 = new Vector2(e.Node1.X, e.Node1.Y);
                var p2 = new Vector2(e.Node2.X, e.Node2.Y);
                var v = new Vector2[] { p1, p2 };
                Editor.AddLine((v, (Stroke.IsSimple, 2), (255, 0, 255, 255)));
            }
        }
        #endregion

        #region PickerEvents  =================================================
        public void Picker1Select()
        {
        }
        public void Picker2Select()
        {
        }
        public void Picker2Create()
        {
        }
        #endregion


        #region HitTest  ======================================================
        public Extent EditorExtent => _graph.ResetExtent();
        public float Picker1Width => 0;
        public float Picker2Width => 0;

        public bool DragHitTest()
        {
            return false;
        }
        public bool EndHitTest()
        {
            return false;
        }
        public bool SkimHitTest()
        {
            ClearHit();

            if (RegionHitTest(DrawPoint2))
            {
                SetHitRegion();
            }

            _hitNode = null;
            var (ok, node) = HitNodeTest(DrawPoint2);
            if (ok)
            {
                _hitNode = node;
                SetHitNode();
                ToolTip_Text1 = node.GetNameId();
                ToolTip_Text2 = node.GetSummaryId();
            }
            return NodeHit || RegionHit;
        }

        public bool TapHitTest()
        {
            ClearHit();

            if (RegionHitTest(DrawPoint1))
            {
                SetHitRegion();
            }

            var (ok, node) = HitNodeTest(DrawPoint1);
            if (ok)
            {
                _hitNode = node;
                SetHitNode();
            }
            return ok;
        }

        public void ClearRegion() => _regionNodes.Clear();
        public bool IsValidRegion()
        {
            var r1 = RegionPoint1;
            var r2 = RegionPoint2;
            _regionNodes.Clear();
            foreach (var n in _graph.Nodes)
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

        private (bool,Node) HitNodeTest(Vector2 p)
        {
            foreach (var n in _graph.Nodes)
            {
                if (n.HitTest((p.X, p.Y))) return (true, n);
            }
            return (false, null);
        }
        private Node _hitNode;
        private Edge _hitEdge;
        private List<Node> _regionNodes = new List<Node>();
        #endregion

        #region Move  =========================================================
        public bool MoveNode()
        {
            if (_hitNode is null) return false;
            var delta = DrawPointDelta(true);
            _hitNode.X += delta.X;
            _hitNode.Y += delta.Y;
            return true;
        }

        public bool MoveRegion()
        {
            var delta = DrawPointDelta(true);
            foreach (var n  in _regionNodes)
            {
                n.X += delta.X;
                n.Y += delta.Y;
            }
            return true;
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
