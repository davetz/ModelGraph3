
namespace ModelGraph.Core
{
    public class Property_QueryX_Select : PropertyOf<QueryX, string>
    {
        internal override IdKey IdKey => IdKey.QueryXSelectProperty;

        internal Property_QueryX_Select(PropertyRoot owner) : base(owner)
        {
            Value = new StringValue(this);
        }

        internal override string GetValue(Item item) => Cast(item).SelectString;
        internal override void SetValue(Item item, string val) =>  Cast(item).Owner.SetSelectString(Cast(item), val);
        internal override string GetParentName(Root root, Item item) => Cast(item).Owner .GetSelectName(Cast(item));
    }
}
