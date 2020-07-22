using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_674_SummaryPropertyRelation : ListModelOf<TableX, Property>
    {
        Relation_Store_SummaryProperty Store_SummaryProperty;
        internal Model_674_SummaryPropertyRelation(Model_654_Table owner, TableX item) : base(owner, item) { }
        private TableX TX => Item as TableX;
        internal override IdKey IdKey => IdKey.Model_674_SummaryPropertyRelation;

        protected override int GetTotalCount() => Store_SummaryProperty.ChildCount(Item);

        protected override IList<Property> GetChildItems() => Store_SummaryProperty.TryGetChildren(Item, out IList<Property> list) ? list : new Property[0];

        protected override void CreateChildModel(Property np)
        {
            new Model_676_SummaryProperty(this, np);
        }

        internal override DropAction ModelDrop(Root root, LineModel dropModel, bool doDrop)
        {
            if (dropModel.GetItem() is Property np)
            {
                if (doDrop)
                {
                    ItemLinked.Record(root, root.Get<Relation_Store_SummaryProperty>(), TX, np);
                    ChildDelta++;
                    AutoExpandLeft = true;
                }
                return DropAction.Link;
            }
            return DropAction.None;
        }

    }
}
