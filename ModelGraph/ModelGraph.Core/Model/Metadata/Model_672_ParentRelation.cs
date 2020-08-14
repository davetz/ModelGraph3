
namespace ModelGraph.Core
{
    public class Model_672_ParentRelation : List1ModelOf<Relation>
    {
        internal Model_672_ParentRelation(Model_663_ParentRelatationList owner, Relation item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_672_ParentRelation;
        public override bool CanExpandRight => true;

        public override bool CanDrag => true;

        public override bool CanReorderItems => true;
        public override bool ReorderItems(Root root, LineModel dropModel) => (Owner is Model_663_ParentRelatationList o) && ReorderChildItems(root, o.GetRelation(), o.Item, Item, dropModel.GetItem());

        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            root.Get<Property_Item_Name>().CreatePropertyModel(this, Item);
            root.Get<Property_Item_Summary>().CreatePropertyModel(this, Item);
            root.Get<Property_Relation_Pairing>().CreatePropertyModel(this, Item);
            root.Get<Property_Relation_IsRequired>().CreatePropertyModel(this, Item);

            return true;
        }
        internal override DropAction ModelDrop(Root root, LineModel dropModel, bool doDrop)
        {
            if (dropModel.GetItem() is TableX tx)
            {
                if (doDrop)
                {
                    ItemLinked.Record(root, root.Get<Relation_StoreX_ChildRelation>(), tx, Item);
                }
                return DropAction.Link;
            }
            return DropAction.None;
        }
    }
}
