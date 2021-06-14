

namespace ModelGraph.Core
{
    public class Property_Shape_SizeY : PropertyOf<ShapeModel, ushort>
    {
        internal override IdKey IdKey => IdKey.ShapeSizeYProperty;

        internal Property_Shape_SizeY(PropertyRoot owner) : base(owner)
        {
            Value = new UInt16Value(this);
        }

        internal override ushort GetValue(Item item) => Cast(item).SizeY;
        internal override void SetValue(Item item, ushort val) => Cast(item).SizeY = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
