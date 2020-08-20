using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6EB_OpenList : List3ModelOf<Graph, Query>
    {
        internal Model_6EB_OpenList(Model_6A5_Graph owner, Graph item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6EB_OpenList;
        public override string GetNameId() => Root.GetNameId(IdKey);

        protected override int GetTotalCount() => Item.OpenQuerys.Count;
        protected override IList<(Query, Query)> GetChildItems() => Item.OpenQuerys;
        protected override (Query, Query) GetItemPair(LineModel child) => (child as Model_ItemPair).ItemPair;
        protected override void CreateChildModel((Query, Query) pair) => new Model_ItemPair(this, Item, pair);
    }
}
