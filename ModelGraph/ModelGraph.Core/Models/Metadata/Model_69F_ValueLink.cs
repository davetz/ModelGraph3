
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_69F_ValueLink : List2ModelOf<QueryX, QueryX>
    {
        internal override IdKey IdKey => IdKey.Model_69F_ValueLink;
        internal Model_69F_ValueLink(LineModel owner, QueryX item) : base(owner, item) { }

        public override bool CanDrag => true;
        public override void GetMenuCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => root.Get<ChangeManager>().RemoveItem(Item)));
        }

        #region List2ModelOf  =================================================
        protected override int GetTotalCount() => Item.Owner.QueryQueryChildCount(Item);
        protected override IList<QueryX> GetChildItems() => Item.Owner.QueryQueryChildList(Item);

        protected override void CreateChildModel(QueryX childItem)
        {
            new Model_69F_ValueLink(this, childItem);
        }
        #endregion

        #region ExpandRight  ==================================================
        public override bool CanExpandRight => true;
        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            root.Get<Property_QueryX_Relation>().CreatePropertyModel(this, Item);
            root.Get<Property_QueryX_IsReversed>().CreatePropertyModel(this, Item);
            root.Get<Property_QueryX_Where>().CreatePropertyModel(this, Item);

            switch (GetCompuType())
            {
                case CompuType.RowValue:
                    break;
                case CompuType.RelatedValue when Item.IsTail:
                        root.Get<Property_QueryX_Select>().CreatePropertyModel(this, Item);
                    break;
                case CompuType.CompositeString:
                case CompuType.CompositeReversed:
                    root.Get<Property_QueryX_Select>().CreatePropertyModel(this, Item);
                    break;
            }
            return true;
        }

        private CompuType GetCompuType()
        {
            var m = Owner;
            for (int i = 0; i < 30; i++) //avoid infinite loop
            {
                if (m is null) break;
                if (m is Model_658_Compute mc) return mc.Item.CompuType;
                m = m.Owner;
            }
            throw new System.Exception("Model_69F_ValueLink corrupt lineModel hierarchy");
        }
        #endregion

        #region ModelDrop  ====================================================
        internal override DropAction ModelDrop(Root root, LineModel dropModel, bool doDrop)
        {
            return (dropModel.GetItem() is Relation rx && Item.Owner.QueryRelationDrop(Item, rx, doDrop)) ? DropAction.Link : DropAction.None;
        }
        #endregion
    }
}
