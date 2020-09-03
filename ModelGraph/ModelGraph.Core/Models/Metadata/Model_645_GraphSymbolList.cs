using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_645_GraphSymbolList : List2ModelOf<GraphX, SymbolX>
    {
        internal Model_645_GraphSymbolList(Model_655_Graph owner, GraphX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_645_GraphSymbolList;

        public override string GetKindId() => string.Empty;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override string GetSummaryId() => Root.GetSummaryId(IdKey);

        protected override int GetTotalCount() => Item.Owner.GetTotalCount(this);
        protected override IList<SymbolX> GetChildItems() => Item.Owner.GetChildItems(this);
        protected override void CreateChildModel(SymbolX childItem) => new Model_656_Symbol(this, childItem);

        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => Item.Owner.CreateNewSymbol(this)));
        }
    }
}
