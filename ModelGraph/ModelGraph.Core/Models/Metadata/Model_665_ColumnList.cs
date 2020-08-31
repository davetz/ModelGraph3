using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_665_ColumnList : List2ModelOf<EnumX, ColumnX>
    {
        internal Model_665_ColumnList(Model_653_Enum owner, EnumX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_665_ColumnList;
        public override string GetNameId() => Root.GetNameId(IdKey);

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => Item.Owner.GetEnumColumnCount(Item);
        protected override IList<ColumnX> GetChildItems() => Item.Owner.GetEnumChildColumns(Item);
        protected override void CreateChildModel(ColumnX childItem)
        {
            new Model_667_Column(this, childItem);
        }
        #endregion

        internal override DropAction ModelDrop(Root root, ItemModel dropModel, bool doDrop)
        {
            if (dropModel.GetItem() is ColumnX cx)
            {
                if (doDrop)
                    Item.Owner.AddEnumColumn(Item, cx);
                return DropAction.Link;
            }
            return DropAction.None;
        }

    }
}
