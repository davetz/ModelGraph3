
namespace ModelGraph.Core
{
    public class Model_648_GraphList : ItemModelOf<GraphXRoot>
    {
        internal Model_648_GraphList(Model_624_ModelingRoot owner, GraphXRoot item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_648_GraphList;
        public override (string, string) GetKindNameId() => (string.Empty, Item.Owner.GetNameId(IdKey));
    }
}
