
using System.Numerics;

namespace ModelGraph.Core
{
    public class GraphModel : CanvasModel, IDataModel
    {
        internal override Item RootItem => _graph;
        private readonly Graph _graph;
        internal GraphModel(Root root, Graph graph) : base (root, ControlType.GraphDisplay)
        {
            _graph = graph;
            RefreshCanvasDrawData();
        }

        #region RequiredMethods  ==============================================
        public override bool MoveNode()
        {
            return false;
        }

        public override bool MoveRegion()
        {
            return false;
        }

        public override bool CreateNode()
        {
            return false;
        }

        public override bool TapHitTest()
        {
            return false;
        }

        public override bool EndHitTest()
        {
            return false;
        }

        public override bool SkimHitTest()
        {
            return false;
        }

        public override bool DragHitTest()
        {
            return false;
        }

        public override bool RegionNodeHitTest()
        {
            return false;
        }

        public override void ShowPropertyPanel()
        {
        }

        public override void HidePropertyPanel()
        {
        }

        public override void ResizeTop()
        {
        }

        public override void ResizeLeft()
        {
        }

        public override void ResizeRight()
        {
        }

        public override void ResizeBottom()
        {
        }

        public override void ResizeTopLeft()
        {
        }

        public override void ResizeTopRight()
        {
        }

        public override void ResizeBottomLeft()
        {
        }

        public override void ResizeBottomRight()
        {
        }

        public override void ResizePropagate()
        {
        }

        public override void RefreshCanvasDrawData()
        {
            _drawLines.Clear();
            foreach (var e in _graph.Edges)
            {
                var p1 = new Vector2(e.Node1.X, e.Node1.Y);
                var p2 = new Vector2(e.Node2.X, e.Node2.Y);
                var p = new Vector2[] { p1, p2 };
                _drawLines.Add((p, false, 2, (255, 0, 255, 0)));
            }
            _fillCircles.Clear();
            foreach (var n in _graph.Nodes)
            {
                _fillCircles.Add(((n.X, n.Y, 6), (255, 255, 205, 80)));
            }
        }
        #endregion
    }
}
