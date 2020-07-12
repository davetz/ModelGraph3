namespace ModelGraph.Core
{
    public class RootModel_612 : LineModel
    {
        internal override IdKey IdKey => IdKey.RootModel_612;
        public override bool CanExpandLeft => true;

        internal RootModel_612(LineModel owner, Root root) : base(owner, root) 
        {
            new RootParamModel_620(this, Item);
            new ErrorRootModel_621(this, Item);
            new ChangeRootModel_622(this, root.Get<ChangeRoot>());
            new MetadataRootModel_623(this, Item);
            new ModelingRootModel_624(this, Item);

            IsExpandedLeft = true;
        }
    }
}
