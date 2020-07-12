
namespace ModelGraph.Core
{
    public class Property_Node_Aspect : EnumPropertyOf<Node>
    {
        internal override IdKey IdKey => IdKey.NodeOrientationProperty;

        internal Property_Node_Aspect(PropertyRoot owner) : base(owner, owner.DataRoot.Get<Enum_Aspect>()) { }

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).Aspect;

        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).Aspect = (Aspect)key;
    }
}
