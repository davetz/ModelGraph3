
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class TableListModel_647 : ListModelOf<TableX>
    {//============================================== In the ModelingRoot hierarchy  ==============
        internal TableListModel_647(ModelingRootModel_624 owner, TableXRoot item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.TableListModel_647;

        private TableXRoot TXR => Item as TableXRoot;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => TXR.Count;
        protected override IList<TableX> GetChildItems() => TXR.Items;
        protected override void CreateChildModel(TableX childItem)
        {
            new TableModel_6A4(this, childItem);
        }
        #endregion
    }
}
