using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_661_ColumnList : ListModelOf<TableX, ColumnX>
    {
        private readonly Relation_Store_ColumnX Store_ColumnX;

        internal Model_661_ColumnList(Model_654_Table owner, TableX item) : base(owner, item) 
        {
            Store_ColumnX = item.GetRoot().Get<Relation_Store_ColumnX>();
        }
        internal override IdKey IdKey => IdKey.Model_661_ColumnList;
        internal override string GetFilterSortId(Root root) => GetNameId(root);

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => Store_ColumnX.ChildCount(Item);
        protected override IList<ColumnX> GetChildItems() => Store_ColumnX.TryGetChildren(Item, out IList<ColumnX> list) ? list : new ColumnX[0];
        protected override void CreateChildModel(ColumnX childItem)
        {
            new Model_657_Column(this, childItem);
        }
        #endregion

        public override void GetButtonCommands(Root root, List<LineCommand> list)
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
