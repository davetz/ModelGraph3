
namespace ModelGraph.Core
{
    public class Property_PairX_Value : PropertyOf<PairX, string>
    {
        internal override IdKey IdKey => IdKey.EnumValueProperty;

        internal Property_PairX_Value(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) => Cast(item).ActualValue;
        internal override void SetValue(Item item, string val) => Cast(item).ActualValue = val;
    }
}
