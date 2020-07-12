
namespace ModelGraph.Core
{
    public class Property_PairX_Text : PropertyOf<PairX, string>
    {
        internal override IdKey IdKey => IdKey.EnumTextProperty;

        internal Property_PairX_Text(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) => Cast(item).Name;
        internal override void SetValue(Item item, string val) => Cast(item).Name = val;
    }
}
