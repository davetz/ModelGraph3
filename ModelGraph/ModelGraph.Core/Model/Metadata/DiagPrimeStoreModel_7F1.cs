
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class DiagPrimeStoreModel_7F1 : ListModelOf<Item>
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal DiagPrimeStoreModel_7F1(DiagRootModel_7F0 owner, Store item) : base(owner, item) { }
        private Store ST => Item as Store;
        internal override IdKey IdKey => IdKey.DiagPrimeStoreModel_7F1;
        public override (string, string) GetKindNameId(Root root) => (string.Empty, Item.GetNameId(root));

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => ItemStore.Count;
        protected override IList<Item> GetChildItems() => ST.GetItems();
        protected override void CreateChildModel(Item itm)
        {
            if (itm is Store)
                new DiagStoreModel_7F3(this, itm);
            else if (itm is Relation rx)
                new DiagRelationModel_7F4(this, rx);
            else
                new DiagItemModel_7F2(this, itm);
        }
        #endregion

    }
}
