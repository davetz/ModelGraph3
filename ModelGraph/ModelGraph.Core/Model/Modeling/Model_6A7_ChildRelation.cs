using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6A7_ChildRelation : List2ModelOf<RowX, RowX>
    {
        private readonly RelationX_RowX_RowX _relationX_RowX_RowX;

        internal Model_6A7_ChildRelation(Model_6B3_ChildRelationList owner, RowX item, Relation rel) : base(owner, item) 
        {
            _relationX_RowX_RowX = rel as RelationX_RowX_RowX;
        }
        internal override IdKey IdKey => IdKey.Model_6A7_ChildRelation;

        public override (string, string) GetKindNameId() => _relationX_RowX_RowX.GetKindNameId();
        internal override string GetFilterSortId() => _relationX_RowX_RowX.GetNameId();

        internal override bool IsItemUsed => _relationX_RowX_RowX.ChildCount(Item) > 0;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => _relationX_RowX_RowX.ChildCount(Item);
        protected override IList<RowX> GetChildItems() => _relationX_RowX_RowX.TryGetChildren(Item, out IList<RowX> list) ? list : new RowX[0];
        protected override void CreateChildModel(RowX childItem)
        {
            new Model_6A1_Row(this, childItem);
        }
        #endregion

        internal override DropAction ModelDrop(Root root, LineModel dropModel, bool doDrop)
        {
            if (dropModel.GetItem() is RowX rx)
            {
                var (_, tail) = _relationX_RowX_RowX.GetHeadTail();
                if (tail == rx.Owner)
                {
                    if (doDrop)
                    {
                        ItemLinked.Record(root, _relationX_RowX_RowX, rx, Item);
                    }
                    return DropAction.Link;
                }
            }
            return DropAction.None;
        }
    }
}
