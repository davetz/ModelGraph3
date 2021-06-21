

namespace ModelGraph.Core
{
    public class Property_Shape_Rotation1 : PropertyOf<ShapeModel, byte>
    {
        internal override IdKey IdKey => IdKey.ShapeRotation1Property;

        internal Property_Shape_Rotation1(PropertyRoot owner) : base(owner)
        {
            Value = new ByteValue(this);
        }

        internal override byte GetValue(Item item) => Cast(item).Rotation1;
        internal override void SetValue(Item item, byte val) => Cast(item).Rotation1 = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
