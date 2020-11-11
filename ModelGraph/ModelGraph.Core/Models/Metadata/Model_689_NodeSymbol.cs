using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_689_NodeSymbol : List1ModelOf<QueryX>
    {
        internal GraphX Aux1;
        internal SymbolX Aux2;
        internal Model_689_NodeSymbol(Model_688_NodeSymbolList owner, GraphX aux1, SymbolX aux2, QueryX item) : base(owner, item) { Aux1 = aux1; Aux2 = aux2; }
        internal override IdKey IdKey => IdKey.Model_689_NodeSymbol;

        public override string GetKindId() => Aux2.GetKindId();
        public override string GetNameId() => Aux2.GetNameId();

        public override void GetMenuCommands(Root root, List<ItemCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => Aux1.Owner.RemoveItem(this, Item)));
        }
        public override bool CanExpandRight => true;
        internal override bool ExpandRight(Root root)
        {
            if (!IsExpandedRight)
            {
                IsExpandedRight = true;
                root.Get<Property_QueryX_Where>().CreatePropertyModel(this, Item);
                return true;
            }
            return false;
        }

    }
}
