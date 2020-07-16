using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6A7_ChildRelation : ListModelOf<RowX>
    {
        private readonly RelationX_RowX_RowX RowX_RowX;

        internal Model_6A7_ChildRelation(Model_6B3_ChildRelationList owner, RowX item, Relation rel) : base(owner, item) 
        {
            RowX_RowX = rel as RelationX_RowX_RowX;
        }
        internal override IdKey IdKey => IdKey.Model_6A7_ChildRelation;

        public override (string, string) GetKindNameId(Root root = null) => RowX_RowX.GetKindNameId(root);

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => RowX_RowX.ChildCount(Item);
        protected override IList<RowX> GetChildItems() => RowX_RowX.TryGetChildren(Item, out IList<RowX> list) ? list : new RowX[0];
        protected override void CreateChildModel(RowX childItem)
        {
            new Model_6A9_RelatedChild(this, childItem);
        }
        #endregion
    }
}
