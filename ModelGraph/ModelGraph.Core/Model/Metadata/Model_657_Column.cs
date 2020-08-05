
namespace ModelGraph.Core
{
    public class Model_657_Column : List1ModelOf<ColumnX>
    {
        internal Model_657_Column(Model_661_ColumnList owner, ColumnX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_657_Column;
        public override bool CanExpandRight => true;
        public override bool CanDrag => true;

        public override (string, string) GetKindNameId() => Item.GetKindNameId();

        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            root.Get<Property_Item_Name>().CreatePropertyModel(this, Item);
            root.Get<Property_Item_Summary>().CreatePropertyModel(this, Item);
            root.Get<Property_ColumnX_ValueType>().CreatePropertyModel(this, Item);
            root.Get<Property_ColumnX_IsChoice>().CreatePropertyModel(this, Item);

            return true;
        }
    }
}
