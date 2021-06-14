

namespace ModelGraph.Core
{
    public class Property_Shape_ExtentSouth : PropertyOf<ShapeModel, short>
    {
        internal override IdKey IdKey => IdKey.ShapeExtentSouthProperty;

        internal Property_Shape_ExtentSouth(PropertyRoot owner) : base(owner)
        {
            Value = new Int16Value(this);
        }

        internal override short GetValue(Item item) => Cast(item).ExtentSouth;
        internal override void SetValue(Item item, short val) => Cast(item).ExtentSouth = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
