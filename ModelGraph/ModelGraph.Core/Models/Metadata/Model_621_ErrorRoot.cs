
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_621_ErrorRoot : List2ModelOf<ErrorManager, Error>
    {
        internal Model_621_ErrorRoot(ItemModel owner, ErrorManager item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_621_ErrorRoot;

        protected override int GetTotalCount() => Item.Count;
        protected override IList<Error> GetChildItems() => Item.Items;
        protected override void CreateChildModel(Error childItem)
        {
            new Model_626_ErrorType(this, childItem);
        }
        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            var pageModel = GetPageModel();
            list.Clear();
            list.Add(new NewViewCommand(this, () => { pageModel.NewView((p) => { new TreeModel(p, (m) => { new Model_621_ErrorRoot(m, Item); }); }, ControlType.PartialTree); }));
        }
    }
}
