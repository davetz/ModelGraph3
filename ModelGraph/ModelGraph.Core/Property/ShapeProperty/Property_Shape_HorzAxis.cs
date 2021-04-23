

namespace ModelGraph.Core
{
    public class Property_Shape_HorzAxis : PropertyOf<ShapeModel, byte>
    {
        internal override IdKey IdKey => IdKey.ShapeHorzAxisProperty;

        internal Property_Shape_HorzAxis(PropertyRoot owner) : base(owner)
        {
            Value = new ByteValue(this);
        }

        internal override byte GetValue(Item item) => Cast(item).HSize;
        internal override void SetValue(Item item, byte val) => Cast(item).HSize = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
