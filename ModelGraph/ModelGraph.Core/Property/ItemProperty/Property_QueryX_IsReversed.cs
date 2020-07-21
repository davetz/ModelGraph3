
namespace ModelGraph.Core
{
    public class Property_QueryX_IsReversed : PropertyOf<QueryX, bool>
    {
        internal override IdKey IdKey => IdKey.QueryXIsReversedProperty;

        internal Property_QueryX_IsReversed(PropertyRoot owner) : base(owner)
        {
            Value = new BoolValue(this);
        }

        internal override bool GetValue(Item item) => Cast(item).IsReversed;
        internal override void SetValue(Item item, bool val) => Cast(item).IsReversed = val;
    }
}
