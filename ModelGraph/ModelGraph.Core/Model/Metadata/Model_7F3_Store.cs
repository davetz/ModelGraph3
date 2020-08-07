using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_7F3_Store : List2ModelOf<Store, Item>
    {
        internal Model_7F3_Store(Model_7F1_PrimeStore owner, Store item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_7F3_Store;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => Item.Count;
        protected override IList<Item> GetChildItems() => Item.GetItems();
        protected override void CreateChildModel(Item itm)
        {
            new Model_7F2_Item(this, itm);
        }
        #endregion
    }
}
