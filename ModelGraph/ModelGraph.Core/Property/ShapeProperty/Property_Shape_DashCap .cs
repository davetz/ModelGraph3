
namespace ModelGraph.Core
{
    public class Property_Shape_DashCap : EnumPropertyOf<SymbolModel>
    {
        internal override IdKey IdKey => IdKey.ShapeLineDashCapProperty;

        internal Property_Shape_DashCap(PropertyManager owner) : base(owner, owner.GetRoot().Get<Enum_CapStyle>()) { }

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).DashCapStyle;
        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).DashCapStyle = (CapStyle)key;
    }
}
