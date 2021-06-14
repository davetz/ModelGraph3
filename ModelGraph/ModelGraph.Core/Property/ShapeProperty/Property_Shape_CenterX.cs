

namespace ModelGraph.Core
{
    public class Property_Shape_CenterX : PropertyOf<ShapeModel, short>
    {
        internal override IdKey IdKey => IdKey.ShapeCenterXProperty;

        internal Property_Shape_CenterX(PropertyRoot owner) : base(owner)
        {
            Value = new Int16Value(this);
        }

        internal override short GetValue(Item item) => Cast(item).CenterX;
        internal override void SetValue(Item item, short val) => Cast(item).CenterX = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
