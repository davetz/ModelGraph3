
namespace ModelGraph.Core
{
    public class Property_Item_Name : PropertyOf<Item, string>
    {
        internal override IdKey IdKey => IdKey.ItemNameProperty;

        internal Property_Item_Name(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) => item.Name;
        internal override void SetValue(Item item, string val) => item.Name = val;
    }
}
