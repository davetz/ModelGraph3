
namespace ModelGraph.Core
{
    public class Model_7FF_RelatedItems : ItemModelOf<Item>
    {
        internal readonly (Item, Item) ItemPair;
        internal Model_7FF_RelatedItems(Model_7F4_Relation owner, Relation item, (Item, Item) itemPair) : base(owner, item) { ItemPair = itemPair; }
        internal override IdKey IdKey => IdKey.Model_7FF_RelatedItems;
        public override string GetKindId() => string.Empty;


        public override string GetNameId()
        {
            var head = ItemPair.Item1.GetParent();
            var tail = ItemPair.Item2.GetParent();
            var headId = head.IsExternal ? head.GetNameId() : head.GetKindId();
            var tailId = tail.IsExternal ? tail.GetNameId() : tail.GetKindId();

            return $"({headId} : {ItemPair.Item1.GetNameId()}) --> ({tailId} : {ItemPair.Item2.GetNameId()})";
        }
    }
}
