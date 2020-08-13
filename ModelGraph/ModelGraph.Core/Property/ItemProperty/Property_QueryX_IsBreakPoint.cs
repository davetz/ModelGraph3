
using System;

namespace ModelGraph.Core
{
    public class Property_QueryX_IsBreakPoint : PropertyOf<QueryX, bool>
    {
        internal override IdKey IdKey => IdKey.QueryXIsBreakPointProperty;
        protected override Type PropetyModelType => typeof(Model_618_CheckProperty);

        internal Property_QueryX_IsBreakPoint(PropertyManager owner) : base(owner)
        {
            Value = new BoolValue(this);
        }

        internal override bool GetValue(Item item) => Cast(item).IsBreakPoint;
        internal override void SetValue(Item item, bool val) => Cast(item).IsBreakPoint = val;
    }
}
