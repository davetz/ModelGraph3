using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class SummaryPropertyModel_676 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal SummaryPropertyModel_676(SummaryPropertyRelationModel_674 owner, Property item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.SummaryPropertyModel_676;
        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);

        public override void GetMenuCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => RemoveSummaryProperty(root)));
        }
        private void RemoveSummaryProperty(Root root)
        {
            var pm = Owner as LineModel;
            var tx = pm.Item;
            pm.ChildDelta++;
            ItemUnLinked.Record(root, root.Get<Relation_Store_SummaryProperty>(), tx, Item);
        }
    }
}
