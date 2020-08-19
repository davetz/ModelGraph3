using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGraph.Core
{
    public class GraphModel : CanvasModel, IDataModel
    {
        internal GraphModel(Root root, Graph graph, ControlType ctlType)
        {
            Owner = root;
            RootItem = graph;
            ControlType = ctlType;

            root.Add(this);

            CreateTestData();
        }
        internal Root Owner;
        internal override Item GetOwner() => Owner;

        void CreateTestData()
        {
            _drawRects.Add((new Extent(10, 10, 100, 100), (128, 255, 255, 0)));
            _drawRects.Add((new Extent(50, 50, 200, 300), (200, 155, 205, 80)));
        }

        #region IDataModel  ===================================================
        public Root DataRoot => Owner;
        public Item RootItem { get; private set; }
        public string TitleName => "TestModelCanvasControl";
        public string TitleSummary => string.Empty;
        public ControlType ControlType { get; private set; }
        public IPageControl PageControl { get; set; }
        public void Release() { }
        public void TriggerUIRefresh() { }
        #endregion

        #region ISelector  ====================================================
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
