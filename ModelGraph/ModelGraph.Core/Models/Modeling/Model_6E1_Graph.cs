using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6E1_Graph : List2ModelOf<GraphX, Graph>
    {
        internal Model_6E1_Graph(LineModel owner, GraphX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6E1_Graph;

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            var treeModel = GetTreeModel();
            list.Clear();
            list.Add(new CreateCommand(this, () => { Item.CreateGraph(); }));
        }

        protected override int GetTotalCount() => Item.Count;
        protected override IList<Graph> GetChildItems() => Item.Items;

        protected override void CreateChildModel(Graph childItem) => new Model_6A5_Graph(this, childItem);
    }
}
