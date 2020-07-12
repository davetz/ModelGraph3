using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class ChildRelationListModel_662 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal ChildRelationListModel_662(TableModel_654 owner, TableX item) : base(owner, item) { }
        private TableX TX => Item as TableX;
        internal override IdKey IdKey => IdKey.ChildRelationListModel_662;
        public override bool CanExpandLeft => TotalCount > 0;
        public override bool CanFilter => true;
        public override bool CanSort => true;

        internal override string GetFilterSortId(Root root) => GetNameId(root);
        public override int TotalCount => DataRoot.Get<Relation_StoreX_ChildRelation>().ChildCount(Item);

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            {
                IsExpandedLeft = true;

                if (root.Get<Relation_StoreX_ChildRelation>().TryGetChildren(Item, out IList<Relation> rxList))
                {
                    foreach (var cx in rxList)
                    {
                        new ChildRelationModel_671(this, cx);
                    }
                }
            }

            return true;
        }
        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => AddNewChildRelation(root)));
        }
        private void AddNewChildRelation(Root root)
        {
            var rx = new RelationX_RowX_RowX(root.Get<RelationXRoot>(), true);

            // the data root implements undo/redo functionality
            ItemCreated.Record(root, rx);
            ItemLinked.Record(root, root.Get<Relation_StoreX_ChildRelation>(), TX, rx);
            ChildDelta -= 2;
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

                    if (!root.Get<Relation_StoreX_ChildRelation>().TryGetChildren(Item, out IList<Relation> rxList))
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

                    foreach (var rx in rxList)
                    {
                        if (prev.TryGetValue(rx, out LineModel m))
                        {
                            CovertAdd(m);
                            prev.Remove(m.Item);
                        }
                        else
                        {
                            new ChildRelationModel_671(this, rx);
                            viewListChange = true;
                        }
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
    }
}
