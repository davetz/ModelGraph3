
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class TableListModel_643 : ListModelOf<TableX>
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal TableListModel_643(MetadataRootModel_623 owner, TableXRoot item) : base(owner, item) { }
        private TableXRoot TXR => Item as TableXRoot;
        internal override IdKey IdKey => IdKey.TableListModel_643;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => TXR.Count;
        protected override IList<TableX> GetChildItems() => TXR.Items;
        protected override void CreateChildModel(TableX childItem)
        {
            new TableModel_654(this, childItem);
        }
        #endregion

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => ItemCreated.Record(root, new TableX(Item as TableXRoot, true))));
        }
    }
}
