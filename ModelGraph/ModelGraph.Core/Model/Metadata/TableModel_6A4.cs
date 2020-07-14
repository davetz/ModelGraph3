
namespace ModelGraph.Core
{
    public class TableModel_6A4 : LineModel
    {//==================================== In the ModelingRoot hierarchy  ==============
        internal TableModel_6A4(TableListModel_647 owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.TableModel_6A4;
        public override bool CanExpandLeft => TotalCount > 0;
        public override int TotalCount => ItemStore.Count;
        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);
        internal override string GetFilterSortId(Root root) => Item.GetNameId(root);
        public override string GetSummaryId(Root root) => Item.GetSummaryId(root);

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;

            //new X661_ColumnXListMetaModel(this, Item);

            IsExpandedLeft = true;
            return true;
        }
    }
}
