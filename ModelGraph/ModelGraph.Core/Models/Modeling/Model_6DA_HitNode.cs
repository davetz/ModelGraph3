using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6DA_HitNode : List1ModelOf<Node>
    {
        internal Model_6DA_HitNode(Node item) : base(null, item) { }
        internal override IdKey IdKey => IdKey.Model_6DA_HitNode;
        public override string GetNameId() => Item.Item.GetNameId();

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
