

namespace ModelGraph.Core
{
    public class Property_Shape_MajorAxis : PropertyOf<ShapeModel, byte>
    {
        internal override IdKey IdKey => IdKey.ShapeMajorAxisProperty;

        internal Property_Shape_MajorAxis(PropertyRoot owner) : base(owner)
        {
            Value = new ByteValue(this);
        }

        internal override byte GetValue(Item item) => Cast(item).Radius1;
        internal override void SetValue(Item item, byte val) => Cast(item).Radius1 = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
