
namespace ModelGraph.Core
{
    public class Property_QueryX_IsBreakPoint : PropertyOf<QueryX, bool>
    {
        internal override IdKey IdKey => IdKey.QueryXIsBreakPointProperty;

        internal Property_QueryX_IsBreakPoint(PropertyRoot owner)
        {
            Owner = owner;
            Value = new BoolValue(this);

            owner.Add(this);
        }

        internal override bool GetValue(Item item) => Cast(item).IsBreakPoint;
        internal override void SetValue(Item item, bool val) => Cast(item).IsBreakPoint = val;
    }
}
