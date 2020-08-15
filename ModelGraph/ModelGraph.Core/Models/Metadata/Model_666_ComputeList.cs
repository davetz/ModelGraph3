﻿using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_666_ComputeList : List2ModelOf<TableX, ComputeX>
    {
        private readonly Relation_Store_ComputeX _relation_Store_ComputeX;

        internal Model_666_ComputeList(Model_654_Table owner, TableX item) : base(owner, item) 
        {
            _relation_Store_ComputeX = item.GetRoot().Get<Relation_Store_ComputeX>();
        }
        internal override IdKey IdKey => IdKey.Model_666_ComputeList;
        internal Relation GetRelation() => _relation_Store_ComputeX;

        public override bool CanDrag => true;

        public override string GetNameId() => Item.Owner.Owner.GetNameId(IdKey);
        public override string GetKindId() => string.Empty;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => _relation_Store_ComputeX.ChildCount(Item);
        protected override IList<ComputeX> GetChildItems() => _relation_Store_ComputeX.TryGetChildren(Item, out IList<ComputeX> list) ? list : new ComputeX[0];
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
            var cx = new ComputeX(root.Get<ComputeXManager>(), true);
            var qx = new QueryX(root.Get<QueryXManager>());

            // the data root implements undo/redo functionality
            ItemCreated.Record(root, cx);
            ItemCreated.Record(root, qx);
            ItemLinked.Record(root, _relation_Store_ComputeX, Item, cx);
            ItemLinked.Record(root, root.Get<Relation_ComputeX_QueryX>(), cx, qx);
            ItemLinked.Record(root, root.Get<Relation_Store_QueryX>(), Item, qx);
        }
    }
}