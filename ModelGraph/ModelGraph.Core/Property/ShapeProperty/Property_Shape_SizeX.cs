

namespace ModelGraph.Core
{
    public class Property_Shape_SizeX : PropertyOf<ShapeModel, ushort>
    {
        internal override IdKey IdKey => IdKey.ShapeSizeXProperty;

        internal Property_Shape_SizeX(PropertyRoot owner) : base(owner)
        {
            Value = new UInt16Value(this);
        }

        internal override ushort GetValue(Item item) => Cast(item).SizeX;
        internal override void SetValue(Item item, ushort val) => Cast(item).SizeX = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
