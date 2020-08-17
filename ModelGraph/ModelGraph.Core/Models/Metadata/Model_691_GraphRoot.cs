using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_691_GraphRoot : List2ModelOf<Store, QueryX>
    {
        internal GraphX Aux1 { get; private set; }
        internal QueryX Aux2 { get; private set; }
        internal Model_691_GraphRoot(Model_682_GraphRootList owner, Store item, GraphX aux1, QueryX aux2) : base(owner, item) { Aux1 = aux1; Aux2 = aux2; }
        internal override IdKey IdKey => IdKey.Model_691_GraphRoot;

        protected override int GetTotalCount() => Aux1.Owner.GetTotalCount(this);
        protected override IList<QueryX> GetChildItems() => Aux1.Owner.GetChildItems(this);
        protected override void CreateChildModel(QueryX childItem) => new Model_692_GraphLink(this, childItem, Aux1);
        internal override DropAction ModelDrop(Root root, LineModel dropModel, bool doDrop) => Aux1.Owner.ModelDrop(this, dropModel, doDrop) ? DropAction.Link : DropAction.None;

        public override bool CanExpandRight => true;
        internal override bool ExpandRight(Root root)
        {
            if (!IsExpandedRight)
            {
                IsExpandedRight = true;
                root.Get<Property_QueryX_Where>().CreatePropertyModel(this, Aux2);
                return true;
            }
            return false;
        }
    }
}
