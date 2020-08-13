
namespace ModelGraph.Core
{
    public class Property_QueryX_Where : PropertyOf<QueryX, string>
    {
        internal override IdKey IdKey => IdKey.QueryXWhereProperty;

        internal Property_QueryX_Where(PropertyManager owner) : base(owner)
        {
            Value = new StringValue(this);
        }

        internal override string GetValue(Item item) => Cast(item).WhereString;
        internal override void SetValue(Item item, string val) => Cast(item).WhereString = val;
        internal override bool HasTargetName => true;
        internal override string GetTargetName(Item item) => $"{Cast(item).GetWhereSelectTargetName()}  {Owner.Owner.GetNameId(IdKey)}";
    }
}
