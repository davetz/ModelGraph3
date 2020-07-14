namespace ModelGraph.Core
{
    public class MetadataRootModel_623 : LineModel
    {
        internal MetadataRootModel_623(RootModel_612 owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.MetadataRootModel_623;
        public override bool CanExpandLeft => true;

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;

            new ViewListModel_631(this, root.Get<ViewXRoot>());
            new EnumListModel_624(this, root.Get<EnumXRoot>());
            new TableListModel_643(this, root.Get<TableXRoot>());
            new GraphListModel_644(this, root.Get<GraphXRoot>());
            new DiagRootModel_7F0(this, root);

            return true;
        }
    }
}
