using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_683_GraphNodeList : List2ModelOf<GraphX, Store>
    {
        internal Model_683_GraphNodeList(Model_655_Graph owner, GraphX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_683_GraphNodeList;

        public override string GetKindId() => string.Empty;
        public override string GetNameId() => Item.Owner.Owner.GetNameId(IdKey);
        public override string GetSummaryId() => Item.Owner.Owner.GetSummaryId(IdKey);

        protected override int GetTotalCount() => 0;
        protected override IList<Store> GetChildItems() => new Store[0];
        protected override void CreateChildModel(Store childItem)
        {
        }

    }
}
