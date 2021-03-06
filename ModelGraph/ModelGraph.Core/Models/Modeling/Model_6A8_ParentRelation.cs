﻿using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6A8_ParentRelation : List2ModelOf<RowX, RowX>
    {
        private readonly RelationX_RowX_RowX _relation_RowX_RowX;

        internal Model_6A8_ParentRelation(Model_6B4_ParentRelationList owner, RowX item, Relation rel) : base(owner, item) 
        {
            _relation_RowX_RowX = rel as RelationX_RowX_RowX;
        }
        internal override IdKey IdKey => IdKey.Model_6A8_ParentRelation;
        public override string GetNameId() => _relation_RowX_RowX.GetNameId();
        internal override string GetFilterSortId() => GetNameId();
        public override string GetKindId() => string.Empty;

        internal override bool IsItemUsed => _relation_RowX_RowX.ParentCount(Item) > 0;

        internal Relation GetRelation() => _relation_RowX_RowX;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => _relation_RowX_RowX.ParentCount(Item);
        protected override IList<RowX> GetChildItems() => _relation_RowX_RowX.TryGetParents(Item, out IList<RowX> list) ? list : new RowX[0];
        protected override void CreateChildModel(RowX childItem)
        {
            new Model_6A3_ParentRow(this, childItem);
        }
        #endregion

        internal override DropAction ModelDrop(Root root, ItemModel dropModel, bool doDrop)
        {
            if (dropModel.GetItem() is RowX rx)
            {
                var (head, _) = _relation_RowX_RowX.GetHeadTail();
                if (head == rx.Owner)
                {
                    if (doDrop)
                    {
                        ItemLinked.Record(root, _relation_RowX_RowX, rx, Item);
                    }
                    return DropAction.Link;
                }
            }
            return DropAction.None;
        }
    }
}
