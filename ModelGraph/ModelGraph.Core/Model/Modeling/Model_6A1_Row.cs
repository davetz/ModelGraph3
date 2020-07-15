﻿
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6A1_Row : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal Model_6A1_Row(Model_6A4_Table owner, RowX item) : base(owner, item) { }
        private RowX RX => Item as RowX;
        internal override IdKey IdKey => IdKey.Model_6A1_Row;

        public override bool CanExpandLeft => true;
        public override bool CanDrag => true;

        #region Identity  =====================================================
        public override (string, string) GetKindNameId(Root root) => root.Get<Relation_Store_NameProperty>().TryGetChild(Item.Owner, out Property prop) ? (Item.Owner.GetNameId(root), prop.Value.GetString(Item)) : (Item.Owner.GetNameId(root), Item.GetIndexId());
        public override string GetSummaryId(Root root) => root.Get<Relation_Store_SummaryProperty>().TryGetChild(Item.Owner, out Property prop) ? prop.Value.GetString(Item) : string.Empty;
        #endregion

        public override void GetMenuCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => root.Get<ChangeRoot>().RemoveItem(Item)));
        }

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;
            var rx = RX;

            new Model_6B1_ColumnList(this, rx);
            new Model_6B2_ComputeList(this, rx);
            new Model_6B3_ChildRelationList(this, rx);
            new Model_6B4_ParentRelationList(this, rx);
            return true;
        }
    }
}
