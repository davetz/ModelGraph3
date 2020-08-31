
namespace ModelGraph.Core
{
    public class Model_618_CheckProperty : PropertyModel
    {
        internal Model_618_CheckProperty(ItemModel owner, Item item, Property prop) : base(owner, item, prop) { }
        internal override IdKey IdKey => IdKey.Model_618_CheckProperty;
        public override bool IsCheckModel => true;
        public override bool GetBoolValue(Root root) => Property.Value.GetBool(Item);
    }
}
