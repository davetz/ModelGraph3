
namespace ModelGraph.Core
{
    public class Property_QueryX_ValueType : PropertyOf<QueryX, string>
    {
        internal override IdKey IdKey => IdKey.ValueXValueTypeProperty;
        internal override bool IsReadonly => true;

        internal Property_QueryX_ValueType(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var root = DataRoot; return root.Get<Enum_ValueType>().GetEnumValueName(root, (int)root.GetValueType(Cast(item))); }
    }
}
