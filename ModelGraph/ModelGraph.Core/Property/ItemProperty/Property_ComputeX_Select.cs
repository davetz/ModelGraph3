
namespace ModelGraph.Core
{
    public class Property_ComputeX_Select : PropertyOf<ComputeX, string>
    {
        internal override IdKey IdKey => IdKey.ComputeXSelectProperty;

        internal Property_ComputeX_Select(PropertyRoot owner) : base(owner)
        {
            Value = new StringValue(this);
        }

        internal override string GetValue(Item item) => Cast(item).GetSelectString();
        internal override void SetValue(Item item, string val) => Cast(item).SetSelectString(val);
    }
}
