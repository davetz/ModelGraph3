
namespace ModelGraph.Core
{
    public class Model_7F4_Relation : LineModel
    {
        internal Model_7F4_Relation(Model_7F1_PrimeStore owner, Relation item) : base(owner, item) { }
        private Relation RX => Item as Relation;
        internal override IdKey IdKey => IdKey.Model_7F4_Relation;

        public override bool CanExpandLeft => true;

        public override (string, string) GetKindNameId(Root root)
        {
            var prop = root.Get<Property_Relation_Pairing>();
            var kind = Item.GetKindId(root);
            var name = $"{Item.GetNameId(root)}        [{prop.GetValue(RX)}]";
            return (kind, name);
        }
        public override string GetSummaryId(Root root) => Item.GetSummaryId(root);
        internal override string GetFilterSortId(Root root) => Item.GetNameId(root);

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;
            var rx = RX;

            new Model_7F5_ChildList(this, rx);
            new Model_7F6_ParentList(this, rx);
            return true;
        }
    }
}
