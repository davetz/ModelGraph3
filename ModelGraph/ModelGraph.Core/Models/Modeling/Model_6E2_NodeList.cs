using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6E2_NodeList : List2ModelOf<Graph, Node>
    {
        internal Model_6E2_NodeList(Model_6A5_Graph owner, Graph item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6E2_NodeList;
        public override string GetNameId() => Root.GetNameId(IdKey);
        protected override int GetTotalCount() => Item.Nodes.Count;
        protected override IList<Node> GetChildItems() => Item.Nodes;

        protected override void CreateChildModel(Node childItem) => new Model_6E9_Node(this, childItem);
    }
}
