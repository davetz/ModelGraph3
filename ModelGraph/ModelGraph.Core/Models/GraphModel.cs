using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public class GraphModel : CanvasModel, IDrawCanvasModel, IPageModel
    {
        internal override Item GetOwner() => Owner;
        internal readonly Graph Graph;

        #region Constructor  ==================================================
        internal GraphModel(Root root, Graph graph)
        {
            Owner = root;
            Graph = graph;
            root.Add(this);

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
        public void RefreshDrawData()
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
        public int Picker1Width => 0;
        public int Picker2Width => 80;

        public void Picker1Select(int y)
        {
        }
        public void Picker2Select(int y)
        {
            var w = Picker2Width;
            var x = w / 2;
            var z = (y / w) * w + x;
            Picker2.Clear();
            Picker2.AddShape(((new Vector2(x, z), new Vector2(x, x)), (ShapeType.Rectangle, StrokeType.Filled, 0), (63, 255, 255, 255)));
        }
        public void Picker2Paste()
        {
        }
        #endregion


        #region HitTest  ======================================================
        public Extent EditorExtent => Graph.ResetExtent();

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
            foreach (var n in _regionNodes)
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
        #endregion

        #region ITreeCanvasModel  =============================================

        public void RefreshViewList(int viewSize, ItemModel leading, ItemModel selected, ChangeType change = ChangeType.None) { }
        public (List<ItemModel>, ItemModel, bool, bool) GetCurrentView(int viewSize, ItemModel leading, ItemModel selected)
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
        public string HeaderTitle => _itemModel.GetItem().GetNameId();

        public void SetUsage(ItemModel model, Usage usage) { }
        public void SetFilter(ItemModel model, string text) { }
        public void SetSorting(ItemModel model, Sorting sorting) { }
        public (int, Sorting, Usage, string) GetFilterParms(ItemModel model) => (_itemModel.Items.Count, Sorting.Unsorted, Usage.None, string.Empty);

        public int GetIndexValue(ItemModel model) => (model is PropertyModel pm) ? pm.GetIndexValue(Owner) : 0;
        public bool GetBoolValue(ItemModel model) => (model is PropertyModel pm) ? pm.GetBoolValue(Owner) : false;
        public string GetTextValue(ItemModel model) => (model is PropertyModel pm) ? pm.GetTextValue(Owner) : string.Empty;
        public string[] GetListValue(ItemModel model) => (model is PropertyModel pm) ? pm.GetListValue(Owner) : new string[0];

        public void PostSetIndexValue(ItemModel model, int val) { if (model is PropertyModel pm) pm.PostSetIndexValue(Owner, val); }
        public void PostSetBoolValue(ItemModel model, bool val) { if (model is PropertyModel pm) pm.PostSetBoolValue(Owner, val); }
        public void PostSetTextValue(ItemModel model, string val) { if (model is PropertyModel pm) pm.PostSetTextValue(Owner, val); }
        public void DragDrop(ItemModel model) { }
        public void DragStart(ItemModel model) { }
        public DropAction DragEnter(ItemModel model) => DropAction.None;
        public void GetButtonCommands(List<ItemCommand> buttonCommands) { }
        public void GetMenuCommands(ItemModel model, List<ItemCommand> menuCommands) { }
        public void GetButtonCommands(ItemModel model, List<ItemCommand> buttonCommands) { }
        #endregion

    }
}

