﻿
namespace ModelGraph.Core
{
    public class Property_Item_Summary : PropertyOf<Item, string>
    {
        internal override IdKey IdKey => IdKey.ItemSummaryProperty;

        internal Property_Item_Summary(PropertyRoot owner) : base(owner)
        {
            Value = new StringValue(this);
        }

        internal override string GetValue(Item item) => item.Summary;
        internal override void SetValue(Item item, string val) => item.Summary = val;
    }
}
