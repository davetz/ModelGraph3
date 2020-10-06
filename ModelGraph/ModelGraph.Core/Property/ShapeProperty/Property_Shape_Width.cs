
using System.Collections;

namespace ModelGraph.Core
{
    public class Property_Shape_Width : PropertyOf<SymbolModel, byte>
    {
        internal override IdKey IdKey => IdKey.ShapeWidthProperty;

        internal Property_Shape_Width(PropertyManager owner) : base(owner)
        {
            Value = new ByteValue(this);
        }

        internal override byte GetValue(Item item) => Cast(item).StrokeWidth;
        internal override void SetValue(Item item, byte val) => Cast(item).StrokeWidth = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
