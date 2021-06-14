

namespace ModelGraph.Core
{
    public class Property_Shape_CenterY : PropertyOf<ShapeModel, short>
    {
        internal override IdKey IdKey => IdKey.ShapeCenterYProperty;

        internal Property_Shape_CenterY(PropertyRoot owner) : base(owner)
        {
            Value = new Int16Value(this);
        }

        internal override short GetValue(Item item) => Cast(item).CenterY;
        internal override void SetValue(Item item, short val) => Cast(item).CenterY = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
