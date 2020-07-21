using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class Model_622_ChangeRoot : ListModelOf<ChangeRoot,ChangeSet>
    {
        internal Model_622_ChangeRoot(Model_612_Root owner, ChangeRoot item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_622_ChangeRoot;

        protected override int GetTotalCount() => Item.Count;

        protected override IList<ChangeSet> GetChildItems() => Item.Items;

        protected override void CreateChildModel(ChangeSet cs)
        {
            new Model_628_ChangeSet(this, cs);
        }
    }
}
