
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_601_Shape : List1ModelOf<SymbolModel>
    {
        private ItemModel[] _fullModelList;
        private List<ItemModel> _selectList = new List<ItemModel>(10);
        private const int I_StrokeStyle = 0;
        private const int I_StrokeWidth = 1;
        private const int I_StartCap = 2;
        private const int I_DashCap = 3;
        private const int I_EndCap = 4;
        internal Model_601_Shape(TreeModel owner, SymbolModel item) : base(owner, item) 
        {
            var root = owner.PageModel.Owner;
            root.Get<Property_Shape_StrokeStyle>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_StrokeWidth>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_StartCap>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_DashCap>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_EndCap>().CreatePropertyModel(this, Item);
            _fullModelList = Items.ToArray();
            Items.Clear();
        }
        internal override IdKey IdKey => IdKey.Model_601_Shape;

        public override string GetNameId() => "Shape";
        public override string GetSummaryId() => string.Empty;

        public override bool CanExpandRight => true;
        internal override bool ExpandRight(Root root)
        {
            IsExpandedRight = true;

            PopulateSelectList();
            Items.Clear();
            Items.AddRange(_selectList);
            return true;
        }
        internal override bool Validate(Root root, Dictionary<Item, ItemModel> prev)
        {
            PopulateSelectList();
            Items.Clear();
            Items.AddRange(_selectList);
            return true;
        }

        void PopulateSelectList()
        {
            var sp = Item.ShowProperty;
            _selectList.Clear();
            if ((sp & ShowProperty.StrokeStyle) != 0) _selectList.Add(_fullModelList[I_StrokeStyle]);
            if ((sp & ShowProperty.StrokeWidth) != 0) _selectList.Add(_fullModelList[I_StrokeWidth]);
            if ((sp & ShowProperty.StartCap) != 0) _selectList.Add(_fullModelList[I_StartCap]);
            if ((sp & ShowProperty.DashCap) != 0) _selectList.Add(_fullModelList[I_DashCap]);
            if ((sp & ShowProperty.EndCap) != 0) _selectList.Add(_fullModelList[I_EndCap]);
        }

    }
}
