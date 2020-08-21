using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6E8_Root : List2ModelOf<Query, Item>
    {
        internal Model_6E8_Root(Model_6E4_RootList owner, Query item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6E8_Root;
        public override string GetNameId() => Item.Owner.GetNameId();

        protected override int GetTotalCount() => Item.ItemCount;
        protected override IList<Item> GetChildItems() => Item.TryGetItems(out Item[]  items) ? items : new Item[0];

        protected override void CreateChildModel(Item childItem) => new Model_6D1_QueryItem(this, childItem, Item);
    }
}
