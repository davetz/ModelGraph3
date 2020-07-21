using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_644_GraphList : ListModelOf<GraphXRoot, GraphX>
    {
        internal Model_644_GraphList(Model_623_MetadataRoot owner, GraphXRoot item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_644_GraphList;

        protected override void CreateChildModel(GraphX gx)
        {
            new Model_655_Graph(this, gx);
        }

        protected override IList<GraphX> GetChildItems() => Item.Items;

        protected override int GetTotalCount() => Item.Count;
    }
}
