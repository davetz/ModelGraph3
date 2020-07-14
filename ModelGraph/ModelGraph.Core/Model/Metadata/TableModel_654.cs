
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class TableModel_654 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal TableModel_654(TableListModel_643 owner, TableX item) : base(owner, item) { }
        private TableX TX => Item as TableX;
        internal override IdKey IdKey => IdKey.TableModel_654;

        public override bool CanExpandLeft => true;
        public override bool CanExpandRight => true;
        public override bool CanDrag => true;

        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);
        public override string GetSummaryId(Root root) => Item.GetSummaryId(root);
        internal override string GetFilterSortId(Root root) => Item.GetNameId(root);

        public override void GetMenuCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => root.Get<ChangeRoot>().RemoveItem(Item)));
        }

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;
            var tx = TX;

            new ColumnListModel_661(this, tx);
            new ComputeListModel_666(this, tx);
            new ChildRelationListModel_662(this, tx);
            new ParentRelatationListModel_663(this, tx);
            new NamePropertyRelationModel_673(this, tx);
            new SummaryPropertyRelationModel_674(this, tx);
            return true;
        }

        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;

            new PropertyTextModel_617(this, Item, root.Get<Property_Item_Summary>());
            new PropertyTextModel_617(this, Item, root.Get<Property_Item_Name>());

            IsExpandedRight = true;
            return true;
        }
    }
}
