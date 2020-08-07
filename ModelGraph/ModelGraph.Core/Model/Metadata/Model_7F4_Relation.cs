
namespace ModelGraph.Core
{
    public class Model_7F4_Relation : List1ModelOf<Relation>
    {
        internal Model_7F4_Relation(Model_7F1_PrimeStore owner, Relation item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_7F4_Relation;

        public override bool CanExpandLeft => true;

        public override string GetNameId() => $"{Item.GetNameId()}        [{Item.Pairing}]";
        public override string GetSummaryId() => Item.GetSummaryId();
        internal override string GetFilterSortId() => Item.GetNameId();

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;

            new Model_7F5_ChildList(this, Item);
            new Model_7F6_ParentList(this, Item);
            return true;
        }
    }
}
