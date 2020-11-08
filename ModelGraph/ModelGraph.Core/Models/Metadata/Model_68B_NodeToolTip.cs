using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_68B_NodeToolTip : ItemModelOf<Property>
    {
        internal Model_68B_NodeToolTip(Model_673_NamePropertyRelation owner, Property item) : base(owner, item) { }
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
