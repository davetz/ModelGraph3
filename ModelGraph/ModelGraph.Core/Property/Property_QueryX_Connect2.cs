
namespace ModelGraph.Core
{
    public class Property_QueryX_Connect2 : EnumPropertyOf<QueryX>
    {
        internal override IdKey IdKey => IdKey.QueryXConnect2Property;

        internal Property_QueryX_Connect2(PropertyRoot owner) : base(owner, owner.DataRoot.Get<Enum_Connect>()) { }

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).PathParm.Target2;
        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).PathParm.Target2 = (Target)key;
    }
}
