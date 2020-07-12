using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class SummaryPropertyRelationModel_674 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal SummaryPropertyRelationModel_674(TableModel_654 owner, TableX item) : base(owner, item) { }
        private TableX TX => Item as TableX;
        internal override IdKey IdKey => IdKey.SummaryPropertyRelationModel_674;

        public override bool CanExpandLeft => TotalCount > 0;
        public override int TotalCount => DataRoot.Get<Relation_Store_SummaryProperty>().ChildCount(Item);        

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            {
                IsExpandedLeft = true;

                if (root.Get<Relation_Store_SummaryProperty>().TryGetChild(Item, out Property sp))
                {
                    new SummaryPropertyModel_676(this, sp);
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

                    if (!root.Get<Relation_Store_SummaryProperty>().TryGetChild(Item, out Property sp))
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

                    if (prev.TryGetValue(sp, out LineModel m))
                    {
                        CovertAdd(m);
                        prev.Remove(m.Item);
                    }
                    else
                    {
                        new SummaryPropertyModel_676(this, sp);
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
