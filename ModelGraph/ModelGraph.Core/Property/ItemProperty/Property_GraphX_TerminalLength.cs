
namespace ModelGraph.Core
{
    public class Property_GraphX_TerminalLength : PropertyOf<GraphX, int>
    {
        internal override IdKey IdKey => IdKey.GraphTerminalLengthProperty;

        internal Property_GraphX_TerminalLength(PropertyManager owner) : base(owner)
        {
            Value = new Int32Value(this);
        }

        internal override int GetValue(Item item) => Cast(item).TerminalLength;
        internal override void SetValue(Item item, int val) => Cast(item).TerminalLength = (byte)val;
    }
}
