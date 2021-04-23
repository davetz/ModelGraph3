

namespace ModelGraph.Core
{
    public class Property_Shape_CentAxis : PropertyOf<ShapeModel, byte>
    {
        internal override IdKey IdKey => IdKey.ShapeCentAxisProperty;

        internal Property_Shape_CentAxis(PropertyRoot owner) : base(owner)
        {
            Value = new ByteValue(this);
        }

        internal override byte GetValue(Item item) => Cast(item).Size;
        internal override void SetValue(Item item, byte val) => Cast(item).Size = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
