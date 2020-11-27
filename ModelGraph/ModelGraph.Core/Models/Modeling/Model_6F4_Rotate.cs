
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6F4_Rotate : ItemModelOf<GraphModel>
    {
        internal Model_6F4_Rotate(ItemModel owner, GraphModel item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6F4_Rotate;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            var pageModel = GetPageModel();
            list.Clear();
            list.Add(new DrawCommand(this, IdKey.Rotate90LeftCommand, Item.Selector.RotateLeft90));
            list.Add(new DrawCommand(this, IdKey.Rotate45LeftCommand, Item.Selector.RotateLeft45));
            list.Add(new DrawCommand(this, IdKey.Rotate45RightCommand, Item.Selector.RotateRight45));
            list.Add(new DrawCommand(this, IdKey.Rotate90RightCommand, Item.Selector.RotateRight90));
        }
    }
}
