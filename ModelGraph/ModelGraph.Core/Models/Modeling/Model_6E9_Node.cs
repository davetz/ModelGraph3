using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6E9_Node : List2ModelOf<Node, Edge>
    {
        internal Model_6E9_Node(Model_6E2_NodeList owner, Node item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6E9_Node;
        public override string GetNameId() => Item.Item.GetNameId();
        protected override int GetTotalCount() => Item.EdgeCount;
        protected override IList<Edge> GetChildItems() => Item.EdgeList;

        protected override void CreateChildModel(Edge childItem) => new Model_6EA_Edge(this, childItem);
        public override bool CanExpandRight => true;
        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            root.Get<Property_Node_CenterXY>().CreatePropertyModel(this, Item);
            root.Get<Property_Node_SizeWH>().CreatePropertyModel(this, Item);
            root.Get<Property_Node_Aspect>().CreatePropertyModel(this, Item);
            root.Get<Property_Node_Labeling>().CreatePropertyModel(this, Item);
            root.Get<Property_Node_Resizing>().CreatePropertyModel(this, Item);
            root.Get<Property_Node_BarWidth>().CreatePropertyModel(this, Item);

            return true;
        }

    }
}
