using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_643_TableList : ListModelOf<TableX>
    {
        internal Model_643_TableList(Model_623_MetadataRoot owner, TableXRoot item) : base(owner, item) { }
        private TableXRoot TXR => Item as TableXRoot;
        internal override IdKey IdKey => IdKey.Model_643_TableList;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => TXR.Count;
        protected override IList<TableX> GetChildItems() => TXR.Items;
        protected override void CreateChildModel(TableX childItem)
        {
            new Model_654_Table(this, childItem);
        }
        #endregion

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => ItemCreated.Record(root, new TableX(Item as TableXRoot, true))));
        }
    }
}
