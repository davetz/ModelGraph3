

namespace ModelGraph.Core
{
    public class Property_Shape_MajorAxis : PropertyOf<SymbolModel, byte>
    {
        internal override IdKey IdKey => IdKey.ShapeMajorAxisProperty;

        internal Property_Shape_MajorAxis(PropertyManager owner) : base(owner)
        {
            Value = new ByteValue(this);
        }

        internal override byte GetValue(Item item) => Cast(item).MajorAxis;
        internal override void SetValue(Item item, byte val) => Cast(item).MajorAxis = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
