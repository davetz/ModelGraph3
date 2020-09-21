
namespace ModelGraph.Core
{
    public class Property_Shape_StartCap : EnumPropertyOf<SymbolModel>
    {
        internal override IdKey IdKey => IdKey.ShapeLineStartCapProperty;

        internal Property_Shape_StartCap(PropertyManager owner) : base(owner, owner.GetRoot().Get<Enum_CapStyle>()) { }

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).StartCapStyle;
        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).StartCapStyle = (CapStyle)key;
    }
}
