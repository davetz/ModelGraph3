
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_69F_ValueLink : List2ModelOf<QueryX, QueryX>
    {
        private Relation _rel;
        private Relation_QueryX_QueryX _relation_QueryX_QueryX;
        private Relation_Relation_QueryX _relation_Relation_QueryX;

        internal override IdKey IdKey => IdKey.Model_69F_ValueLink;
        internal Model_69F_ValueLink(LineModel owner, QueryX item, Relation_QueryX_QueryX qx_qx, Relation_Relation_QueryX re_qx) : base(owner, item) 
        {
            _relation_QueryX_QueryX = qx_qx;
            _relation_Relation_QueryX = re_qx;
            _relation_Relation_QueryX.TryGetParent(item, out _rel);
        }

        public override bool CanDrag => true;
        public override void GetMenuCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => root.Get<ChangeRoot>().RemoveItem(Item)));
        }

        #region List2ModelOf  =================================================
        protected override int GetTotalCount() => _relation_QueryX_QueryX.ChildCount(Item);

        protected override IList<QueryX> GetChildItems() => _relation_QueryX_QueryX.TryGetChildren(Item, out IList<QueryX> list) ? list : new QueryX[0];

        protected override void CreateChildModel(QueryX childItem)
        {
            new Model_69F_ValueLink(this, childItem, _relation_QueryX_QueryX, _relation_Relation_QueryX);
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
            root.Get<Property_QueryX_Select>().CreatePropertyModel(this, Item);

            return true;
        }
        #endregion

        #region ModelDrop  ====================================================
        internal override DropAction ModelDrop(Root root, LineModel dropModel, bool doDrop)
        {
            if (dropModel.GetItem() is Relation rx)
            {
                var (_, t1) = _rel.GetHeadTail();
                var (h2, t2) = rx.GetHeadTail();
                if (t1 == h2 || t1 == t2)
                {
                    if (doDrop)
                    {
                        Item.IsTail = false;
                        var qx2 = new QueryX(root.Get<QueryXRoot>(), QueryType.Value);
                        qx2.IsReversed = t1 == t2;
                        ItemCreated.Record(root, qx2);
                        ItemLinked.Record(root, _relation_QueryX_QueryX, Item, qx2);
                        ItemLinked.Record(root, _relation_Relation_QueryX, rx, qx2);
                    }
                    return DropAction.Link;
                }
            }
            return DropAction.None;
        }
        #endregion

    }
}
