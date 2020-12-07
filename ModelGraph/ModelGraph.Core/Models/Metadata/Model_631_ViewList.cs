
namespace ModelGraph.Core
{
    public class Model_631_ViewList : ItemModelOf<ViewXRoot>
    {
        internal Model_631_ViewList(Model_623_MetadataRoot owner, ViewXRoot item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_631_ViewList;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override string GetKindId() => string.Empty;
    }
}
