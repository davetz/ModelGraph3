﻿using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_688_NodeSymbolList : List2ModelOf<Store, QueryX>
    {
        internal readonly GraphX Aux;

        internal Model_688_NodeSymbolList(Model_684_GraphNode owner, GraphX aux, Store item) : base(owner, item) 
        {
            Aux = aux;
        }
        internal override IdKey IdKey => IdKey.Model_688_NodeSymbolList;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override string GetKindId() => string.Empty;

        protected override int GetTotalCount() => Aux.Owner.ChildCount(this);

        protected override IList<QueryX> GetChildItems() => Aux.Owner.GetChildren(this);

        protected override void CreateChildModel(QueryX qx)
        {
            var sx = Aux.Owner.GetSymbol(this, qx);
            new Model_689_NodeSymbol(this, Aux, sx, qx);
        }
        internal override DropAction ModelDrop(Root root, ItemModel dropModel, bool doDrop) => (dropModel.GetItem() is SymbolX sx && Aux.Owner.DropSymbol(this, sx, Item, doDrop)) ? DropAction.Link :  DropAction.None;
    }
}
