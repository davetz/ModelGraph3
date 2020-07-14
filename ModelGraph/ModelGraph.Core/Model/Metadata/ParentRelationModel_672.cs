
namespace ModelGraph.Core
{
    public class ParentRelationModel_672 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal ParentRelationModel_672(ParentRelatationListModel_663 owner, Relation item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ParentRelationModel_672;
        private Relation RX => Item as Relation;
        public override bool CanExpandRight => true;

        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);
        public override string GetSummaryId(Root root) => Item.GetSummaryId(root);

        public override bool CanDrag => true;

        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            new PropertyCheckModel_618(this, Item, root.Get<Property_Relation_IsRequired>());
            new PropertyComboModel_619(this, Item, root.Get<Property_Relation_Pairing>());
            new PropertyTextModel_617(this, Item, root.Get<Property_Item_Summary>());
            new PropertyTextModel_617(this, Item, root.Get<Property_Item_Name>());

            return true;
        }
        internal override DropAction ModelDrop(Root root, LineModel dropModel, bool doDrop)
        {
            if (dropModel.Item is TableX tx)
            {
                if (doDrop)
                {
                    ItemLinked.Record(root, root.Get<Relation_StoreX_ChildRelation>(), tx, RX);
                }
                return DropAction.Link;
            }
            return DropAction.None;
        }
    }
}
