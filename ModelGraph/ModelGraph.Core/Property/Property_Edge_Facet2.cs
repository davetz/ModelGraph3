
namespace ModelGraph.Core
{
    public class Property_Edge_Facet2 : EnumPropertyOf<Edge>
    {
        internal override IdKey IdKey => IdKey.EdgeFace2Property;

        internal Property_Edge_Facet2(PropertyRoot owner) : base(owner, owner.DataRoot.Get<Enum_Facet>()) { }

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).Facet2;

        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).Facet2 = (Facet)key;
    }
}
