using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_661_ColumnList : List2ModelOf<TableX, ColumnX>
    {
        private readonly Relation_Store_ColumnX _relation_Store_ColumnX;

        internal Model_661_ColumnList(Model_654_Table owner, TableX item) : base(owner, item) 
        {
            _relation_Store_ColumnX = item.GetRoot().Get<Relation_Store_ColumnX>();
        }
        internal override IdKey IdKey => IdKey.Model_661_ColumnList;
        internal Relation GetRelation() => _relation_Store_ColumnX;

        public override string GetNameId() => Root.GetNameId(IdKey);
        public override string GetKindId() => string.Empty;

        internal override string GetFilterSortId() => GetNameId();

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => _relation_Store_ColumnX.ChildCount(Item);
        protected override IList<ColumnX> GetChildItems() => _relation_Store_ColumnX.TryGetChildren(Item, out IList<ColumnX> list) ? list : new ColumnX[0];
        protected override void CreateChildModel(ColumnX childItem)
        {
            new Model_657_Column(this, childItem);
        }
        #endregion

        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => AddNewColumnX(root)));
        }
        private void AddNewColumnX(Root root)
        {
            var cx = new ColumnX(root.Get<ColumnXRoot>(), true);
            var sto = Item as Store;

            // the data root implements undo/redo functionality
            ItemCreated.Record(root, cx);
            ItemLinked.Record(root, root.Get<Relation_Store_ColumnX>(), sto, cx);
        }
    }
}
