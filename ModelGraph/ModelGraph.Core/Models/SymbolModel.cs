using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public class SymbolModel : CanvasModel, IPageModel
    {
        internal readonly SymbolX Symbol;

        #region Constructor  ==================================================
        internal SymbolModel(Root root, SymbolX symbol) : base(root)
        {        
            Symbol = symbol;
            RefreshDrawData();
            PageControl?.Refresh();
        }
        #endregion

        #region IPageModel  ===================================================
        public string TitleName => "TestModelCanvasControl";
        public string TitleSummary => string.Empty;
        public ControlType ControlType => ControlType.SymbolEditor;
        public IPageControl PageControl { get; set; }
        public void Release()
        {
            Owner.Remove(this);
            Discard(); //discard myself and recursivly discard all my children
        }

        public void TriggerUIRefresh()
        {
            if (Symbol.ModelDelta != ModelDelta)
            {
                ModelDelta = Symbol.ModelDelta;
                PageControl?.Refresh();
            }
        }
        #endregion


        #region RefreshDrawData  ==============================================
        override public void RefreshDrawData()
        {
            var hw = (float)ESZ/2;
            var center = new Vector2(hw, hw);

            Editor.Clear();
            var shapes = Symbol.GetShapes();
            if (shapes.Count == 0) shapes.Add(new Circle());
            foreach (var s in shapes)
            {
                s.AddDrawData(Editor, hw, center);
            }
        }
        #endregion

        #region PickerEvents  =================================================
        override public int Picker1Width => 30;
        override public int Picker2Width => 30;

        override public void Picker1Select(int y)
        {
            var w = Picker1Width;
            var x = w / 2;
            var z = (y / w) * w + x;
            Picker1.Clear();
            Picker1.AddShape(((new Vector2(x, z), new Vector2(x, x)), (ShapeType.Rectangle, StrokeType.Filled, 0), (63, 255, 255, 255)));
        }
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
        override public Extent EditorExtent => new Extent(-ESZ, -ESZ, ESZ, ESZ);
        private const int ESZ = 80;

        override public bool SkimHitTest()
        {
            ClearHit();
            return false;
        }

        override public bool TapHitTest()
        {
            ClearHit();
            return false;
        }

        override public void ClearRegion() => _regionNodes.Clear();

        private Node _hitNode;
        private List<Node> _regionNodes = new List<Node>();
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

        #endregion

    }
}

