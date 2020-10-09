

namespace ModelGraph.Core
{
    public class Property_Shape_Polylocked : PropertyOf<SymbolModel, bool>
    {
        internal override IdKey IdKey => IdKey.ShapePolylockedProperty;

        internal Property_Shape_Polylocked(PropertyManager owner) : base(owner)
        {
            Value = new BoolValue(this);
        }

        internal override bool GetValue(Item item) => Cast(item).PolyLocked;
        internal override void SetValue(Item item, bool val) => Cast(item).PolyLocked = val;
        internal override void CreatePropertyModel(ItemModel owner, Item item) => new Model_616_DeltaProperty(owner, item, this);
    }
}
