
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6F3_Flip : ItemModelOf<DrawModel>
    {
        internal Model_6F3_Flip(ItemModel owner, DrawModel item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6F3_Flip;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            var pageModel = GetPageModel();
            list.Clear();
            list.Add(new DrawCommand(this, IdKey.FlipVertCommand, Item.FlipVert));
            list.Add(new DrawCommand(this, IdKey.FlipHorzCommand, Item.FlipHorz));
        }
    }
}
