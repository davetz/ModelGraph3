

namespace ModelGraph.Core
{
    public class Property_Shape_HorzAxis : PropertyOf<SymbolModel, byte>
    {
        internal override IdKey IdKey => IdKey.ShapeHorzAxisProperty;

        internal Property_Shape_HorzAxis(PropertyManager owner) : base(owner)
        {
            Value = new ByteValue(this);
        }

        internal override byte GetValue(Item item) => Cast(item).HorzAxis;
        internal override void SetValue(Item item, byte val) => Cast(item).HorzAxis = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
