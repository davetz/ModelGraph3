
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6F2_HorzAlign : ItemModelOf<Graph>
    {
        internal Model_6F2_HorzAlign(ItemModel owner, Graph item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6F2_HorzAlign;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            var pageModel = GetPageModel();
            list.Clear();
            list.Add(new DrawCommand(this, IdKey.AlignLeftCommand, Noop));
            list.Add(new DrawCommand(this, IdKey.AlignCenterCommand, Item.Selector.AlignVert));
            list.Add(new DrawCommand(this, IdKey.AlignRightCommand, Noop));
        }
        private void Noop() { }
    }
}
