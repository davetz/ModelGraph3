
namespace ModelGraph.Core
{
    public class Model_7F7_ParentChild : ItemModelOf<Item>
    {
        internal readonly (Item, Item) ItemPair;
        internal Model_7F7_ParentChild(Model_7F5_ChildList owner, Relation item, (Item, Item) itemPair) : base(owner, item) { ItemPair = itemPair; }
        internal override IdKey IdKey => IdKey.Model_7F7_ParentChild;

        public override (string, string) GetKindNameId() => (GetKindId(), $"({ItemPair.Item1.GetParentId()} : {ItemPair.Item1.GetNameId()}) --> ({ItemPair.Item2.GetParentId()} : {ItemPair.Item2.GetNameId()})");
    }
}
