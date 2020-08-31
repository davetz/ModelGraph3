
namespace ModelGraph.Core
{
    public class Model_7F2_Item : ItemModelOf<Item>
    {
        internal Model_7F2_Item(ItemModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_7F2_Item;

        public override string GetNameId() => Item.GetFullNameId();
        internal override string GetFilterSortId() => $"{Item.GetKindId()} {Item.GetFullNameId()}";
    }
}
