
namespace ModelGraph.Core
{
    public class Property_GraphX_TerminalStretch : PropertyOf<GraphX, int>
    {
        internal override IdKey IdKey => IdKey.GraphTerminalStretchProperty;

        internal Property_GraphX_TerminalStretch(PropertyRoot owner)
        {
            Owner = owner;
            Value = new Int32Value(this);

            owner.Add(this);
        }

        internal override int GetValue(Item item) => Cast(item).TerminalSkew;
        internal override void SetValue(Item item, int val) => Cast(item).TerminalSkew = (byte)val;
    }
}
