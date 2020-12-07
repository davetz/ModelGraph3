
using System;

namespace ModelGraph.Core
{
    public class Property_Relation_IsRequired : PropertyOf<Relation, bool>
    {
        internal override IdKey IdKey => IdKey.RelationIsRequiredProperty;
        protected override Type PropetyModelType => typeof(Model_618_CheckProperty);

        internal Property_Relation_IsRequired(PropertyRoot owner) : base(owner)
        {
            Value = new BoolValue(this);
        }

        internal override bool GetValue(Item item) => Cast(item).IsRequired;
        internal override void SetValue(Item item, bool val) => Cast(item).IsRequired = val;
    }
}
