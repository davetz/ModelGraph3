

namespace ModelGraph.Core
{
    public class PairModel_652 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal PairModel_652(PairListModel_664 owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.PairModel_652;
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
