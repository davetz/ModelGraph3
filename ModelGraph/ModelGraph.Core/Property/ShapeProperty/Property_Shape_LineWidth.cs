﻿
using System.Collections;

namespace ModelGraph.Core
{
    public class Property_Shape_LineWidth : PropertyOf<SymbolModel, byte>
    {
        internal override IdKey IdKey => IdKey.ShapeLineWidthProperty;

        internal Property_Shape_LineWidth(PropertyManager owner) : base(owner)
        {
            Value = new ByteValue(this);
        }

        internal override byte GetValue(Item item) => Cast(item).LineWidth;
        internal override void SetValue(Item item, byte val) => Cast(item).LineWidth = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}