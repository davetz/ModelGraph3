using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_686_NodeColorList : List2ModelOf<Store, Property>
    {
        GraphX Aux;
        internal Model_686_NodeColorList(Model_684_GraphNode owner, GraphX aux, Store item) : base(owner, item) { Aux = aux; }
        internal override IdKey IdKey => IdKey.Model_686_NodeColorList;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override string GetKindId() => string.Empty;

        protected override int GetTotalCount() => Aux.Owner.GetTotalCount(this);

        protected override IList<Property> GetChildItems() => Aux.Owner.GetChildItems(this);

        protected override void CreateChildModel(Property np)
        {
            //new Model_675_NameProperty(this, np);
        }

        internal override DropAction ModelDrop(Root root, ItemModel dropModel, bool doDrop) => (dropModel.GetItem() is Property np && Aux.Owner.ModelDrop(this, np, Item, doDrop)) ?  DropAction.Link :  DropAction.None;
    }
}
