
namespace ModelGraph.Core
{
    public class Property_ComputeX_Where : PropertyOf<ComputeX, string>
    {
        internal override IdKey IdKey => IdKey.ComputeXWhereProperty;

        internal Property_ComputeX_Where(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var root = DataRoot; return root.GetWhereProperty(Cast(item)); }
        internal override void SetValue(Item item, string val) { var root = DataRoot; root.TrySetWhereProperty(Cast(item), val); }
    }
}
