using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6A8_ParentRelation : List2ModelOf<RowX, RowX>
    {
        private readonly RelationX_RowX_RowX _relationX_RowX_RowX;

        internal Model_6A8_ParentRelation(Model_6B4_ParentRelationList owner, RowX item, Relation rel) : base(owner, item) 
        {
            _relationX_RowX_RowX = rel as RelationX_RowX_RowX;
        }
        internal override IdKey IdKey => IdKey.Model_6A8_ParentRelation;
        public override (string, string) GetKindNameId(Root root = null) => _relationX_RowX_RowX.GetKindNameId(root);
        internal override string GetFilterSortId(Root root) => _relationX_RowX_RowX.GetNameId(root);

        internal override bool IsItemUsed => _relationX_RowX_RowX.ParentCount(Item) > 0;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => _relationX_RowX_RowX.ParentCount(Item);
        protected override IList<RowX> GetChildItems() => _relationX_RowX_RowX.TryGetParents(Item, out IList<RowX> list) ? list : new RowX[0];
        protected override void CreateChildModel(RowX childItem)
        {
            new Model_6A1_Row(this, childItem);
        }
        #endregion
    }
}
