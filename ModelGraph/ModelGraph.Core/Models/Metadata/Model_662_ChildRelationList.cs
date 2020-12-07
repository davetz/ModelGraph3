using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_662_ChildRelationList : List2ModelOf<TableX, Relation>
    {
        private readonly Relation_StoreX_ChildRelation _relation_StoreX_ChildRelation;

        internal Model_662_ChildRelationList(Model_654_Table owner, TableX item) : base(owner, item) 
        {
            _relation_StoreX_ChildRelation = item.GetRoot().Get<Relation_StoreX_ChildRelation>();
        }
        internal override IdKey IdKey => IdKey.Model_662_ChildRelationList;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override string GetKindId() => string.Empty;

        internal Relation GetRelation() => _relation_StoreX_ChildRelation;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => _relation_StoreX_ChildRelation.ChildCount(Item);
        protected override IList<Relation> GetChildItems() => _relation_StoreX_ChildRelation.TryGetChildren(Item, out IList<Relation> list) ? list : new Relation[0];
        protected override void CreateChildModel(Relation childItem)
        {
            new Model_671_ChildRelation(this, childItem);
        }
        #endregion

        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => AddNewChildRelation(root)));
        }
        private void AddNewChildRelation(Root root)
        {
            var rx = new RelationX_RowX_RowX(root.Get<RelationXRoot>(), true);

            // the data root implements undo/redo functionality
            ItemCreated.Record(root, rx);
            ItemLinked.Record(root, _relation_StoreX_ChildRelation, Item, rx);
            ChildDelta -= 2;
        }
    }
}
