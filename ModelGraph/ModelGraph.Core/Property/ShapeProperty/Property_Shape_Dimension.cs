

namespace ModelGraph.Core
{
    public class Property_Shape_Dimension : PropertyOf<SymbolModel, byte>
    {
        internal override IdKey IdKey => IdKey.ShapeDimensionProperty;

        internal Property_Shape_Dimension(PropertyRoot owner) : base(owner)
        {
            Value = new ByteValue(this);
        }

        internal override byte GetValue(Item item) => Cast(item).Dimension;
        internal override void SetValue(Item item, byte val) => Cast(item).Dimension = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
