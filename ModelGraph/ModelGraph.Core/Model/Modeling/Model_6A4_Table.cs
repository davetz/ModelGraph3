
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6A4_Table : List2ModelOf<TableX,  RowX>
    {
        internal Model_6A4_Table(Model_647_TableList owner, TableX item) : base(owner, item) { }

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.Model_6A4_Table;
        public override (string, string) GetKindNameId() => Item.GetKindNameId();
        public override string GetSummaryId() => Item.GetSummaryId();
        #endregion

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => Item.Count;
        protected override IList<RowX> GetChildItems() => Item.Items;
        protected override void CreateChildModel(RowX childItem)
        {
            new Model_6A1_Row(this, childItem);
        }
        #endregion

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => ItemCreated.Record(root, new RowX(Item, true))));
        }
    }
}
