using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6D1_QueryItem : List2ModelOf<Item, Query>
    {
        internal Query Aux1;
        internal Model_6D1_QueryItem(ItemModel owner, Item item, Query aux1) : base(owner, item) { Aux1 = aux1; }
        internal override IdKey IdKey => Aux1.GetModelIdKey(false);
        public override string GetNameId() => Item.GetNameId();
        public override string GetKindId() => (Aux1.QueryKind == QueryType.Graph) ? Item.GetParent().GetNameId() : $"{Root.GetKindId(IdKey)} {Item.GetParent().GetNameId()}";

        protected override int GetTotalCount() => Aux1.QueryCount(Item);
        protected override IList<Query> GetChildItems() => Aux1.TryGetQuerys(Item, out Query[] list) ? list : new Query[0]; 

        protected override void CreateChildModel(Query childItem) => new Model_6C1_QueryLink(this, childItem);
    }
}
