using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6B3_ChildRelationList : ListModelOf<RowX, Relation>
    {
        private Relation_StoreX_ChildRelation StoreX_ChildRelation;
        private RowX RX => Item as RowX;

        internal Model_6B3_ChildRelationList(Model_6A1_Row owner, RowX item) : base(owner, item) 
        {
            StoreX_ChildRelation = item.GetRoot().Get<Relation_StoreX_ChildRelation>();
        }
        internal override IdKey IdKey => IdKey.Model_6B3_ChildRelationList;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => StoreX_ChildRelation.ChildCount(Item.GetOwner());
        protected override IList<Relation> GetChildItems() => StoreX_ChildRelation.TryGetChildren(Item.GetOwner(), out IList<Relation> list) ? list : new Relation[0];
        protected override void CreateChildModel(Relation childItem)
        {
            new Model_6A7_ChildRelation(this, RX, childItem);
        }
        #endregion

    }
}
