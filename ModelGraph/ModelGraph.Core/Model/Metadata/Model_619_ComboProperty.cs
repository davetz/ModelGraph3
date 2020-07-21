﻿
namespace ModelGraph.Core
{
    public class Model_619_ComboProperty : PropertyModelOf<Item>
    {
        internal Model_619_ComboProperty(LineModel owner, Item item, Property prop) : base(owner, item, prop) { }
        internal override IdKey IdKey => IdKey.Model_619_ComboProperty;
        public override bool IsComboModel => true;

        public override (string, string) GetKindNameId(Root root) => (null, Property.GetNameId(root));
        public override string GetSummaryId(Root root) => Property.GetSummaryId(root);

        public override int GetIndexValue(Root root) => Property.GetIndexValue(Item);
        public override string[] GetlListValue(Root root) => Property.GetlListValue(root);
    }
}
