
namespace ModelGraph.Core
{
    public class Property_QueryX_Where : PropertyOf<QueryX, string>
    {
        internal override IdKey IdKey => IdKey.QueryXWhereProperty;

        internal Property_QueryX_Where(PropertyRoot owner) : base(owner)
        {
            Value = new StringValue(this);
        }

        internal override string GetValue(Item item) => Cast(item).WhereString;
        internal override void SetValue(Item item, string val) => Cast(item).WhereString = val;
        internal override string GetParentName(Root root, Item item) => Cast(item).GetTailTableName();
    }
}
