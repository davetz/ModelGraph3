using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6B3_ChildRelationList : ListModelOf<Relation>
    {
        private Relation_StoreX_ChildRelation StoreX_ChildRelation;
        private RowX RX => Item as RowX;

        internal Model_6B3_ChildRelationList(LineModel owner, RowX item) : base(owner, item) 
        {
            StoreX_ChildRelation = item.DataRoot.Get<Relation_StoreX_ChildRelation>();
        }
        internal override IdKey IdKey => IdKey.Model_6B3_ChildRelationList;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => StoreX_ChildRelation.ChildCount(Item.Owner);
        protected override IList<Relation> GetChildItems() => StoreX_ChildRelation.TryGetChildren(Item.Owner, out IList<Relation> list) ? list : new Relation[0];
        protected override void CreateChildModel(Relation childItem)
        {
            new Model_6A7_ChildRelation(this, RX, childItem);
        }
        #endregion

    }
}
