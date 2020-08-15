
namespace ModelGraph.Core
{
    public class Model_63A_ViewList : ItemModelOf<ViewXManager>
    {
        internal Model_63A_ViewList(Model_624_ModelingRoot owner, ViewXManager item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_63A_ViewList;
        public override string GetNameId() => Item.Owner.GetNameId(IdKey);
    }
}
