using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_689_NodeSymbol : ItemModelOf<SymbolX>
    {
        GraphX Aux1;
        Store Aux2;
        internal Model_689_NodeSymbol(Model_688_NodeSymbolList owner, GraphX aux1, Store aux2, SymbolX item) : base(owner, item) { Aux1 = aux1; Aux2 = aux2; }
        internal override IdKey IdKey => IdKey.Model_689_NodeSymbol;

        public override void GetMenuCommands(Root root, List<ItemCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => RemoveNameProperty(root)));
        }
        private void RemoveNameProperty(Root root)
        {
        }
    }
}
