
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_681_GraphColoring : List2ModelOf<GraphX, Property>
    {
        internal Model_681_GraphColoring(Model_655_Graph owner, GraphX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_681_GraphColoring;

        public override string GetKindId() => string.Empty;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override string GetSummaryId() => Root.GetSummaryId(IdKey);

        protected override int GetTotalCount() => 0;
        protected override IList<Property> GetChildItems() => new Property[0];
        protected override void CreateChildModel(Property childItem)
        {
        }
    }
}
