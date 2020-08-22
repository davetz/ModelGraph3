using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6C1_QueryLink : List2ModelOf<Query, Item>
    {
        internal Model_6C1_QueryLink(Model_6D1_QueryItem owner, Query item) : base(owner, item) { }
        internal override IdKey IdKey => Item.GetModelIdKey(true);
        public override string GetNameId() => Item.Owner.GetNameId();
        public override string GetKindId() => Root.GetKindId(IdKey);


        protected override int GetTotalCount() => Item.ItemCount;
        protected override IList<Item> GetChildItems() => Item.TryGetItems(out Item[] list) ? list : new Item[0]; 

        protected override void CreateChildModel(Item childItem) => new Model_6D1_QueryItem(this, childItem, Item);
    }
}
