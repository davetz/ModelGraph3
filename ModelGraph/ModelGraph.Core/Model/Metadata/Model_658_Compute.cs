
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_658_Compute : List2ModelOf<ComputeX, QueryX>
    {
        private Relation_QueryX_QueryX _relation_QueryX_QueryX;
        private Relation_ComputeX_QueryX _relation_ComputeX_QueryX;
        private Relation_Relation_QueryX _relation_Relation_QueryX;

        internal override IdKey IdKey => IdKey.Model_658_Compute;
        internal Model_658_Compute(Model_666_ComputeList owner, ComputeX item) : base(owner, item) 
        {
            var root = Item.Owner.Owner;
            _relation_QueryX_QueryX = root.Get<Relation_QueryX_QueryX>();
            _relation_ComputeX_QueryX = root.Get<Relation_ComputeX_QueryX>();
            _relation_Relation_QueryX = root.Get<Relation_Relation_QueryX>();
        }

        public override bool CanDrag => true;
        public override void GetMenuCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => root.Get<ChangeRoot>().RemoveItem(Item)));
        }

        #region List2ModelOf  =================================================
        protected override int GetTotalCount() => _relation_ComputeX_QueryX.TryGetChild(Item, out QueryX q1) ? _relation_QueryX_QueryX.ChildCount(q1) : 0;

        protected override IList<QueryX> GetChildItems() => _relation_ComputeX_QueryX.TryGetChild(Item, out QueryX q1) &&_relation_QueryX_QueryX.TryGetChildren(q1, out IList<QueryX> list) ? list : new QueryX[0];

        protected override void CreateChildModel(QueryX qx)
        {
            new Model_69E_ValueHead(this, qx, _relation_QueryX_QueryX, _relation_Relation_QueryX);
        }
        #endregion

        #region ExpandRight  ==================================================
        public override bool CanExpandRight => true;
        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            root.Get<Property_Item_Name>().CreatePropertyModel(this, Item);
            root.Get<Property_Item_Summary>().CreatePropertyModel(this, Item);
            root.Get<Property_ComputeX_CompuType>().CreatePropertyModel(this, Item);
            root.Get<Property_ComputeX_Separator>().CreatePropertyModel(this, Item);
            root.Get<Property_ComputeX_Select>().CreatePropertyModel(this, Item);
            root.Get<Property_ComputeX_ValueType>().CreatePropertyModel(this, Item);

            return true;
        }
        #endregion

        #region ModelDrop  ====================================================
        internal override DropAction ModelDrop(Root root, LineModel dropModel, bool doDrop)
        {
            if (dropModel.GetItem() is Relation rx && _relation_ComputeX_QueryX.TryGetChild(Item, out QueryX qx1) && root.Get<Relation_Store_ComputeX>().TryGetParent(Item, out Store st))
            {
                var (head, tail) = rx.GetHeadTail();
                if (st == head || st == tail)
                {
                    if (doDrop)
                    {
                        qx1.IsTail = false;
                        var qx2 = new QueryX(root.Get<QueryXRoot>(), QueryType.Value);
                        qx2.IsReversed = st == tail;
                        ItemCreated.Record(root, qx2);
                        ItemLinked.Record(root, _relation_QueryX_QueryX, qx1, qx2);
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
