

namespace ModelGraph.Core
{
    public class Property_Shape_Radius2 : PropertyOf<ShapeModel, byte>
    {
        internal override IdKey IdKey => IdKey.ShapeRadius2Property;

        internal Property_Shape_Radius2(PropertyRoot owner) : base(owner)
        {
            Value = new ByteValue(this);
        }

        internal override byte GetValue(Item item) => Cast(item).Radius2;
        internal override void SetValue(Item item, byte val) => Cast(item).Radius2 = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
