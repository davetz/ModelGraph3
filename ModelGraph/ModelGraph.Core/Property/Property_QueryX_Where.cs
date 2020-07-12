
namespace ModelGraph.Core
{
    public class Property_QueryX_Where : PropertyOf<QueryX, string>
    {
        internal override IdKey IdKey => IdKey.QueryXWhereProperty;

        internal Property_QueryX_Where(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) => Cast(item).WhereString;
        internal override void SetValue(Item item, string val) => DataRoot.TrySetWhereProperty(Cast(item), val);
        internal override string GetParentName(Root root, Item item) => DataRoot.GetWhereName(Cast(item));
    }
}
