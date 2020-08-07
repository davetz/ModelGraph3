
namespace ModelGraph.Core
{
    public class Model_7F8_ChildParent : ItemModelOf<Item>
    {
        internal readonly (Item, Item) ItemPair;
        internal Model_7F8_ChildParent(Model_7F6_ParentList owner, Relation item, (Item, Item) itemPair) : base(owner, item) { ItemPair = itemPair; }
        internal override IdKey IdKey => IdKey.Model_7F8_ChildParent;
        public override string GetNameId() => $"({ItemPair.Item1.GetParent().GetNameId()} : {ItemPair.Item1.GetNameId()}) --> ({ItemPair.Item2.GetParent().GetNameId()} : {ItemPair.Item2.GetNameId()})";
    }
}
