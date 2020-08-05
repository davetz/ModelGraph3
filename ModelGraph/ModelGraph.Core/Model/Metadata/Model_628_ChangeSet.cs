
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_628_ChangeSet : List2ModelOf<ChangeSet, ItemChange>
    {
        internal Model_628_ChangeSet(Model_622_ChangeRoot owner, ChangeSet item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_628_ChangeSet;

        public override (string, string) GetKindNameId() => (null, Item.GetNameId());

        protected override int GetTotalCount() => Item.Count;

        protected override IList<ItemChange> GetChildItems() => Item.Items;

        protected override void CreateChildModel(ItemChange ic)
        {
            new Model_629_ChangeItem(this, ic);
        }
    }
}
