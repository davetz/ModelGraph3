
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6A4_Table : List2ModelOf<TableX,  RowX>
    {
        internal Model_6A4_Table(Model_647_TableList owner, TableX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6A4_Table;

        private TableX TX => Item as TableX;

        #region Identity  =====================================================
        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);
        public override string GetSummaryId(Root root) => Item.GetSummaryId(root);
        #endregion

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => TX.Count;
        protected override IList<RowX> GetChildItems() => TX.Items;
        protected override void CreateChildModel(RowX childItem)
        {
            new Model_6A1_Row(this, childItem);
        }
        #endregion
    }
}
