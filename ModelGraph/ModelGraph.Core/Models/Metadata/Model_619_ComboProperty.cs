
namespace ModelGraph.Core
{
    public class Model_619_ComboProperty : PropertyModel
    {
        internal Model_619_ComboProperty(ItemModel owner, Item item, Property prop) : base(owner, item, prop) { }
        internal override IdKey IdKey => IdKey.Model_619_ComboProperty;
        public override bool IsComboModel => true;
        public override int GetIndexValue(Root root) => Property.GetIndexValue(Item);
        public override string[] GetListValue(Root root) => Property.GetlListValue(root);
    }
}
