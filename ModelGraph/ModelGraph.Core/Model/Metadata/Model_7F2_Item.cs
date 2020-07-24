
namespace ModelGraph.Core
{
    public class Model_7F2_Item : ItemModelOf<Item>
    {
        internal Model_7F2_Item(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_7F2_Item;

        public override (string, string) GetKindNameId(Root root) => (Item.GetKindId(root), Item.GetDoubleNameId(root)) ;
        internal override string GetFilterSortId(Root root) => $"{Item.GetKindId(root)} {Item.GetDoubleNameId(root)}";
    }
}
