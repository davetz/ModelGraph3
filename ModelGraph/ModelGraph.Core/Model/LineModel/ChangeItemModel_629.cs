
namespace ModelGraph.Core
{
    public class ChangeItemModel_629 : LineModel
    {
        internal ChangeItemModel_629(ChangeSetModel_628 owner, ItemChange item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ChangeItemModel_629;



        public override (string, string) GetKindNameId(Root root) => (Item.GetKindId(root), ItemChange.Name);

        ItemChange ItemChange => Item as ItemChange;

    }
}
