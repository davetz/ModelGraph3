using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class Model_622_ChangeRoot : List2ModelOf<ChangeRoot, ChangeSet>
    {
        internal Model_622_ChangeRoot(Model_612_Root owner, ChangeRoot item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_622_ChangeRoot;

        public override string GetNameId() => Item.Owner.GetNameId(IdKey);


        protected override int GetTotalCount() => Item.Count;

        public override bool CanExpandAll => true;

        protected override IList<ChangeSet> GetChildItems() => Item.ItemsReversed;

        protected override void CreateChildModel(ChangeSet cs)
        {
            new Model_628_ChangeSet(this, cs);
        }
    }
}
