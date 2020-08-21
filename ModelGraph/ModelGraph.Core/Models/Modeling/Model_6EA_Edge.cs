
namespace ModelGraph.Core
{
    public class Model_6EA_Edge : List1ModelOf<Edge>
    {
        internal Model_6EA_Edge(LineModel owner, Edge item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6EA_Edge;
        public override bool CanExpandRight => true;
        public override string GetNameId() => $"{Item.Node1.Item.GetNameId()} -->  {Item.Node2.Item.GetNameId()}";

        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            root.Get<Property_Edge_Facet1>().CreatePropertyModel(this, Item);
            root.Get<Property_Edge_Facet2>().CreatePropertyModel(this, Item);

            return true;
        }
    }
}
