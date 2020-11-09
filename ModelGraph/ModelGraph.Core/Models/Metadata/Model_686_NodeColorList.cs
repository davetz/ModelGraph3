using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_686_NodeColorList : List2ModelOf<Store, Property>
    {
        internal GraphX Aux;
        internal Model_686_NodeColorList(Model_684_GraphNode owner, GraphX aux, Store item) : base(owner, item) { Aux = aux; }
        internal override IdKey IdKey => IdKey.Model_686_NodeColorList;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override string GetKindId() => string.Empty;

        protected override int GetTotalCount() => Aux.Owner.GetTotalCount(this);

        protected override IList<Property> GetChildItems() => Aux.Owner.GetChildItems(this);

        protected override void CreateChildModel(Property np)
        {
            new Model_687_NodeColorProperty(this, np);
        }

        internal override DropAction ModelDrop(Root root, ItemModel dropModel, bool doDrop) => (dropModel.GetItem() is Property p && Aux.Owner.ModelDrop(this, p, Aux, Item, doDrop)) ?  DropAction.Link :  DropAction.None;
    }
}
