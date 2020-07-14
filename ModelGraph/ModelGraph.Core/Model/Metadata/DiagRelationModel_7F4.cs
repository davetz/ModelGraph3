
namespace ModelGraph.Core
{
    public class DiagRelationModel_7F4 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal DiagRelationModel_7F4(DiagPrimeStoreModel_7F1 owner, Relation item) : base(owner, item) { }
        private Relation RX => Item as Relation;
        internal override IdKey IdKey => IdKey.DiagRelationModel_7F4;

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

            new DiagChildListModel_7F5(this, rx);
            new DiagParentListModel_7F6(this, rx);
            return true;
        }
    }
}
