﻿
namespace ModelGraph.Core
{
    public class Property_Shape_LineStyle : EnumPropertyOf<SymbolModel>
    {
        internal override IdKey IdKey => IdKey.ShapeLineStyleProperty;

        internal Property_Shape_LineStyle(PropertyManager owner) : base(owner, owner.GetRoot().Get<Enum_StrokeStyle>()) { }

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).LineStyle;
        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).LineStyle = (StrokeStyle)key;
    }
}