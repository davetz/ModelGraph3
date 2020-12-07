
namespace ModelGraph.Core
{
    public class Property_QueryX_DashStyle : EnumPropertyOf<QueryX>
    {
        internal override IdKey IdKey => IdKey.QueryXDashStyleProperty;

        internal Property_QueryX_DashStyle(PropertyRoot owner) : base(owner, owner.GetRoot().Get<Enum_StrokeStyle>()) { }

        internal override int GetItemPropertyValue(Item item) => (Cast(item).PathParm is null) ? 0 : (int)Cast(item).PathParm.DashStyle;
        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).PathParm.DashStyle = (DashStyle)key;
    }
}
