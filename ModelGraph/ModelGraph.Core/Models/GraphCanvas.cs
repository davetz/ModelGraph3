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
        }
        #endregion

        #region HitTest  ======================================================
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
            return false;
        }

        public bool TapHitTest()
        {
            return false;
        }
        #endregion

        #region Move  =========================================================
        public bool MoveNode()
        {
            return false;
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

        #region PanZoom  ======================================================
        private const float maxScale = 2;
        private const float minZoomDiagonal = 8000;

        public void Pan(Vector2 adder)
        {
        }
        public void Zoom(float changeFactor)
        {
        }
        public void ZoomToExtent()
        {
        }
        public void PanZoomReset(float aw, float ah)
        {

            var e = _graph.ResetExtent();
            var ew = (float)e.Width;
            var eh = (float)e.Hieght;

            if (aw < 1) aw = 1;
            if (ah < 1) ah = 1;
            if (ew < 1) ew = 1;
            if (eh < 1) eh = 1;

            var zw = aw / ew;
            var zh = ah / eh;
            var z = (zw < zh) ? zw : zh;

            // zoom required to make the view extent fit the canvas
            if (z > maxScale) z = maxScale;
            _scale = z;

            var ec = new Vector2(e.CenterX, e.CenterY) * z; //center point of scaled view extent
            var ac = new Vector2(aw / 2, ah / 2); //center point of the canvas
            _offset = ac - ec; //complete offset need to center the view extent on the canvas

            RefreshDrawData();
        }
        public void RefreshDrawData()
        {
            ClearDrawData();
            foreach (var n in _graph.Nodes)
            {
                var c = new Vector2(n.X, n.Y);
                var r = n.Radius * _scale;
                Vector2 p = c * _scale + _offset;
                _drawCircles.Add(((p.X, p.Y, r), Stroke.IsFilled, 0, (255, 255, 0, 255)));
            }
            foreach (var e in _graph.Edges)
            {
                var c1 = new Vector2(e.Node1.X, e.Node1.Y);
                var c2 = new Vector2(e.Node2.X, e.Node2.Y);
                var p1 = c1 * _scale + _offset;
                var p2 = c2 * _scale + _offset;
                var v = new Vector2[] { p1, p2 };
                _drawLines.Add((v, Stroke.IsSimple, 2, (255, 0, 255, 255)));
            }
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
