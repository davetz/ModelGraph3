
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6F5_CutCopy : ItemModelOf<DrawModel>
    {
        internal Model_6F5_CutCopy(ItemModel owner, DrawModel item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6F5_CutCopy;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            var pageModel = GetPageModel();
            list.Clear();
            list.Add(new DrawCommand(this, IdKey.CutCommand, Item.Cut));
            list.Add(new DrawCommand(this, IdKey.CopyCommand, Item.Copy));
        }
    }
}
