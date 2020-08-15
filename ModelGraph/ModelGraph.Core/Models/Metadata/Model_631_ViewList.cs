
namespace ModelGraph.Core
{
    public class Model_631_ViewList : ItemModelOf<ViewXManager>
    {
        internal Model_631_ViewList(Model_623_MetadataRoot owner, ViewXManager item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_631_ViewList;
        public override string GetNameId() => Item.Owner.GetNameId(IdKey);
    }
}
