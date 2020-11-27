
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6F3_Flip : ItemModelOf<GraphModel>
    {
        internal Model_6F3_Flip(ItemModel owner, GraphModel item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6F3_Flip;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            var pageModel = GetPageModel();
            list.Clear();
            list.Add(new DrawCommand(this, IdKey.FlipVertCommand, Item.Selector.FlipVert));
            list.Add(new DrawCommand(this, IdKey.FlipHorzCommand, Item.Selector.FlipHorz));
        }
    }
}
