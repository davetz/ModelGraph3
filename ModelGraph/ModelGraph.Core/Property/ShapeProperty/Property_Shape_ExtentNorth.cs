

namespace ModelGraph.Core
{
    public class Property_Shape_ExtentNorth : PropertyOf<ShapeModel, short>
    {
        internal override IdKey IdKey => IdKey.ShapeExtentNorthProperty;

        internal Property_Shape_ExtentNorth(PropertyRoot owner) : base(owner)
        {
            Value = new Int16Value(this);
        }

        internal override short GetValue(Item item) => Cast(item).ExtentNorth;
        internal override void SetValue(Item item, short val) => Cast(item).ExtentNorth = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
