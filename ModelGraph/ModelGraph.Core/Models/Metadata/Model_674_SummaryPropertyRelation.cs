using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_674_SummaryPropertyRelation : List2ModelOf<TableX, Property>
    {
        Relation_Store_SummaryProperty _relation_Store_SummaryProperty;
        internal Model_674_SummaryPropertyRelation(Model_654_Table owner, TableX item) : base(owner, item)
        {
            _relation_Store_SummaryProperty = item.Owner.Owner.Get<Relation_Store_SummaryProperty>();
        }
        internal override IdKey IdKey => IdKey.Model_674_SummaryPropertyRelation;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override string GetKindId() => string.Empty;

        protected override int GetTotalCount() => _relation_Store_SummaryProperty.ChildCount(Item);

        protected override IList<Property> GetChildItems() => _relation_Store_SummaryProperty.TryGetChildren(Item, out IList<Property> list) ? list : new Property[0];

        protected override void CreateChildModel(Property np)
        {
            new Model_676_SummaryProperty(this, np);
        }

        internal override DropAction ModelDrop(Root root, ItemModel dropModel, bool doDrop)
        {
            if (dropModel.GetItem() is Property np)
            {
                if (doDrop)
                {
                    ItemLinked.Record(root, _relation_Store_SummaryProperty, Item, np);
                    ChildDelta++;
                    AutoExpandLeft = true;
                }
                return DropAction.Link;
            }
            return DropAction.None;
        }

    }
}
