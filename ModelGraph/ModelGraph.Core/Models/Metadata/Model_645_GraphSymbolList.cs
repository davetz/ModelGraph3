using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_645_GraphSymbolList : List2ModelOf<GraphX, SymbolX>
    {
        internal Model_645_GraphSymbolList(Model_655_Graph owner, GraphX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_645_GraphSymbolList;

        public override string GetKindId() => string.Empty;
        public override string GetNameId() => Item.Owner.Owner.GetNameId(IdKey);
        public override string GetSummaryId() => Item.Owner.Owner.GetSummaryId(IdKey);

        protected override int GetTotalCount() => 0;
        protected override IList<SymbolX> GetChildItems() => new SymbolX[0];
        protected override void CreateChildModel(SymbolX childItem)
        {
        }
    }
}
