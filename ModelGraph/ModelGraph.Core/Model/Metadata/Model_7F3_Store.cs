
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_7F3_Store : ListModelOf<Item>
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal Model_7F3_Store(Model_7F1_PrimeStore owner, Item item) : base(owner, item) { }
        private Store ST => Item as Store;
        internal override IdKey IdKey => IdKey.Model_7F3_Store;

        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => ST.Count;
        protected override IList<Item> GetChildItems() => ST.GetItems();
        protected override void CreateChildModel(Item itm)
        {
            new Model_7F2_Item(this, itm);
        }
        #endregion
    }
}
