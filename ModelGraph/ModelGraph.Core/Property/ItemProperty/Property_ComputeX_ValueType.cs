
namespace ModelGraph.Core
{
    public class Property_ComputeX_ValueType : PropertyOf<ComputeX, string>
    {
        internal override IdKey IdKey => IdKey.ComputeXValueTypeProperty;
        internal override bool IsReadonly => true;

        internal Property_ComputeX_ValueType(PropertyManager owner) : base(owner)
        {
            Value = new StringValue(this);
        }

        internal override string GetValue(Item item) => Owner.Owner.Get<Enum_ValueType>().GetEnumValueName((int)Cast(item).Value.ValType); 
        internal override string GetTargetName(Item item) => Owner.Owner.Get<Relation_Store_ComputeX>().TryGetParent(item, out Store p) ? p.GetNameId() : InvalidItem;
    }
}
