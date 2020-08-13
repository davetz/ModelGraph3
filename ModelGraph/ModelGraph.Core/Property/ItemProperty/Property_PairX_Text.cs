
namespace ModelGraph.Core
{
    public class Property_PairX_Text : PropertyOf<PairX, string>
    {
        internal override IdKey IdKey => IdKey.EnumTextProperty;

        internal Property_PairX_Text(PropertyManager owner) : base(owner)
        {
            Value = new StringValue(this);
        }

        internal override string GetValue(Item item) => Cast(item).DisplayValue;
        internal override void SetValue(Item item, string val) => Cast(item).DisplayValue = val;
    }
}
