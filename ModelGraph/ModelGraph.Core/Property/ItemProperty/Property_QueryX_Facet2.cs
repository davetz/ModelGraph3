
namespace ModelGraph.Core
{
    public class Property_QueryX_Facet2 : EnumPropertyOf<QueryX>
    {
        internal override IdKey IdKey => IdKey.QueryXFacet2Property;

        internal Property_QueryX_Facet2(PropertyManager owner) : base(owner, owner.GetRoot().Get<Enum_Facet>()) { }

        internal override int GetItemPropertyValue(Item item) => (Cast(item).PathParm is null) ? 0 : (int)Cast(item).PathParm.Facet2;
        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).PathParm.Facet2 = (Facet)key;
    }
}
