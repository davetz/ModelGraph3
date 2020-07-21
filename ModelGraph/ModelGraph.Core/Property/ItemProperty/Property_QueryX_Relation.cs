
namespace ModelGraph.Core
{
    public class Property_QueryX_Relation : PropertyOf<QueryX, string>
    {
        internal override IdKey IdKey => IdKey.QueryXRelationProperty;
        internal override bool IsReadonly => true;

        internal Property_QueryX_Relation(PropertyRoot owner) : base(owner)
        {
            Value = new StringValue(this);
        }

        internal override string GetValue(Item item) => DataRoot.GetQueryXRelationName(Cast(item));
    }
}
