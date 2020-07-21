
using System;

namespace ModelGraph.Core
{
    public class Property_ColumnX_IsChoice : PropertyOf<ColumnX, bool>
    {
        internal override IdKey IdKey => IdKey.ColumnIsChoiceProperty;
        protected override Type PropetyModelType => typeof(Model_618_CheckProperty);

        internal Property_ColumnX_IsChoice(PropertyRoot owner) : base(owner)
        {
            Value = new BoolValue(this);
        }

        internal override bool GetValue(Item item) => Cast(item).IsChoice;
        internal override void SetValue(Item item, bool val) => Cast(item).IsChoice = val;
    }
}
