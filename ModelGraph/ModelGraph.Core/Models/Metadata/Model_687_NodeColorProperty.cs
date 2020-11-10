using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_687_NodeColorProperty : ItemModelOf<Property>
    {
        internal GraphX Aux;
        internal Model_687_NodeColorProperty(Model_686_NodeColorList owner, GraphX aux, Property item) : base(owner, item) { Aux = aux; }
        internal override IdKey IdKey => IdKey.Model_687_NodeColorProperty;

        public override void GetMenuCommands(Root root, List<ItemCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => Aux.Owner.RemoveItem(this, Item)));
        }
    }
}
