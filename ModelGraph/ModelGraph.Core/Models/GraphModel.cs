using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public class GraphModel : CanvasModel, IPageModel
    {
        internal readonly Graph Graph;

        #region Constructor  ==================================================
        internal GraphModel(Root root, Graph graph) : base(root)
        {
            Graph = graph;
            RefreshDrawData();
        }
        #endregion

        #region IPageModel  ===================================================
        public string TitleName => "TestModelCanvasControl";
        public string TitleSummary => string.Empty;
        public ControlType ControlType => ControlType.GraphDisplay;
        public IPageControl PageControl { get; set; }
        public void Release()
        {
            Owner.Remove(this);
            Discard(); //discard myself and recursivly discard all my children
        }

        public void TriggerUIRefresh()
        {
            if (Graph.ChildDelta != ChildDelta)
            {
                ChildDelta = Graph.ChildDelta;
                PageControl?.Refresh();
            }
        }
        #endregion


        #region CreateDrawData  ===============================================
        override public void RefreshDrawData()
        {
            Editor.Clear();
            foreach (var e in Graph.Edges)
            {
                var p1 = new Vector2(e.Node1.X, e.Node1.Y);
                var p2 = new Vector2(e.Node2.X, e.Node2.Y);
                Editor.AddShape(((p1, p2), (ShapeType.Line, StrokeType.Simple, 2), (255, 0, 255, 255)));
            }
            foreach (var n in Graph.Nodes)
            {
                var c = new Vector2(n.X, n.Y);
                var d = new Vector2(n.DX, n.DY);
                Editor.AddShape(((c, d), (ShapeType.Rectangle, StrokeType.Filled, 1), (255, 255, 0, 255)));
            }
        }
        #endregion

        #region PickerEvents  =================================================
        override public int Picker2Width => 80;

        override public void Picker2Select(int y)
        {
            var w = Picker2Width;
            var x = w / 2;
            var z = (y / w) * w + x;
            Picker2.Clear();
            Picker2.AddShape(((new Vector2(x, z), new Vector2(x, x)), (ShapeType.Rectangle, StrokeType.Filled, 0), (63, 255, 255, 255)));
        }
        #endregion


        #region HitTest  ======================================================
        override public Extent EditorExtent => Graph.ResetExtent();

        override public bool SkimHitTest()
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

        override public bool TapHitTest()
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

        override public void ClearRegion() => _regionNodes.Clear();
        override public bool IsValidRegion()
        {
            var r1 = RegionPoint1;
            var r2 = RegionPoint2;
            _regionNodes.Clear();
            foreach (var n in Graph.Nodes)
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

        private (bool, Node) HitNodeTest(Vector2 p)
        {
            foreach (var n in Graph.Nodes)
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
        override public bool MoveNode()
        {
            if (_hitNode is null) return false;
            var delta = DrawPointDelta(true);
            _hitNode.X += delta.X;
            _hitNode.Y += delta.Y;
            return true;
        }

        override public bool MoveRegion()
        {
            var delta = DrawPointDelta(true);
            foreach (var n in _regionNodes)
            {
                n.X += delta.X;
                n.Y += delta.Y;
            }
            return true;
        }
        #endregion

        #region ITreeCanvasModel  =============================================

        override public void RefreshViewList(int viewSize, ItemModel leading, ItemModel selected, ChangeType change = ChangeType.None) { }
        override public (List<ItemModel>, ItemModel, bool, bool) GetCurrentView(int viewSize, ItemModel leading, ItemModel selected)
        {
            if (_hitNode is null) return (new List<ItemModel>(0), null, false, false);
            if (_itemModel != null)
            {
                if (_itemModel.GetItem() != _hitNode)
                {
                    _itemModel.Discard();
                    _itemModel = new Model_6DA_HitNode(_hitNode);
                }
            }
            else
                _itemModel = new Model_6DA_HitNode(_hitNode);

            _itemModel.ExpandRight(Owner);

            if (selected is null || !_itemModel.Items.Contains(selected))
                selected = _itemModel.Count == 0 ? null : _itemModel.Items[0];

            return (_itemModel.Items, selected, true, true);
        }
        private ItemModel _itemModel;
        override public string HeaderTitle => _itemModel.GetItem().GetNameId();

        override public (int, Sorting, Usage, string) GetFilterParms(ItemModel model) => (_itemModel.Items.Count, Sorting.Unsorted, Usage.None, string.Empty);
        #endregion

    }
}

