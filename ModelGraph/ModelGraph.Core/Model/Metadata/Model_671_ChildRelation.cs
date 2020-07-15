
namespace ModelGraph.Core
{
    public class Model_671_ChildRelation : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal Model_671_ChildRelation(Model_662_ChildRelationList owner, Relation item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_671_ChildRelation;
        private Relation RX => Item as Relation;


        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);
        public override string GetSummaryId(Root root) => Item.GetSummaryId(root);

        public override bool CanDrag => true;
        
        public override bool CanExpandRight => true;
        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            new Model_618_CheckProperty(this, Item, root.Get<Property_Relation_IsRequired>());
            new Model_619_ComboProperty(this, Item, root.Get<Property_Relation_Pairing>());
            new Model_617_TextProperty(this, Item, root.Get<Property_Item_Summary>());
            new Model_617_TextProperty(this, Item, root.Get<Property_Item_Name>());

            return true;
        }

        internal override DropAction ModelDrop(Root root, LineModel dropModel, bool doDrop)
        {
            if (dropModel.Item is TableX tx)
            {
                if (doDrop)
                {
                    ItemLinked.Record(root, root.Get<Relation_StoreX_ParentRelation>(), tx, RX);
                }
                return DropAction.Link;
            }
            return DropAction.None;
        }
    }
}
