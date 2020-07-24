using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6B2_ComputeList : List2ModelOf<RowX, ComputeX>
    {
        private readonly Relation_Store_ComputeX Store_ComputeX;

        internal Model_6B2_ComputeList(Model_6A1_Row owner, RowX item) : base(owner, item) 
        {
            Store_ComputeX = item.GetRoot().Get<Relation_Store_ComputeX>();
        }
        internal override IdKey IdKey => IdKey.Model_6B2_ComputeList;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => Store_ComputeX.ChildCount(Item.Owner);
        protected override IList<ComputeX> GetChildItems() => Store_ComputeX.TryGetChildren(Item.Owner, out IList<ComputeX> list) ? list : new ComputeX[0];
        protected override void CreateChildModel(ComputeX childItem)
        {
            childItem.CreatePropertyModel(this, Item);
        }
        #endregion
    }
}
