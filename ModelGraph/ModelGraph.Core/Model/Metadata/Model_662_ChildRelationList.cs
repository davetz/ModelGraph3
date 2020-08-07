using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_662_ChildRelationList : List2ModelOf<TableX, Relation>
    {
        private Relation_StoreX_ChildRelation StoreX_ChildRelation;

        internal Model_662_ChildRelationList(Model_654_Table owner, TableX item) : base(owner, item) 
        {
            StoreX_ChildRelation = item.GetRoot().Get<Relation_StoreX_ChildRelation>();
        }
        internal override IdKey IdKey => IdKey.Model_662_ChildRelationList;
        public override string GetNameId() => Item.Owner.Owner.GetNameId(IdKey);
        public override string GetKindId() => string.Empty;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => StoreX_ChildRelation.ChildCount(Item);
        protected override IList<Relation> GetChildItems() => StoreX_ChildRelation.TryGetChildren(Item, out IList<Relation> list) ? list : new Relation[0];
        protected override void CreateChildModel(Relation childItem)
        {
            new Model_671_ChildRelation(this, childItem);
        }
        #endregion

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => AddNewChildRelation(root)));
        }
        private void AddNewChildRelation(Root root)
        {
            var rx = new RelationX_RowX_RowX(root.Get<RelationXRoot>(), true);

            // the data root implements undo/redo functionality
            ItemCreated.Record(root, rx);
            ItemLinked.Record(root, StoreX_ChildRelation, Item, rx);
            ChildDelta -= 2;
        }
    }
}
