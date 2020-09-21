
namespace ModelGraph.Core
{
    public class Property_Shape_EndCap : EnumPropertyOf<SymbolModel>
    {
        internal override IdKey IdKey => IdKey.ShapeLineEndCapProperty;

        internal Property_Shape_EndCap(PropertyManager owner) : base(owner, owner.GetRoot().Get<Enum_CapStyle>()) { }

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).EndCapStyle;
        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).EndCapStyle = (CapStyle)key;
    }
}
