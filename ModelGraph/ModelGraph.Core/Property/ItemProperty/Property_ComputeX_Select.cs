
namespace ModelGraph.Core
{
    public class Property_ComputeX_Select : PropertyOf<ComputeX, string>
    {
        internal override IdKey IdKey => IdKey.ComputeXSelectProperty;

        internal Property_ComputeX_Select(PropertyRoot owner) : base(owner)
        {
            Value = new StringValue(this);
        }

        internal override string GetValue(Item item) => DataRoot.GetSelectProperty(Cast(item));
        internal override void SetValue(Item item, string val) => DataRoot.TrySetSelectProperty(Cast(item), val);
    }
}
