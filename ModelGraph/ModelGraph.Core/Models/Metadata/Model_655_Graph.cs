namespace ModelGraph.Core
{
    public class Model_655_Graph : List1ModelOf<GraphX>
    {
        internal Model_655_Graph(ItemModel owner, GraphX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_655_Graph;

        public override bool CanExpandLeft => true;
        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;

            new Model_682_GraphRootList(this, Item);
            new Model_683_GraphNodeList(this, Item);
            new Model_645_GraphSymbolList(this, Item);
            new Model_681_GraphColoring(this, Item);
            return true;
        }

        public override bool CanExpandRight => true;
        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            root.Get<Property_Item_Name>().CreatePropertyModel(this, Item);
            root.Get<Property_Item_Summary>().CreatePropertyModel(this, Item);
            root.Get<Property_GraphX_TerminalLength>().CreatePropertyModel(this, Item);
            root.Get<Property_GraphX_TerminalSpacing>().CreatePropertyModel(this, Item);
            root.Get<Property_GraphX_TerminalStretch>().CreatePropertyModel(this, Item);
            root.Get<Property_GraphX_SymbolSize>().CreatePropertyModel(this, Item);
            return true;
        }
    }
}
