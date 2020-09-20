
using System.Collections;

namespace ModelGraph.Core
{
    public class Property_Shape_StrokeWidth : PropertyOf<SymbolModel, byte>
    {
        internal override IdKey IdKey => IdKey.ShapeStrokeWidthProperty;

        internal Property_Shape_StrokeWidth(PropertyManager owner) : base(owner)
        {
            Value = new ByteValue(this);
        }

        internal override byte GetValue(Item item) => Cast(item).StrokeWidth;
        internal override void SetValue(Item item, byte val) => Cast(item).StrokeWidth = val;
    }
}
