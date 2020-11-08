using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class Model_683_GraphNodeList : List2ModelOf<GraphX, Store>
    {
        internal Model_683_GraphNodeList(Model_655_Graph owner, GraphX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_683_GraphNodeList;

        public override string GetKindId() => string.Empty;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override string GetSummaryId() => Root.GetSummaryId(IdKey);

        protected override int GetTotalCount() => Item.NodeStore_QuerySymbol.Count;
        protected override IList<Store> GetChildItems() => Item.NodeStore_QuerySymbol.Keys.ToArray();
        protected override void CreateChildModel(Store childItem) => new Model_684_GraphNode(this, Item, childItem);
    }
}
