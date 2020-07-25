using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_7F5_ChildList : List3ModelOf<Relation>
    {
        internal Model_7F5_ChildList(Model_7F4_Relation owner, Relation item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_7F5_ChildList;


        protected override int GetTotalCount() => Item.GetChildLinkPairCount();
        protected override IList<(Item, Item)> GetChildItems() => Item.GetChildLinkPairList();
        protected override (Item, Item) GetItemPair(LineModel child) => (child as Model_7F7_ParentChild).ItemPair;
        protected override void CreateChildModel((Item, Item) pair) => new Model_7F7_ParentChild(this, Item, pair);
    }
}
