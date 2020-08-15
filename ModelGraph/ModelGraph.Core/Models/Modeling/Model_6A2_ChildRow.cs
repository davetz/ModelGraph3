using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6A2_ChildRow : List1ModelOf<RowX>
    {
        internal Model_6A2_ChildRow(Model_6A7_ChildRelation owner, RowX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6A2_ChildRow;
        public override string GetNameId() => Owner is Model_6A4_Table ? Item.GetNameId() : Item.GetFullNameId();
        public override string GetKindId() => Owner is Model_6A4_Table ? Item.GetKindId() : string.Empty;

        public override bool CanDrag => true;

        public override void GetMenuCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => root.Get<ChangeManager>().RemoveItem(Item)));
        }

        public override bool CanReorderItems => true;
        public override bool ReorderItems(Root root, LineModel dropModel) => (Owner is Model_6A7_ChildRelation o) && ReorderChildItems(root, o.GetRelation(), o.Item, Item, dropModel.GetItem());


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
