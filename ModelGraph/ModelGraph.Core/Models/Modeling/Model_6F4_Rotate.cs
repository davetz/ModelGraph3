
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6F4_Rotate : ItemModelOf<Graph>
    {
        internal Model_6F4_Rotate(ItemModel owner, Graph item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6F4_Rotate;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            var pageModel = GetPageModel();
            list.Clear();
            list.Add(new DrawCommand(this, IdKey.Rotate90LeftCommand, Noop));
            list.Add(new DrawCommand(this, IdKey.Rotate45LeftCommand, Noop));
            list.Add(new DrawCommand(this, IdKey.Rotate45RightCommand, Noop));
            list.Add(new DrawCommand(this, IdKey.Rotate90RightCommand, Noop));
        }
        private void Noop() { }
    }
}
