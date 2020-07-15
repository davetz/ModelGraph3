

namespace ModelGraph.Core
{
    public class Model_652_Pair : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal Model_652_Pair(Model_664_PairList owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_652_Pair;
        public override bool CanExpandRight => true;
        public override bool CanDrag => true;

        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);

        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            new Model_617_TextProperty(this, Item, root.Get<Property_PairX_Value>());
            new Model_617_TextProperty(this, Item, root.Get<Property_PairX_Text>());

            return true;
        }
    }
}
