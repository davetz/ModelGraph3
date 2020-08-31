using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6B1_ColumnList : List2ModelOf<RowX, ColumnX>
    {
        private readonly Relation_Store_ColumnX _relation_Store_ColumnX;

        internal Model_6B1_ColumnList(ItemModel owner, RowX item) : base(owner, item) 
        {
            _relation_Store_ColumnX = item.GetRoot().Get<Relation_Store_ColumnX>();
        }
        internal override IdKey IdKey => IdKey.Model_6B1_ColumnList;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override string GetKindId() => string.Empty;

        public override bool CanFilterUsage => true;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => _relation_Store_ColumnX.ChildCount(Item.Owner);
        protected override IList<ColumnX> GetChildItems() => _relation_Store_ColumnX.TryGetChildren(Item.Owner, out IList<ColumnX> list) ? list : new ColumnX[0];
        protected override void CreateChildModel(ColumnX childItem)
        {
            childItem.CreatePropertyModel(this, Item);
        }
        #endregion
    }
}
