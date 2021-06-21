
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6F1_VertAlign : ItemModelOf<DrawModel>
    {
        internal Model_6F1_VertAlign(ItemModel owner, DrawModel item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6F1_VertAlign;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            var pageModel = GetPageModel();
            list.Clear();
            list.Add(new DrawCommand(this, IdKey.AlignTopCommand, noop));
            list.Add(new DrawCommand(this, IdKey.AlignCenterCommand, Item.AlignVertCenter));
            list.Add(new DrawCommand(this, IdKey.AlignBottomCommand, noop));
        }
        private void noop() { }
    }
}
