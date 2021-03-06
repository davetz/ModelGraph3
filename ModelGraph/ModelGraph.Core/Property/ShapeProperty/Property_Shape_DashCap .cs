﻿
namespace ModelGraph.Core
{
    public class Property_Shape_DashCap : EnumPropertyOf<ShapeModel>
    {
        internal override IdKey IdKey => IdKey.ShapeDashCapProperty;

        internal Property_Shape_DashCap(PropertyRoot owner) : base(owner, owner.GetRoot().Get<Enum_CapStyle>()) { }

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).DashCap;
        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).DashCap = (CapStyle)key;
    }
}
