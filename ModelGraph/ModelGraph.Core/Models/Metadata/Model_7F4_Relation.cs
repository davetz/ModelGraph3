
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_7F4_Relation : List3ModelOf<Relation, Item>
    {
        internal Model_7F4_Relation(Model_7F1_PrimeStore owner, Relation item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_7F4_Relation;

        public override bool CanExpandLeft => true;

        public override string GetNameId() => $"{Item.GetNameId()}        [{Item.Pairing}]";
        public override bool CanDrag => true;

        protected override int GetTotalCount() => Item.GetChildLinkPairCount();
        protected override IList<(Item, Item)> GetChildItems() => Item.GetChildLinkPairList();
        protected override (Item, Item) GetItemPair(ItemModel child) => (child as Model_7FF_RelatedItems).ItemPair;
        protected override void CreateChildModel((Item, Item) pair) => new Model_7FF_RelatedItems(this, Item, pair);
    }
}
