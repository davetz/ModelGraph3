﻿
namespace ModelGraph.Core
{
    public class Property_QueryX_LineColor : PropertyOf<QueryX, string>
    {
        internal override IdKey IdKey => IdKey.QueryXLineColorProperty;

        internal Property_QueryX_LineColor(PropertyRoot owner) : base(owner)
        {
            Value = new StringValue(this);
        }

        internal override string GetValue(Item item) => Cast(item).PathParm.LineColor;
        internal override void SetValue(Item item, string val) => Cast(item).PathParm.LineColor = val;
    }
}
