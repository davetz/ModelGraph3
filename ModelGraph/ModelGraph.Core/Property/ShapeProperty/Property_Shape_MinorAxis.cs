

namespace ModelGraph.Core
{
    public class Property_Shape_MinorAxis : PropertyOf<SymbolModel, byte>
    {
        internal override IdKey IdKey => IdKey.ShapeMinorAxisProperty;

        internal Property_Shape_MinorAxis(PropertyManager owner) : base(owner)
        {
            Value = new ByteValue(this);
        }

        internal override byte GetValue(Item item) => Cast(item).MinorAxis;
        internal override void SetValue(Item item, byte val) => Cast(item).MinorAxis = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
