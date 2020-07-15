using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_673_NamePropertyRelation : LineModel
    {
        internal Model_673_NamePropertyRelation(Model_654_Table owner, TableX item) : base(owner, item) { }
        private TableX TX => Item as TableX;
        internal override IdKey IdKey => IdKey.Model_673_NamePropertyRelation;

        public override bool CanExpandLeft => TotalCount > 0;
        public override int TotalCount => DataRoot.Get<Relation_Store_NameProperty>().ChildCount(Item);        

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            {
                IsExpandedLeft = true;

                if (root.Get<Relation_Store_NameProperty>().TryGetChild(Item, out Property np))
                {
                    new Model_675_NameProperty(this, np);
                }
            }

            return true;
        }

        internal override bool Validate(Root root, Dictionary<Item, LineModel> prev)
        {
            var viewListChange = false;
            if (IsExpanded || AutoExpandLeft)
            {
                AutoExpandLeft = false;
                IsExpandedLeft = true;

                if (ChildDelta != Item.ChildDelta)
                {
                    ChildDelta = Item.ChildDelta;

                    if (!root.Get<Relation_Store_NameProperty>().TryGetChild(Item, out Property np))
                    {
                        IsExpandedLeft = false;
                        DiscardChildren();
                        CovertClear();
                        return true;
                    }

                    prev.Clear();
                    foreach (var child in Items)
                    {
                        prev[child.Item] = child;
                    }
                    CovertClear();

                    if (prev.TryGetValue(np, out LineModel m))
                    {
                        CovertAdd(m);
                        prev.Remove(m.Item);
                    }
                    else
                    {
                        new Model_675_NameProperty(this, np);
                        viewListChange = true;
                    }

                    if (prev.Count > 0)
                    {
                        viewListChange = true;
                        foreach (var model in prev.Values) { model.Discard(); }
                    }
                }
            }
            return viewListChange || base.Validate(root, prev);
        }

        internal override DropAction ModelDrop(Root root, LineModel dropModel, bool doDrop)
        {
            if (dropModel.Item is Property np)
            {
                if (doDrop)
                {
                    ItemLinked.Record(root, root.Get<Relation_Store_NameProperty>(), TX, np);
                    ChildDelta++;
                    AutoExpandLeft = true;
                }
                return DropAction.Link;
            }
            return DropAction.None;
        }

    }
}
