using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_644_GraphList : List2ModelOf<GraphXRoot, GraphX>
    {
        internal Model_644_GraphList(Model_623_MetadataRoot owner, GraphXRoot item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_644_GraphList;
        public override (string, string) GetKindNameId() => (string.Empty, Item.Owner.GetNameId(IdKey));

        protected override void CreateChildModel(GraphX gx)
        {
            new Model_655_Graph(this, gx);
        }

        protected override IList<GraphX> GetChildItems() => Item.Items;

        protected override int GetTotalCount() => Item.Count;
    }
}
