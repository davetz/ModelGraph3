
namespace ModelGraph.Core
{
    public class Property_QueryX_LineStyle : EnumPropertyOf<QueryX>
    {
        internal override IdKey IdKey => IdKey.QueryXLineStyleProperty;

        internal Property_QueryX_LineStyle(PropertyManager owner) : base(owner, owner.GetRoot().Get<Enum_LineStyle>()) { }

        internal override int GetItemPropertyValue(Item item) => (Cast(item).PathParm is null) ? 0 : (int)Cast(item).PathParm.LineStyle;
        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).PathParm.LineStyle = (LineStyle)key;
    }
}
