﻿
namespace ModelGraph.Core
{
    public class Model_658_Compute : LineModel
    {
        internal Model_658_Compute(Model_666_ComputeList owner, ComputeX item) : base(owner, item) { }
        private ComputeX CX => Item as ComputeX;
        internal override IdKey IdKey => IdKey.Model_658_Compute;

        public override bool CanExpandRight => true;
        public override bool CanDrag => true;

        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);

        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            root.Get<Property_Item_Name>().CreatePropertyModel(this, Item);
            root.Get<Property_Item_Summary>().CreatePropertyModel(this, Item);
            root.Get<Property_ComputeX_CompuType>().CreatePropertyModel(this, Item);
            root.Get<Property_ComputeX_Separator>().CreatePropertyModel(this, Item);
            root.Get<Property_ComputeX_Select>().CreatePropertyModel(this, Item);
            root.Get<Property_ComputeX_ValueType>().CreatePropertyModel(this, Item);

            return true;
        }
    }
}