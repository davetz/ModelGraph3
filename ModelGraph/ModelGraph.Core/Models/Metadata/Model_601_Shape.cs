
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_601_Shape : List1ModelOf<ShapeModel>
    {
        private ItemModel[] _fullModelList;
        private List<ItemModel> _selectList = new List<ItemModel>(10);
        private const int I_StrokeStyle = 0;
        private const int I_StrokeWidth = 1;
        private const int I_StartCap = 2;
        private const int I_DashCap = 3;
        private const int I_EndCap = 4;
        private const int I_Factor1 = 5;
        private const int I_Radius1 = 6;
        private const int I_Radius2 = 7;
        private const int I_Rotation = 8;
        private const int I_Dimension = 9;
        private const int I_SizeX = 10;
        private const int I_SizeY = 11;
        private const int I_CenterX = 12;
        private const int I_CenterY = 13;
        private const int I_ExtentEast = 14;
        private const int I_ExtentWest = 15;
        private const int I_ExtentNorth = 16;
        private const int I_ExtentSouth = 17;
        internal Model_601_Shape(TreeModel owner, ShapeModel item) : base(owner, item) 
        {
            var root = owner.PageModel.Owner;
            root.Get<Property_Shape_StrokeStyle>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_StrokeWidth>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_StartCap>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_DashCap>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_EndCap>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_Factor1>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_Radius1>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_Radius2>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_Rotation1>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_Dimension>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_SizeX>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_SizeY>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_CenterX>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_CenterY>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_ExtentEast>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_ExtentWest>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_ExtentNorth>().CreatePropertyModel(this, Item);
            root.Get<Property_Shape_ExtentSouth>().CreatePropertyModel(this, Item);
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
            if (Item.IsAugmentedPropertyList) new Model_6F5_CutCopy(this, Item);
            Items.AddRange(_selectList);
            return true;
        }

        void PopulateSelectList()
        {
            var sp = Item.PropertyFlags;
            var ap = Item.IsAugmentedPropertyList;
            _selectList.Clear();
            if ((sp & ShapeProperty.StrokeStyle) != 0) _selectList.Add(_fullModelList[I_StrokeStyle]);
            if ((sp & ShapeProperty.StartCap) != 0) _selectList.Add(_fullModelList[I_StartCap]);
            if ((sp & ShapeProperty.DashCap) != 0) _selectList.Add(_fullModelList[I_DashCap]);
            if ((sp & ShapeProperty.EndCap) != 0) _selectList.Add(_fullModelList[I_EndCap]);
            if ((sp & ShapeProperty.StrokeWidth) != 0) _selectList.Add(_fullModelList[I_StrokeWidth]);
            if (Item.IsAugmentedPropertyList)
            {
                if ((sp & ShapeProperty.SizeX) != 0) _selectList.Add(_fullModelList[I_SizeX]);
                if ((sp & ShapeProperty.SizeY) != 0) _selectList.Add(_fullModelList[I_SizeY]);
                if ((sp & ShapeProperty.CenterX) != 0) _selectList.Add(_fullModelList[I_CenterX]);
                if ((sp & ShapeProperty.CenterY) != 0) _selectList.Add(_fullModelList[I_CenterY]);
                if ((sp & ShapeProperty.ExtentEast) != 0) _selectList.Add(_fullModelList[I_ExtentEast]);
                if ((sp & ShapeProperty.ExtentWest) != 0) _selectList.Add(_fullModelList[I_ExtentWest]);
                if ((sp & ShapeProperty.ExtentNorth) != 0) _selectList.Add(_fullModelList[I_ExtentNorth]);
                if ((sp & ShapeProperty.ExtentSouth) != 0) _selectList.Add(_fullModelList[I_ExtentSouth]);
            }
        }

    }
}
