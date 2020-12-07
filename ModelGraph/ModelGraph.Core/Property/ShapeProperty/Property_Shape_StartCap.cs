
namespace ModelGraph.Core
{
    public class Property_Shape_StartCap : EnumPropertyOf<SymbolModel>
    {
        internal override IdKey IdKey => IdKey.ShapeStartCapProperty;

        internal Property_Shape_StartCap(PropertyRoot owner) : base(owner, owner.GetRoot().Get<Enum_CapStyle>()) { }

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).StartCap;
        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).StartCap = (CapStyle)key;
    }
}
