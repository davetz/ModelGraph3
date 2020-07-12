
namespace ModelGraph.Core
{
    public class Property_Item_Description : PropertyOf<Item, string>
    {
        internal override IdKey IdKey => IdKey.ItemDescriptionProperty;

        internal Property_Item_Description(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) => item.Description;
        internal override void SetValue(Item item, string val) => item.Description = val;
    }
}
