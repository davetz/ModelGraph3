
namespace ModelGraph.Core
{
    public class Model_617_TextProperty : PropertyModel
    {
        internal Model_617_TextProperty(LineModel owner, Item item, Property prop) : base(owner, item, prop) { }
        internal override IdKey IdKey => IdKey.Model_617_TextProperty;
        public override bool IsTextModel => true;

        public override string GetTextValue(Root root) => Property.Value.GetString(Item);
    }
}
