
namespace ModelGraph.Core
{
    public class GraphModel : CanvasModel, IDataModel
    {
        internal override Item RootItem => _graph;
        private readonly Graph _graph;
        internal GraphModel(Root root, Graph graph) : base (root, ControlType.GraphDisplay)
        {
            _graph = graph;
            CreateTestData();
        }

        void CreateTestData()
        {
            _drawRects.Add(((10, 10, 100, 100), (128, 255, 255, 0)));
            _drawRects.Add(((50, 50, 200, 300), (200, 155, 205, 80)));
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
        }
        #endregion
    }
}
