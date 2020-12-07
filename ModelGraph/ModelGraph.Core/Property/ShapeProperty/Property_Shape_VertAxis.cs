

namespace ModelGraph.Core
{
    public class Property_Shape_VertAxis : PropertyOf<SymbolModel, byte>
    {
        internal override IdKey IdKey => IdKey.ShapeVertAxisProperty;

        internal Property_Shape_VertAxis(PropertyRoot owner) : base(owner)
        {
            Value = new ByteValue(this);
        }

        internal override byte GetValue(Item item) => Cast(item).VSize;
        internal override void SetValue(Item item, byte val) => Cast(item).VSize = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
