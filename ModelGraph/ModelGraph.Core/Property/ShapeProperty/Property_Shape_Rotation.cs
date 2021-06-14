

namespace ModelGraph.Core
{
    public class Property_Shape_Rotation : PropertyOf<ShapeModel, byte>
    {
        internal override IdKey IdKey => IdKey.ShapeRotationProperty;

        internal Property_Shape_Rotation(PropertyRoot owner) : base(owner)
        {
            Value = new ByteValue(this);
        }

        internal override byte GetValue(Item item) => Cast(item).Rotation;
        internal override void SetValue(Item item, byte val) => Cast(item).Rotation = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
