using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_68B_NodeToolTip : ItemModelOf<Property>
    {
        internal GraphX Aux;

        internal Model_68B_NodeToolTip(Model_68A_NodeToolTipList owner, GraphX aux, Property item) : base(owner, item) { Aux = aux; }
        internal override IdKey IdKey => IdKey.Model_68B_NodeToolTip;

        public override void GetMenuCommands(Root root, List<ItemCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => Aux.Owner.RemoveItem(this, Item)));
        }
    }
}
