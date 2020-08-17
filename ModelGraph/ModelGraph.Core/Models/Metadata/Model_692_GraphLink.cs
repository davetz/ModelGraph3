using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_692_GraphLink : List2ModelOf<QueryX, QueryX>
    {
        internal GraphX Aux1 { get; private set; }
        internal Model_692_GraphLink(LineModel owner, QueryX item, GraphX aux1) : base(owner, item) { Aux1 = aux1; }
        internal override IdKey IdKey => IdKey.Model_692_GraphLink;
        public override string GetKindId() => Item.Owner.Owner.GetKindId(IdKey);

        protected override int GetTotalCount() => Aux1.Owner.GetTotalCount(this);
        protected override IList<QueryX> GetChildItems() => Aux1.Owner.GetChildItems(this);
        protected override void CreateChildModel(QueryX childItem) => new Model_692_GraphLink(this, childItem, Aux1);
        internal override DropAction ModelDrop(Root root, LineModel dropModel, bool doDrop) => Aux1.Owner.ModelDrop(this, dropModel, doDrop) ? DropAction.Link : DropAction.None;
    }
}
