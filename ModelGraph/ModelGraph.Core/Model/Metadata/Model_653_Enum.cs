
namespace ModelGraph.Core
{
    public class Model_653_Enum : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal Model_653_Enum(Model_624_EnumList owner, EnumX item) : base(owner, item) { }
        private EnumX EX => Item as EnumX;
        internal override IdKey IdKey => IdKey.Model_653_Enum;

        public override bool CanExpandLeft => true;
        public override bool CanExpandRight => true;

        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);
        public override string GetSummaryId(Root root) => Item.GetSummaryId(root);

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;
            var ex = EX;

            new Model_664_PairList(this, ex);
            new Model_665_ColumnList(this, ex);
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
