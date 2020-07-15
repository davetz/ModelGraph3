using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_662_ChildRelationList : ListModelOf<Relation>
    {
        private Relation_StoreX_ChildRelation StoreX_ChildRelation;

        internal Model_662_ChildRelationList(Model_654_Table owner, TableX item) : base(owner, item) 
        {
            StoreX_ChildRelation = item.DataRoot.Get<Relation_StoreX_ChildRelation>();
        }
        private TableX TX => Item as TableX;
        internal override IdKey IdKey => IdKey.Model_662_ChildRelationList;

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
            ItemLinked.Record(root, StoreX_ChildRelation, TX, rx);
            ChildDelta -= 2;
        }
    }
}
