
namespace ModelGraph.Core
{
    public class Model_658_Compute : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
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

            new Model_617_TextProperty(this, Item, root.Get<Property_ComputeX_ValueType>());
            new Model_617_TextProperty(this, Item, root.Get<Property_ComputeX_Select>());
            new Model_617_TextProperty(this, Item, root.Get<Property_ComputeX_Separator>());
            new Model_619_ComboProperty(this, Item, root.Get<Property_ComputeX_CompuType>());
            new Model_617_TextProperty(this, Item, root.Get<Property_Item_Summary>());
            new Model_617_TextProperty(this, Item, root.Get<Property_Item_Name>());

            return true;
        }
    }
}
