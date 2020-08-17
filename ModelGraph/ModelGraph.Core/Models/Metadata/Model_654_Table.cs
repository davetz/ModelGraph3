using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_654_Table : List1ModelOf<TableX>
    {
        internal Model_654_Table(Model_643_TableList owner, TableX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_654_Table;

        public override bool CanDrag => true;

        public override void GetMenuCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => root.Get<ChangeManager>().RemoveItem(Item)));
        }

        public override bool CanReorderItems => true;
        public override bool ReorderItems(Root root, LineModel dropModel) => (dropModel is Model_643_TableList m) && ReorderStoreItems(root, Item.Owner, Item, m.Item);

        public override bool CanExpandLeft => true;
        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;

            new Model_661_ColumnList(this, Item);
            new Model_666_ComputeList(this, Item);
            new Model_662_ChildRelationList(this, Item);
            new Model_663_ParentRelatationList(this, Item);
            new Model_673_NamePropertyRelation(this, Item);
            new Model_674_SummaryPropertyRelation(this, Item);
            return true;
        }

        public override bool CanExpandRight => true;
        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            root.Get<Property_Item_Name>().CreatePropertyModel(this, Item);
            root.Get<Property_Item_Summary>().CreatePropertyModel(this, Item);
            return true;
        }
    }
}
