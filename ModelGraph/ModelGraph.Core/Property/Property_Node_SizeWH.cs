
namespace ModelGraph.Core
{
    public class Property_Node_SizeWH : PropertyOf<Node, int[]>
    {
        internal override IdKey IdKey => IdKey.NodeSizeWHProperty;

        internal Property_Node_SizeWH(PropertyRoot owner)
        {
            Owner = owner;
            Value = new Int32ArrayValue(this);

            owner.Add(this);
        }

        internal override int[] GetValue(Item item) => Cast(item).SizeWH;
        internal override void SetValue(Item item, int[] val) => Cast(item).SizeWH = val;
    }
}
