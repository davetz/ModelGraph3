using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6E4_RootList : List2ModelOf<Graph, Query>
    {
        internal Model_6E4_RootList(Model_6A5_Graph owner, Graph item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6E4_RootList;
        public override string GetNameId() => Root.GetNameId(IdKey);

        protected override int GetTotalCount() => Item.Count;
        protected override IList<Query> GetChildItems() => new Query[0];//Item.Items;

        protected override void CreateChildModel(Query childItem) { } // => new Model_6A5_Graph(this, childItem);
    }
}
