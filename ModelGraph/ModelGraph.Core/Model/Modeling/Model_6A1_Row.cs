﻿using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6A1_Row : List1ModelOf<RowX>
    {
        internal Model_6A1_Row(LineModel owner, RowX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6A1_Row;

        public override bool CanDrag => true;

        #region Identity  =====================================================
        public override (string, string) GetKindNameId() => (null, Item.GetNameId());
        public override string GetSummaryId() => Item.GetSummaryId();
        #endregion

        public override void GetMenuCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => root.Get<ChangeRoot>().RemoveItem(Item)));
        }

        public override bool CanExpandLeft => true;
        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;

            new Model_6B1_ColumnList(this, Item);
            new Model_6B2_ComputeList(this, Item);
            new Model_6B3_ChildRelationList(this, Item);
            new Model_6B4_ParentRelationList(this, Item);
            return true;
        }

        public override bool CanExpandRight => Item.Owner.Owner.GetChoiceProperties(Item.Owner).Count > 0;
        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            var list = Item.Owner.Owner.GetChoiceProperties(Item.Owner);
            foreach (var cx in list)
            {
                cx.CreatePropertyModel(this, Item);
            }
            return true;
        }
    }
}
