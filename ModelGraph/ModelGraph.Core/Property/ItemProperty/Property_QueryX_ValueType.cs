
namespace ModelGraph.Core
{
    public class Property_QueryX_ValueType : PropertyOf<QueryX, string>
    {
        internal override IdKey IdKey => IdKey.ValueXValueTypeProperty;
        internal override bool IsReadonly => true;

        internal Property_QueryX_ValueType(PropertyRoot owner) : base(owner)
        {
            Value = new StringValue(this);
        }

        internal override string GetValue(Item item) => "Need to fix this";
    }
}
