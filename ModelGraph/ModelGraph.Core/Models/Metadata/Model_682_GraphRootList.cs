using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class Model_682_GraphRootList : List2ModelOf<GraphX, Store>
    {
        internal Model_682_GraphRootList(Model_655_Graph owner, GraphX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_682_GraphRootList;

        public override string GetKindId() => string.Empty;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override string GetSummaryId() => Root.GetSummaryId(IdKey);

        protected override int GetTotalCount() => Item.Owner.GetTotalCount(this);
        protected override IList<Store> GetChildItems() => Item.Owner.TryGetChildItems(this, out _store_QueryX) ? _store_QueryX.Keys.ToArray() : new Store[0];
        private Dictionary<Store, QueryX> _store_QueryX;
        protected override void CreateChildModel(Store st) => new Model_691_GraphRoot(this, st, Item, _store_QueryX[st]);
        internal override DropAction ModelDrop(Root root, LineModel dropModel, bool doDrop) => Item.Owner.ModelDrop(this, dropModel, doDrop) ? DropAction.Link : DropAction.None;
    }
}
