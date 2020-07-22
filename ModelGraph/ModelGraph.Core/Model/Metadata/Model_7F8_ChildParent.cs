
namespace ModelGraph.Core
{
    public class Model_7F8_ChildParent : LineModelOf<Item>
    {
        internal readonly (Item, Item) ItemPair;
        internal Model_7F8_ChildParent(Model_7F6_ParentList owner, Relation item, (Item, Item) itemPair) : base(owner, item) { ItemPair = itemPair; }
        internal override IdKey IdKey => IdKey.Model_7F8_ChildParent;
        public override (string, string) GetKindNameId(Root root) => (GetKindId(root), $"({ItemPair.Item1.GetParentId(root)} : {ItemPair.Item1.GetNameId(root)}) --> ({ItemPair.Item2.GetParentId(root)} : {ItemPair.Item2.GetNameId(root)})");
    }
}
