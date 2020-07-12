
namespace ModelGraph.Core
{
    public class Property_ComputeX_Select : PropertyOf<ComputeX, string>
    {
        internal override IdKey IdKey => IdKey.ComputeXSelectProperty;

        internal Property_ComputeX_Select(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) => DataRoot.GetSelectProperty(Cast(item));
        internal override void SetValue(Item item, string val) => DataRoot.TrySetSelectProperty(Cast(item), val);
    }
}
