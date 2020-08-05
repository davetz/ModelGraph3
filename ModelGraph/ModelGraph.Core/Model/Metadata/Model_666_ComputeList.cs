using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_666_ComputeList : List2ModelOf<TableX, ComputeX>
    {
        private readonly Relation_Store_ComputeX Store_ComputeX;

        internal Model_666_ComputeList(Model_654_Table owner, TableX item) : base(owner, item) 
        {
            Store_ComputeX = item.GetRoot().Get<Relation_Store_ComputeX>();
        }
        internal override IdKey IdKey => IdKey.Model_666_ComputeList;

        public override bool CanDrag => true;

        internal override string GetFilterSortId() => GetNameId();

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => Store_ComputeX.ChildCount(Item);
        protected override IList<ComputeX> GetChildItems() => Store_ComputeX.TryGetChildren(Item, out IList<ComputeX> list) ? list : new ComputeX[0];
        protected override void CreateChildModel(ComputeX childItem)
        {
            new Model_658_Compute(this, childItem);
        }
        #endregion

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => AddNewComputeX(root)));
        }
        private void AddNewComputeX(Root root)
        {
            var cx = new ComputeX(root.Get<ComputeXRoot>(), true);

            // the data root implements undo/redo functionality
            ItemCreated.Record(root, cx);
            ItemLinked.Record(root, Store_ComputeX, Item, cx);
        }
    }
}
