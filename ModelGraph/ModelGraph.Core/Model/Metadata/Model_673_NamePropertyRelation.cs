﻿using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_673_NamePropertyRelation : ListModelOf<TableX, Property>
    {
        Relation_Store_NameProperty Store_NameProperty;

        internal Model_673_NamePropertyRelation(Model_654_Table owner, TableX item) : base(owner, item) 
        {
            Store_NameProperty = item.GetRoot().Get<Relation_Store_NameProperty>();
        }
        internal override IdKey IdKey => IdKey.Model_673_NamePropertyRelation;

        protected override int GetTotalCount() => Store_NameProperty.ChildCount(Item);

        protected override IList<Property> GetChildItems() => Store_NameProperty.TryGetChildren(Item, out IList<Property> list) ? list : new Property[0];

        protected override void CreateChildModel(Property np)
        {
            new Model_675_NameProperty(this, np);
        }

        internal override DropAction ModelDrop(Root root, LineModel dropModel, bool doDrop)
        {
            if (dropModel.GetItem() is Property np)
            {
                if (doDrop)
                {
                    ItemLinked.Record(root, root.Get<Relation_Store_NameProperty>(), Item, np);
                    ChildDelta++;
                    AutoExpandLeft = true;
                }
                return DropAction.Link;
            }
            return DropAction.None;
        }

    }
}
