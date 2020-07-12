
namespace ModelGraph.Core
{
    public class PropertyTextModel_617 : PropertyModel
    {
        internal PropertyTextModel_617(LineModel owner, Item item, Property prop) : base(owner, item, prop) { }
        internal override IdKey IdKey => IdKey.PropertyTextModel_617;
        public override bool IsTextModel => true;

        public override (string, string) GetKindNameId(Root root) => (null, Property.GetNameId(root));
        public override string GetSummaryId(Root root) => Property.GetSummaryId(root);

        public override string GetTextValue(Root root) => Property.Value.GetString(Item);
    }
}
