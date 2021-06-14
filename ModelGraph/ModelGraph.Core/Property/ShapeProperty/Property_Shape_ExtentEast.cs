

namespace ModelGraph.Core
{
    public class Property_Shape_ExtentEast : PropertyOf<ShapeModel, short>
    {
        internal override IdKey IdKey => IdKey.ShapeExtentEastProperty;

        internal Property_Shape_ExtentEast(PropertyRoot owner) : base(owner)
        {
            Value = new Int16Value(this);
        }

        internal override short GetValue(Item item) => Cast(item).ExtentEast;
        internal override void SetValue(Item item, short val) => Cast(item).ExtentEast = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
