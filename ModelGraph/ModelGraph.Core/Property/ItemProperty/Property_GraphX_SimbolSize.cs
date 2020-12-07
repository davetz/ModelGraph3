﻿
namespace ModelGraph.Core
{
    public class Property_GraphX_SymbolSize : PropertyOf<GraphX, int>
    {
        internal override IdKey IdKey => IdKey.GraphSymbolSizeProperty;

        internal Property_GraphX_SymbolSize(PropertyRoot owner) : base(owner)
        {
            Value = new Int32Value(this);
        }

        internal override int GetValue(Item item) => Cast(item).SymbolSize;
        internal override void SetValue(Item item, int val) => Cast(item).SymbolSize = (byte)val;
    }
}
