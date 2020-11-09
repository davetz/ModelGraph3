using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_68B_NodeToolTip : ItemModelOf<Property>
    {
        internal GraphX Aux1;
        internal Store Aux2;

        internal Model_68B_NodeToolTip(Model_68A_NodeToolTipList owner, GraphX aux1, Store aux2, Property item) : base(owner, item) { Aux1 = aux1; Aux2 = aux2; }
        internal override IdKey IdKey => IdKey.Model_68B_NodeToolTip;

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
