using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_663_ParentRelatationList : List2ModelOf<TableX, Relation>
    {
        private readonly Relation_StoreX_ParentRelation _relation_StoreX_ParentRelation;
        internal Model_663_ParentRelatationList(Model_654_Table owner, TableX item) : base(owner, item) 
        {
            _relation_StoreX_ParentRelation = item.GetRoot().Get<Relation_StoreX_ParentRelation>();
        }
        internal override IdKey IdKey => IdKey.Model_663_ParentRelatationList;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override string GetKindId() => string.Empty;

        internal Relation GetRelation() => _relation_StoreX_ParentRelation;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => _relation_StoreX_ParentRelation.ChildCount(Item);
        protected override IList<Relation> GetChildItems() => _relation_StoreX_ParentRelation.TryGetChildren(Item, out IList<Relation> list) ? list : new Relation[0];
        protected override void CreateChildModel(Relation childItem)
        {
            new Model_672_ParentRelation(this, childItem);
        }
        #endregion

        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => AddNewParentRelation(root)));
        }
        private void AddNewParentRelation(Root root)
        {
            var rx = new RelationX_RowX_RowX(root.Get<RelationXManager>(), true);

            // the data root implements undo/redo functionality
            ItemCreated.Record(root, rx);
            ItemLinked.Record(root, _relation_StoreX_ParentRelation, Item, rx);
            ChildDelta -= 2;
        }
    }
}
