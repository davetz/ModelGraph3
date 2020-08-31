using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6A1_Row : List1ModelOf<RowX>
    {
        internal Model_6A1_Row(Model_6A4_Table owner, RowX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6A1_Row;
        public override string GetNameId() => Owner is Model_6A4_Table ? Item.GetNameId() : Item.GetFullNameId();
        public override string GetKindId() => Owner is Model_6A4_Table ? Item.GetKindId() : string.Empty;

        public override bool CanDrag => true;

        public override void GetMenuCommands(Root root, List<ItemCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => root.Get<ChangeManager>().RemoveItem(Item)));
        }

        public override bool CanReorderItems => true;
        public override bool ReorderItems(Root root, ItemModel dropModel) => (dropModel is Model_6A1_Row m) && ReorderStoreItems(root, Item.Owner, Item, m.Item);


        public override bool CanExpandLeft => true;
        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;

            new Model_6B1_ColumnList(this, Item);
            new Model_6B2_ComputeList(this, Item);
            new Model_6B3_ChildRelationList(this, Item);
            new Model_6B4_ParentRelationList(this, Item);
            return true;
        }

        public override bool CanExpandRight => Item.Owner.Owner.GetChoiceProperties(Item.Owner).Count > 0;
        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            var list = Item.Owner.Owner.GetChoiceProperties(Item.Owner);
            foreach (var cx in list)
            {
                cx.CreatePropertyModel(this, Item);
            }
            return true;
        }
    }
}
