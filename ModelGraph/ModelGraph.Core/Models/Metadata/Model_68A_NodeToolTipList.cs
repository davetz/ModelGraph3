using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_68A_NodeToolTipList : List2ModelOf<Store, Property>
    {
        GraphX Aux;

        internal Model_68A_NodeToolTipList(Model_684_GraphNode owner, GraphX aux, Store item) : base(owner, item) { Aux = aux; }
        internal override IdKey IdKey => IdKey.Model_68A_NodeToolTipList;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override string GetKindId() => string.Empty;

        protected override int GetTotalCount() => 0;

        protected override IList<Property> GetChildItems() => new Property[0];

        protected override void CreateChildModel(Property np)
        {
            //new Model_675_NameProperty(this, np);
        }

        internal override DropAction ModelDrop(Root root, ItemModel dropModel, bool doDrop)
        {
            return DropAction.None;
        }

    }
}
