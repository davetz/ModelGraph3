
using System;

namespace ModelGraph.Core
{
    public class Property_Shape_IsFilled : PropertyOf<SymbolModel, bool>
    {
        internal override IdKey IdKey => IdKey.ShapeIsFilledProperty;
        protected override Type PropetyModelType => typeof(Model_618_CheckProperty);

        internal Property_Shape_IsFilled(PropertyManager owner) : base(owner)
        {
            Value = new BoolValue(this);
        }

        internal override bool GetValue(Item item) => Cast(item).IsFilled;
        internal override void SetValue(Item item, bool val) => Cast(item).IsFilled = val;
    }
}
