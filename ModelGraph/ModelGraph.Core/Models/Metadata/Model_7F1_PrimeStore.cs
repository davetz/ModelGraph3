using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_7F1_PrimeStore : List2ModelOf<Store, Item>
    {
        internal Model_7F1_PrimeStore(Model_7F0_Root owner, Store item) : base(owner, item) { }
        private Store ST => Item as Store;
        internal override IdKey IdKey => IdKey.Model_7F1_PrimeStore;
        public override string GetKindId() => string.Empty;

        public override bool CanDrag => true;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => ItemStore.Count;
        protected override IList<Item> GetChildItems() => ST.GetItems();
        protected override void CreateChildModel(Item itm)
        {
            if (itm is Store)
                new Model_7F3_Store(this, itm as Store);
            else if (itm is Relation rx)
                new Model_7F4_Relation(this, rx);
            else
                new Model_7F2_Item(this, itm);
        }
        #endregion

    }
}
