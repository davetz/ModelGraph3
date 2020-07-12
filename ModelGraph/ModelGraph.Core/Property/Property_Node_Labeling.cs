
namespace ModelGraph.Core
{
    public class Property_Node_Labeling : EnumPropertyOf<Node>
    {
        internal override IdKey IdKey => IdKey.NodeLabelingProperty;

        internal Property_Node_Labeling(PropertyRoot owner) : base(owner, owner.DataRoot.Get<Enum_Labeling>()) { }

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).Labeling;
        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).Labeling = (Labeling)key;
    }
}
