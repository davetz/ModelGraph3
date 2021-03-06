﻿

namespace ModelGraph.Core
{
    public class Property_Shape_Factor1 : PropertyOf<ShapeModel, byte>
    {
        internal override IdKey IdKey => IdKey.ShapeFactor1Property;

        internal Property_Shape_Factor1(PropertyRoot owner) : base(owner)
        {
            Value = new ByteValue(this);
        }

        internal override byte GetValue(Item item) => Cast(item).Factor1;
        internal override void SetValue(Item item, byte val) => Cast(item).Factor1 = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
