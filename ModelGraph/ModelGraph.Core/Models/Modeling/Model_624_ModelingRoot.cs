﻿
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_624_ModelingRoot : List1ModelOf<Root>
    {
        internal Model_624_ModelingRoot(ItemModel owner, Root item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_624_ModelingRoot;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override bool CanExpandLeft => true;

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;

            new Model_63A_ViewList(this, root.Get<ViewXRoot>());
            new Model_647_TableList(this, root.Get<TableXRoot>());
            new Model_648_GraphList(this, root.Get<GraphXRoot>());

            return true;
        }
        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            var pageModel = GetPageModel();
            list.Clear();
            list.Add(new NewViewCommand(this, () => { pageModel.NewView((p) => { new TreeModel(p, (m) => { new Model_624_ModelingRoot(m, Item); }); }, ControlType.PartialTree); }));
        }
    }
}
