
namespace ModelGraph.Core
{
    public class Property_Edge_Facet1 : EnumPropertyOf<Edge>
    {
        internal override IdKey IdKey => IdKey.EdgeFace1Property;

        internal Property_Edge_Facet1(PropertyRoot owner) : base(owner, owner.DataRoot.Get<Enum_Facet>()) { }

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).Facet1;
        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).Facet1 = (Facet)key;
    }
}
