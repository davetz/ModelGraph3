using System;

namespace ModelGraph.Core
{
    public class Property_ColumnX_ValueType : EnumPropertyOf<ColumnX>
    {
        internal override IdKey IdKey => IdKey.ColumnValueTypeProperty;

        internal Property_ColumnX_ValueType(PropertyRoot owner) : base(owner, owner.DataRoot.Get<Enum_ValueType>()) { }

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).Value.ValType;

        internal override void SetItemPropertyValue(Item item, int key) => Cast(item).TrySetValueType((ValType)key);
    }
}
