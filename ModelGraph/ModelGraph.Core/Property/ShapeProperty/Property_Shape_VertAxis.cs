﻿

namespace ModelGraph.Core
{
    public class Property_Shape_VertAxis : PropertyOf<SymbolModel, byte>
    {
        internal override IdKey IdKey => IdKey.ShapeVertAxisProperty;

        internal Property_Shape_VertAxis(PropertyManager owner) : base(owner)
        {
            Value = new ByteValue(this);
        }

        internal override byte GetValue(Item item) => Cast(item).VertAxis;
        internal override void SetValue(Item item, byte val) => Cast(item).VertAxis = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}