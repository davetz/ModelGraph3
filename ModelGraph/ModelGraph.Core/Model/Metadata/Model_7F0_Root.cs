
namespace ModelGraph.Core
{
    public class Model_7F0_Root : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal Model_7F0_Root(Model_623_MetadataRoot owner, Root root) : base(owner, root) { }
        private Root RT => Item as Root;
        internal override IdKey IdKey => IdKey.Model_7F0_Root;

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
                new Model_7F1_PrimeStore(this, itm);
            }

            return true;
        }
    }
}
