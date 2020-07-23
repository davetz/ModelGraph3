
namespace ModelGraph.Core
{
    public class Model_612_Root : LineModelOf<Root>
    {
        internal override IdKey IdKey => IdKey.Model_612_Root;
        public override bool CanExpandLeft => true;

        internal Model_612_Root(LineModel owner, Root root) : base(owner, root) 
        {
            new Model_620_RootParm(this, Item);
            new Model_621_ErrorRoot(this, root.Get<ErrorRoot>());
            new Model_622_ChangeRoot(this, root.Get<ChangeRoot>());
            new Model_623_MetadataRoot(this, Item);
            new Model_624_ModelingRoot(this, Item);

            IsExpandedLeft = true;
        }
    }
}
