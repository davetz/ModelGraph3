﻿
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_656_Symbol : List1ModelOf<SymbolX>
    {
        internal Model_656_Symbol(Model_645_GraphSymbolList owner, SymbolX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_656_Symbol;
        public override bool CanExpandRight => true;
        public override bool CanDrag => true;

        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            root.Get<Property_Item_Name>().CreatePropertyModel(this, Item);
            root.Get<Property_Item_Summary>().CreatePropertyModel(this, Item);

            return true;
        }
        public override bool CanReorderItems => true;
        public override bool ReorderItems(Root root, ItemModel dropModel) => (dropModel is Model_656_Symbol m) && ReorderStoreItems(root, Item.Owner, Item, m.Item);
        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            var pageModel = GetPageModel();
            list.Clear();
            list.Add(new EditCommand(this, () => { pageModel.NewView( (p) => { new ShapeModel(p, root, Item); }, ControlType.ComplexDraw); }));
        }
        public override void GetMenuCommands(Root root, List<ItemCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => root.Get<ChangeManager>().RemoveItem(Item)));
        }
    }
}
