using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6A5_Graph : List1ModelOf<Graph>
    {
        internal Model_6A5_Graph(Model_6E1_Graph owner, Graph item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6A5_Graph;

        public override string GetNameId() => $"{Item.Owner.GetNameId()} {Item.Index}";

        public override bool CanDrag => true;

        public override void GetMenuCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => root.Get<ChangeManager>().RemoveItem(Item)));
        }
        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            var treeModel = GetTreeModel();
            list.Clear();
            list.Add(new NewViewCommand(this, () => { treeModel.NewView(new GraphModel(Item.Owner.Owner.Owner, Item)); }));
        }
        public override bool CanExpandLeft => true;
        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;

            //new Model_661_ColumnList(this, Item);
            //new Model_666_ComputeList(this, Item);
            //new Model_662_ChildRelationList(this, Item);
            //new Model_663_ParentRelatationList(this, Item);
            //new Model_673_NamePropertyRelation(this, Item);
            //new Model_674_SummaryPropertyRelation(this, Item);
            return true;
        }
    }
}
