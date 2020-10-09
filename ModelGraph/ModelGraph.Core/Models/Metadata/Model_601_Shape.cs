
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
        private const int I_AuxAxis = 5;
        private const int I_CentAxis = 6;
        private const int I_HorzAxis = 7;
        private const int I_VertAxis = 8;
        private const int I_MajorAxis = 9;
        private const int I_MinorAxis = 10;
        private const int I_Dimension = 11;
        private const int I_PolyLocked = 12;
        internal Model_601_Shape(TreeModel owner, SymbolModel item) : base(owner, item) 
        {
            var root = owner.PageModel.Owner;
            root.Get<Property_Shape_LineStyle>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_LineWidth>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_StartCap>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_DashCap>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_EndCap>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_AuxAxis>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_CentAxis>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_HorzAxis>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_VertAxis>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_MajorAxis>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_MinorAxis>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_Dimension>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_Polylocked>().CreatePropertyModel(this, Item);
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
            var sp = Item.PropertyFlags;
            _selectList.Clear();
            if ((sp & ShapeProperty.LineStyle) != 0) _selectList.Add(_fullModelList[I_StrokeStyle]);
            if ((sp & ShapeProperty.StartCap) != 0) _selectList.Add(_fullModelList[I_StartCap]);
            if ((sp & ShapeProperty.DashCap) != 0) _selectList.Add(_fullModelList[I_DashCap]);
            if ((sp & ShapeProperty.EndCap) != 0) _selectList.Add(_fullModelList[I_EndCap]);
            if ((sp & ShapeProperty.LineWidth) != 0) _selectList.Add(_fullModelList[I_StrokeWidth]);
            if ((sp & ShapeProperty.Cent) != 0) _selectList.Add(_fullModelList[I_CentAxis]);
            if ((sp & ShapeProperty.Horz) != 0) _selectList.Add(_fullModelList[I_HorzAxis]);
            if ((sp & ShapeProperty.Vert) != 0) _selectList.Add(_fullModelList[I_VertAxis]);
            if ((sp & ShapeProperty.Major) != 0) _selectList.Add(_fullModelList[I_MajorAxis]);
            if ((sp & ShapeProperty.Minor) != 0) _selectList.Add(_fullModelList[I_MinorAxis]);
            if ((sp & ShapeProperty.Aux) != 0) _selectList.Add(_fullModelList[I_AuxAxis]);
            if ((sp & ShapeProperty.Dim) != 0) _selectList.Add(_fullModelList[I_Dimension]);
            if ((sp & ShapeProperty.PolyLocked) != 0) _selectList.Add(_fullModelList[I_PolyLocked]);
        }

    }
}
