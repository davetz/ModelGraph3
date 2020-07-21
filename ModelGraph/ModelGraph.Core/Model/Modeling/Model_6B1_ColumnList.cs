using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6B1_ColumnList : ListModelOf<ColumnX>
    {
        private readonly Relation_Store_ColumnX Store_ColumnX;

        internal Model_6B1_ColumnList(Model_6A1_Row owner, RowX item) : base(owner, item) 
        {
            Store_ColumnX = item.GetRoot().Get<Relation_Store_ColumnX>();
        }
        internal override IdKey IdKey => IdKey.Model_6B1_ColumnList;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => Store_ColumnX.ChildCount(Item.Owner);
        protected override IList<ColumnX> GetChildItems() => Store_ColumnX.TryGetChildren(Item.Owner, out IList<ColumnX> list) ? list : new ColumnX[0];
        protected override void CreateChildModel(ColumnX childItem)
        {
            childItem.CreatePropertyModel(this, Item);
        }
        #endregion
    }
}
