﻿
namespace ModelGraph.Core
{
    public class Property_Shape_StrokeStyle : EnumPropertyOf<ShapeModel>
    {
        internal override IdKey IdKey => IdKey.ShapeStrokeStyleProperty;

        internal Property_Shape_StrokeStyle(PropertyRoot owner) : base(owner, owner.GetRoot().Get<Enum_StrokeStyle>()) { }

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).StrokeStyle;
        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).StrokeStyle = (StrokeStyle)key;
    }
}
