using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_644_GraphList : List2ModelOf<GraphXManager, GraphX>
    {
        internal Model_644_GraphList(Model_623_MetadataRoot owner, GraphXManager item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_644_GraphList;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override string GetKindId() => string.Empty;

        protected override void CreateChildModel(GraphX gx)
        {
            new Model_655_Graph(this, gx);
        }

        protected override IList<GraphX> GetChildItems() => Item.Items;

        protected override int GetTotalCount() => Item.Count;

        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => ItemCreated.Record(root, new GraphX(Item, true))));
        }

    }
}
