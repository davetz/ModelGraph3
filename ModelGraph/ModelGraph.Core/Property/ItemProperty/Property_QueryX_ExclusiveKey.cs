﻿
namespace ModelGraph.Core
{
    public class Property_QueryX_ExclusiveKey : PropertyOf<QueryX, byte>
    {
        internal override IdKey IdKey => IdKey.QueryXExclusiveKeyProperty;

        internal Property_QueryX_ExclusiveKey(PropertyManager owner) : base(owner)
        {
            Value = new ByteValue(this);
        }

        internal override byte GetValue(Item item) => Cast(item).ExclusiveKey;
        internal override void SetValue(Item item, byte val) => Cast(item).ExclusiveKey = val;
    }
}
