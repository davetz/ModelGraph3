using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6E9_Node : List2ModelOf<Node, Edge>
    {
        internal Model_6E9_Node(Model_6E2_NodeList owner, Node item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6E9_Node;
        public override string GetNameId() => Item.Item.GetNameId();
        protected override int GetTotalCount() => Item.EdgeCount;
        protected override IList<Edge> GetChildItems() => Item.EdgeList;

        protected override void CreateChildModel(Edge childItem) => new Model_6EA_Edge(this, childItem);
    }
}
