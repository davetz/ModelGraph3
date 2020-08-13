
namespace ModelGraph.Core
{
    public class Property_ComputeX_Separator : PropertyOf<ComputeX, string>
    {
        internal override IdKey IdKey => IdKey.ComputeXSeparatorProperty;

        internal Property_ComputeX_Separator(PropertyManager owner) : base(owner)
        {
            Value = new StringValue(this);
        }

        internal override string GetValue(Item item) => Cast(item).Separator;
        internal override void SetValue(Item item, string val) => Cast(item).Separator = val;
    }
}
