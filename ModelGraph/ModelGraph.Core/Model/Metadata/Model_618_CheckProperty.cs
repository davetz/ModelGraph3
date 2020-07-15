
namespace ModelGraph.Core
{
    public class Model_618_CheckProperty : PropertyModel
    {
        internal Model_618_CheckProperty(LineModel owner, Item item, Property prop) : base(owner, item, prop) { }
        internal override IdKey IdKey => IdKey.Model_618_CheckProperty;
        public override bool IsCheckModel => true;

        public override (string, string) GetKindNameId(Root root) => (null, Property.GetNameId(root));
        public override string GetSummaryId(Root root) => Property.GetSummaryId(root);
        public override bool GetBoolValue(Root root) => Property.Value.GetBool(Item);
    }
}
