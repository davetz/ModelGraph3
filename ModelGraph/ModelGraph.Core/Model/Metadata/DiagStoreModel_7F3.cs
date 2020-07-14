
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class DiagStoreModel_7F3 : ListModelOf<Item>
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal DiagStoreModel_7F3(DiagPrimeStoreModel_7F1 owner, Item item) : base(owner, item) { }
        private Store ST => Item as Store;
        internal override IdKey IdKey => IdKey.DiagStoreModel_7F3;

        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => ST.Count;
        protected override IList<Item> GetChildItems() => ST.GetItems();
        protected override void CreateChildModel(Item itm)
        {
            new DiagItemModel_7F2(this, itm);
        }
        #endregion
    }
}
