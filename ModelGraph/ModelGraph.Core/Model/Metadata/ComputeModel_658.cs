
namespace ModelGraph.Core
{
    public class ComputeModel_658 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal ComputeModel_658(ComputeListModel_666 owner, ComputeX item) : base(owner, item) { }
        private ComputeX CX => Item as ComputeX;
        internal override IdKey IdKey => IdKey.ComputeModel_658;

        public override bool CanExpandRight => true;
        public override bool CanDrag => true;

        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);

        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            new PropertyTextModel_617(this, Item, root.Get<Property_ComputeX_ValueType>());
            new PropertyTextModel_617(this, Item, root.Get<Property_ComputeX_Select>());
            new PropertyTextModel_617(this, Item, root.Get<Property_ComputeX_Separator>());
            new PropertyComboModel_619(this, Item, root.Get<Property_ComputeX_CompuType>());
            new PropertyTextModel_617(this, Item, root.Get<Property_Item_Summary>());
            new PropertyTextModel_617(this, Item, root.Get<Property_Item_Name>());

            return true;
        }
    }
}
