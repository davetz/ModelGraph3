using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6E1_Graph : List1ModelOf<GraphX>
    {
        internal Model_6E1_Graph(LineModel owner, GraphX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6E1_Graph;

        public override bool CanExpandLeft => true;
        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;

            return true;
        }

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            var treeModel = GetTreeModel();
            list.Clear();
            list.Add(new NewViewCommand(this, () => { treeModel.NewView(new GraphModel(Item.Owner.Owner, null, ControlType.GraphDisplay)); }));
        }
    }
}
