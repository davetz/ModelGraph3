
namespace ModelGraph.Core
{
    public class DiagRootModel_7F0 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal DiagRootModel_7F0(MetadataRootModel_623 owner, Root root) : base(owner, root) { }
        private Root RT => Item as Root;
        internal override IdKey IdKey => IdKey.DiagRootModel_7F0;

        public override bool CanExpandLeft => TotalCount > 0;
        public override bool CanFilter => TotalCount > 1;
        public override bool CanSort => TotalCount > 1;
        public override int TotalCount => RT.PrimeStores.Length;


        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;

            IsExpandedLeft = true;

            foreach (var itm in RT.PrimeStores)
            {
                new DiagPrimeStoreModel_7F1(this, itm);
            }

            return true;
        }
    }
}
