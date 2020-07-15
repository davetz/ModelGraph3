
namespace ModelGraph.Core
{
    public class Model_657_Column : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal Model_657_Column(Model_661_ColumnList owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_657_Column;
        public override bool CanExpandRight => true;
        public override bool CanDrag => true;

        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);

        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            new Model_618_CheckProperty(this, Item, root.Get<Property_ColumnX_IsChoice>());
            new Model_619_ComboProperty(this, Item, root.Get<Property_ColumnX_ValueType>());
            new Model_617_TextProperty(this, Item, root.Get<Property_Item_Summary>());
            new Model_617_TextProperty(this, Item, root.Get<Property_Item_Name>());

            return true;
        }
    }
}
