using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_643_TableList : List2ModelOf<TableXManager, TableX>
    {
        internal Model_643_TableList(Model_623_MetadataRoot owner, TableXManager item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_643_TableList;
        public override string GetNameId() => Item.Owner.GetNameId(IdKey);

        protected override int GetTotalCount() => Item.Count;
        protected override IList<TableX> GetChildItems() => Item.Items;
        protected override void CreateChildModel(TableX childItem)
        {
            new Model_654_Table(this, childItem);
        }

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => ItemCreated.Record(root, new TableX(Item, true))));
        }
    }
}
