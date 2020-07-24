
namespace ModelGraph.Core
{
    public class Model_624_ModelingRoot : List1ModelOf<Root>
    {
        internal Model_624_ModelingRoot(Model_612_Root owner, Root item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_624_ModelingRoot;
        public override bool CanExpandLeft => true;

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;

            new Model_63A_ViewList(this, root.Get<ViewXRoot>());
            new Model_647_TableList(this, root.Get<TableXRoot>());
            new Model_648_GraphList(this, root.Get<GraphXRoot>());

            return true;
        }

    }
}
