using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6E3_EdgeList : List2ModelOf<Graph, Edge>
    {
        internal Model_6E3_EdgeList(Model_6A5_Graph owner, Graph item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6E3_EdgeList;
        public override string GetNameId() => Root.GetNameId(IdKey);

        protected override int GetTotalCount() => Item.Edges.Count;
        protected override IList<Edge> GetChildItems() => Item.Edges;

        protected override void CreateChildModel(Edge childItem) => new Model_6EA_Edge(this, childItem);
    }
}
