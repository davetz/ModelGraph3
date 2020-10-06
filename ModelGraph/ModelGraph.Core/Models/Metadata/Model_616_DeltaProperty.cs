
namespace ModelGraph.Core
{
    public class Model_616_DeltaProperty : PropertyModel
    {
        internal Model_616_DeltaProperty(ItemModel owner, Item item, Property prop) : base(owner, item, prop) { }
        internal override IdKey IdKey => IdKey.Model_616_DeltaProperty;
        public override bool IsDeltaModel => true;

        public override string GetTextValue(Root root) => Property.Value.GetString(Item);
        public override int GetInt32Value(Root root) => Property.Value.GetValue(Item, out int val) ? val : 0;
    }
}
