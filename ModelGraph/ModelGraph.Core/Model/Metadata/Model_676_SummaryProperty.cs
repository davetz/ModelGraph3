using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_676_SummaryProperty : LineModelOf<Property>
    {
        internal Model_676_SummaryProperty(Model_674_SummaryPropertyRelation owner, Property item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_676_SummaryProperty;
        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);

        public override void GetMenuCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => RemoveSummaryProperty(root)));
        }
        private void RemoveSummaryProperty(Root root)
        {
            Owner.ChildDelta++;
            ItemUnLinked.Record(root, root.Get<Relation_Store_SummaryProperty>(), Owner.GetItem(), Item);
        }
    }
}
