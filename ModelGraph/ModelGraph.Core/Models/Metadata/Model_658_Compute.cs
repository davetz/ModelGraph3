﻿
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_658_Compute : List2ModelOf<ComputeX, QueryX>
    {
        internal override IdKey IdKey => IdKey.Model_658_Compute;
        internal Model_658_Compute(Model_666_ComputeList owner, ComputeX item) : base(owner, item) { }

        public override bool CanDrag => true;
        public override bool CanExpandAll => true;
        public override void GetMenuCommands(Root root, List<ItemCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => root.Get<ChangeManager>().RemoveItem(Item)));
        }
        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            list.Clear();
            list.Add(new ValidateCommand(this, () => Item.Owner.ValidateComputeX(Item)));
        }

        public override bool CanReorderItems => true;
        public override bool ReorderItems(Root root, ItemModel dropModel) => (Owner is Model_666_ComputeList o) && ReorderChildItems(root, root.Get<Relation_Store_ComputeX>(), o.Item, Item, dropModel.GetItem());

        protected override int GetTotalCount() => Item.Owner.GetQueryXCount(Item);
        protected override IList<QueryX> GetChildItems() => Item.Owner.GetQueryXList(Item);
        protected override void CreateChildModel(QueryX qx) =>  new Model_69F_ValueLink(this, qx);

        #region ExpandRight  ==================================================
        public override bool CanExpandRight => true;
        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight && Count > 0) return false;
            IsExpandedRight = true;

            root.Get<Property_Item_Name>().CreatePropertyModel(this, Item);
            root.Get<Property_Item_Summary>().CreatePropertyModel(this, Item);
            root.Get<Property_ComputeX_CompuType>().CreatePropertyModel(this, Item);

            if (Item.CompuType == CompuType.CompositeString || Item.CompuType == CompuType.CompositeReversed) 
                root.Get<Property_ComputeX_Separator>().CreatePropertyModel(this, Item);

            if (Item.CompuType != CompuType.RelatedValue) 
                root.Get<Property_ComputeX_Select>().CreatePropertyModel(this, Item);

            root.Get<Property_ComputeX_ValueType>().CreatePropertyModel(this, Item);

            return true;
        }
        #endregion

        #region ModelDrop  ====================================================
        internal override DropAction ModelDrop(Root root, ItemModel dropModel, bool doDrop) => 
            (dropModel.GetItem() is Relation rx && Item.Owner.ComputeXRelationDrop(Item, rx, doDrop)) ? DropAction.Link : DropAction.None;
        #endregion

    }
}
