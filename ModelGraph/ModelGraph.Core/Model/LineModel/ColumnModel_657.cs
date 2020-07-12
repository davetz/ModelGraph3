
namespace ModelGraph.Core
{
    public class ColumnModel_657 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal ColumnModel_657(ColumnListModel_661 owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ColumnModel_657;
        public override bool CanExpandRight => true;
        public override bool CanDrag => true;

        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);

        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            new PropertyCheckModel_618(this, Item, root.Get<Property_ColumnX_IsChoice>());
            new PropertyComboModel_619(this, Item, root.Get<Property_ColumnX_ValueType>());
            new PropertyTextModel_617(this, Item, root.Get<Property_Item_Summary>());
            new PropertyTextModel_617(this, Item, root.Get<Property_Item_Name>());

            return true;
        }
    }
}
