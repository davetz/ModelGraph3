
namespace ModelGraph.Core
{
    public class Property_Node_Resizing : EnumPropertyOf<Node>
    {
        internal override IdKey IdKey => IdKey.NodeResizingProperty;

        internal Property_Node_Resizing(PropertyRoot owner) : base(owner, owner.DataRoot.Get<Enum_Resizing>()) { }

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).Sizing;
        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).Sizing = (Sizing)key;
    }
}
