using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_684_GraphNode : List1ModelOf<Store>
    {
        GraphX Aux;
        internal Model_684_GraphNode(Model_683_GraphNodeList owner, GraphX aux, Store item) : base(owner, item) { Aux = aux; } 
        internal override IdKey IdKey => IdKey.Model_684_GraphNode;

        public override bool CanExpandLeft => true;
        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;

            new Model_686_NodeColorList(this, Aux, Item);
            new Model_688_NodeSymbolList(this, Aux, Item);
            new Model_68A_NodeToolTipList(this, Aux, Item);
            return true;
        }

    }
}
