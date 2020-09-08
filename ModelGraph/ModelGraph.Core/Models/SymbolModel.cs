using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public class SymbolModel : CanvasModel, IDrawCanvasModel, IPageModel
    {
        internal override Item GetOwner() => Owner;
        internal readonly SymbolX Symbol;

        #region Constructor  ==================================================
        internal SymbolModel(Root root, SymbolX symbol)
        {
            Owner = root;
            Symbol = symbol;
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
            if (Symbol.ChildDelta != ChildDelta)
            {
                ChildDelta = Symbol.ChildDelta;
                PageControl?.Refresh();
            }
        }
        #endregion


        #region CreateDrawData  ===============================================
        public void RefreshDrawData()
        {
            Editor.Clear();
        }
        #endregion

        #region PickerEvents  =================================================
        public int Picker1Width => 20;
        public int Picker2Width => 20;

        public void Picker1Select(int y)
        {
            var w = Picker1Width;
            var x = w / 2;
            var z = (y / w) * w + x;
            Picker1.Clear();
            Picker1.AddShape(((new Vector2(x, z), new Vector2(x, x)), (Stroke.ShapeIsFilled, Shape.Rectangle, 0), (63, 255, 255, 255)));
        }
        public void Picker2Select(int y)
        {
            var w = Picker2Width;
            var x = w / 2;
            var z = (y / w) * w + x;
            Picker2.Clear();
            Picker2.AddShape(((new Vector2(x, z), new Vector2(x, x)), (Stroke.ShapeIsFilled, Shape.Rectangle, 0), (63, 255, 255, 255)));
        }
        public void Picker2Paste()
        {
        }
        #endregion


        #region HitTest  ======================================================
        public Extent EditorExtent => new Extent(320,320);

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
            return false;
        }

        public bool TapHitTest()
        {
            ClearHit();
            return false;
        }

        public void ClearRegion() => _regionNodes.Clear();
        public bool IsValidRegion()
        {
            return false;
        }
        private bool RegionHitTest(Vector2 p)
        {
            return false;
        }

        private (bool, Node) HitNodeTest(Vector2 p)
        {
            return (false, null);
        }
        private Node _hitNode;
        private List<Node> _regionNodes = new List<Node>();
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

