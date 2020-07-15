using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6B3_ChildRelationList : ListModelOf<Relation>
    {
        private Relation_StoreX_ChildRelation StoreX_ChildRelation;

        internal Model_6B3_ChildRelationList(Model_6A1_Row owner, RowX item) : base(owner, item) 
        {
            StoreX_ChildRelation = item.DataRoot.Get<Relation_StoreX_ChildRelation>();
        }
        internal override IdKey IdKey => IdKey.Model_6B3_ChildRelationList;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => StoreX_ChildRelation.ChildCount(Item.Owner);
        protected override IList<Relation> GetChildItems() => StoreX_ChildRelation.TryGetChildren(Item.Owner, out IList<Relation> list) ? list : new Relation[0];
        protected override void CreateChildModel(Relation childItem)
        {
            //new ChildRelationModel_671(this, childItem);
        }
        #endregion

    }
}
