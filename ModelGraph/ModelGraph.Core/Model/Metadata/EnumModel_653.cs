
namespace ModelGraph.Core
{
    public class EnumModel_653 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal EnumModel_653(EnumListModel_624 owner, EnumX item) : base(owner, item) { }
        private EnumX EX => Item as EnumX;
        internal override IdKey IdKey => IdKey.EnumModel_653;

        public override bool CanExpandLeft => true;
        public override bool CanExpandRight => true;

        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);
        public override string GetSummaryId(Root root) => Item.GetSummaryId(root);

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;
            var ex = EX;

            new PairListModel_664(this, ex);
            new ColumnListModel_665(this, ex);
            return true;
        }

        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            root.Get<Property_Item_Summary>().CreatePropertyModel(this, Item);
            root.Get<Property_Item_Name>().CreatePropertyModel(this, Item);

            return true;
        }
    }
}
