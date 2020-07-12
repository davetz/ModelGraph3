
namespace ModelGraph.Core
{
    public class Property_GraphX_TerminalSpacing : PropertyOf<GraphX, int>
    {
        internal override IdKey IdKey => IdKey.GraphTerminalSpacingProperty;

        internal Property_GraphX_TerminalSpacing(PropertyRoot owner)
        {
            Owner = owner;
            Value = new Int32Value(this);

            owner.Add(this);
        }

        internal override int GetValue(Item item) => Cast(item).TerminalSpacing;
        internal override void SetValue(Item item, int val) => Cast(item).TerminalSpacing = (byte)val;
    }
}
