
namespace ModelGraph.Core
{
    public class Model_652_Pair : List1ModelOf<PairX>
    {
        internal Model_652_Pair(Model_664_PairList owner, PairX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_652_Pair;
        public override bool CanExpandRight => true;
        public override bool CanDrag => true;

        public override (string, string) GetKindNameId() => Item.GetKindNameId();

        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            root.Get<Property_PairX_Text>().CreatePropertyModel(this, Item);
            root.Get<Property_PairX_Value>().CreatePropertyModel(this, Item);

            return true;
        }
    }
}
