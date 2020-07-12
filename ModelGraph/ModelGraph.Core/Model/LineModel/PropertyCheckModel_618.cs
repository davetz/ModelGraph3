
namespace ModelGraph.Core
{
    public class PropertyCheckModel_618 : PropertyModel
    {
        internal PropertyCheckModel_618(LineModel owner, Item item, Property prop) : base(owner, item, prop) { }
        internal override IdKey IdKey => IdKey.PropertyCheckModel_618;
        public override bool IsCheckModel => true;

        public override (string, string) GetKindNameId(Root root) => (null, Property.GetNameId(root));
        public override string GetSummaryId(Root root) => Property.GetSummaryId(root);
        public override bool GetBoolValue(Root root) => Property.Value.GetBool(Item);
    }
}
