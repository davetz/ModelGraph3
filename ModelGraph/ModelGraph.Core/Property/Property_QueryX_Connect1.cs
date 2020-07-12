
namespace ModelGraph.Core
{
    public class Property_QueryX_Connect1 : EnumPropertyOf<QueryX>
    {
        internal override IdKey IdKey => IdKey.QueryXConnect1Property;

        internal Property_QueryX_Connect1(PropertyRoot owner) : base(owner, owner.DataRoot.Get<Enum_Connect>()) { }

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).PathParm.Target1;
        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).PathParm.Target1 = (Target)key;
    }
}
