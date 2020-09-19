using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6A5_Graph : List1ModelOf<Graph>
    {
        internal Model_6A5_Graph(Model_6E1_Graph owner, Graph item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6A5_Graph;
        public override string GetKindId() =>  Item.GetKindId();

        public override bool CanDrag => true;

        public override void GetMenuCommands(Root root, List<ItemCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => root.Get<ChangeManager>().RemoveItem(Item)));
        }
        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            var pageModel = GetPageModel();
            list.Clear();
            list.Add(new NewViewCommand(this, () => { pageModel.NewView( (p) => { new GraphModel(p, root, Item); }, ControlType.GraphDisplay); }));
        }
        
        public override bool CanExpandLeft => true;
        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;

            new Model_6E2_NodeList(this, Item);
            new Model_6E3_EdgeList(this, Item);
            new Model_6EB_OpenList(this, Item);
            new Model_6E4_RootList(this, Item);
            new Model_6E5_LevelList(this, Item);
            return true;
        }
    }
}
