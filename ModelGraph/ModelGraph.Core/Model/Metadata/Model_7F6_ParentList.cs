using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_7F6_ParentList : List3ModelOf<Relation>
    {
        internal Model_7F6_ParentList(Model_7F4_Relation owner, Relation item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_7F6_ParentList;

        protected override int GetTotalCount() => Item.GetParentLinkPairCount();
        protected override IList<(Item, Item)> GetChildItems() => Item.GetParentLinkPairList();
        protected override (Item, Item) GetItemPair(LineModel child) => (child as Model_7F8_ChildParent).ItemPair;
        protected override void CreateChildModel((Item, Item) pair) => new Model_7F8_ChildParent(this, Item, pair);
    }
}
