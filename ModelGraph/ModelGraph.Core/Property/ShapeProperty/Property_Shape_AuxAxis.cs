

namespace ModelGraph.Core
{
    public class Property_Shape_AuxAxis : PropertyOf<SymbolModel, byte>
    {
        internal override IdKey IdKey => IdKey.ShapeAuxAxisProperty;

        internal Property_Shape_AuxAxis(PropertyManager owner) : base(owner)
        {
            Value = new ByteValue(this);
        }

        internal override byte GetValue(Item item) => Cast(item).AuxAxis;
        internal override void SetValue(Item item, byte val) => Cast(item).AuxAxis = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
