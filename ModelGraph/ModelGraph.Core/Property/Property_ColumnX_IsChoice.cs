
namespace ModelGraph.Core
{
    public class Property_ColumnX_IsChoice : PropertyOf<ColumnX, bool>
    {
        internal override IdKey IdKey => IdKey.ColumnIsChoiceProperty;

        internal Property_ColumnX_IsChoice(PropertyRoot owner)
        {
            Owner = owner;
            Value = new BoolValue(this);

            owner.Add(this);
        }

        internal override bool GetValue(Item item) => Cast(item).IsChoice;
        internal override void SetValue(Item item, bool val) => Cast(item).IsChoice = val;
    }
}
