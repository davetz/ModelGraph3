

namespace ModelGraph.Core
{
    public class PairXModel_652 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal PairXModel_652(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.PairXModel_652;
        public override bool CanExpandRight => true;
        public override bool CanDrag => true;

        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);

        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            new PropertyTextModel_617(this, Item, root.Get<Property_PairX_Value>());
            new PropertyTextModel_617(this, Item, root.Get<Property_PairX_Text>());

            return true;
        }
    }
}
