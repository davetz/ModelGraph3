
namespace ModelGraph.Core
{
    public class Model_629_ChangeItem : LineModel
    {
        internal Model_629_ChangeItem(Model_628_ChangeSet owner, ItemChange item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_629_ChangeItem;



        public override (string, string) GetKindNameId(Root root) => (Item.GetKindId(root), ItemChange.Name);

        ItemChange ItemChange => Item as ItemChange;

    }
}
