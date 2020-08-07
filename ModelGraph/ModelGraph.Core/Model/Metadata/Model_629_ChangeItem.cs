
namespace ModelGraph.Core
{
    public class Model_629_ChangeItem : ItemModelOf<ItemChange>
    {
        internal Model_629_ChangeItem(Model_628_ChangeSet owner, ItemChange item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_629_ChangeItem;

        public override string GetNameId() => Item.Name;
    }
}
