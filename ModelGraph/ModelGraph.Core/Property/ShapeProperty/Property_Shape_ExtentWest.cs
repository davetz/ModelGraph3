

namespace ModelGraph.Core
{
    public class Property_Shape_ExtentWest : PropertyOf<ShapeModel, short>
    {
        internal override IdKey IdKey => IdKey.ShapeExtentWestProperty;

        internal Property_Shape_ExtentWest(PropertyRoot owner) : base(owner)
        {
            Value = new Int16Value(this);
        }

        internal override short GetValue(Item item) => Cast(item).ExtentWest;
        internal override void SetValue(Item item, short val) => Cast(item).ExtentWest = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
