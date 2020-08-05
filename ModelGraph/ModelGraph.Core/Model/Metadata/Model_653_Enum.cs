﻿
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_653_Enum : List1ModelOf<EnumX>
    {
        internal Model_653_Enum(Model_624_EnumList owner, EnumX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_653_Enum;

        public override bool CanExpandLeft => true;
        public override bool CanExpandRight => true;

        public override (string, string) GetKindNameId() => Item.GetKindNameId();
        public override string GetSummaryId() => Item.GetSummaryId();

        public override void GetMenuCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => root.Get<ChangeRoot>().RemoveItem(Item)));
        }

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;

            new Model_664_PairList(this, Item);
            new Model_665_ColumnList(this, Item);
            return true;
        }

        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            root.Get<Property_Item_Name>().CreatePropertyModel(this, Item);
            root.Get<Property_Item_Summary>().CreatePropertyModel(this, Item);

            return true;
        }
    }
}
