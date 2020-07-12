
namespace ModelGraph.Core
{
    public class Property_Relation_IsRequired : PropertyOf<Relation, bool>
    {
        internal override IdKey IdKey => IdKey.RelationIsRequiredProperty;

        internal Property_Relation_IsRequired(PropertyRoot owner)
        {
            Owner = owner;
            Value = new BoolValue(this);

            owner.Add(this);
        }

        internal override bool GetValue(Item item) => Cast(item).IsRequired;
        internal override void SetValue(Item item, bool val) => Cast(item).IsRequired = val;
    }
}
