﻿
namespace ModelGraph.Core
{
    public class Property_GraphX_TerminalLength : PropertyOf<GraphX, int>
    {
        internal override IdKey IdKey => IdKey.GraphTerminalLengthProperty;

        internal Property_GraphX_TerminalLength(PropertyRoot owner) : base(owner)
        {
            Value = new Int32Value(this);
        }

        internal override int GetValue(Item item) => Cast(item).TerminalLength;
        internal override void SetValue(Item item, int val) => Cast(item).TerminalLength = (byte)val;
    }
}
