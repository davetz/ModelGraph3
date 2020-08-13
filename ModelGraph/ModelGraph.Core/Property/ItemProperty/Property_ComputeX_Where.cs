
namespace ModelGraph.Core
{
    public class Property_ComputeX_Where : PropertyOf<ComputeX, string>
    {
        internal override IdKey IdKey => IdKey.ComputeXWhereProperty;

        internal Property_ComputeX_Where(PropertyManager owner) : base(owner)
        {
            Value = new StringValue(this);
        }

        internal override string GetValue(Item item) =>  Cast(item).GetWhereString();
        internal override void SetValue(Item item, string val) => Cast(item).SetWhereString(val);
    }
}
