
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class ParentRelatationListModel_663 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal ParentRelatationListModel_663(TableModel_654 owner, TableX item) : base(owner, item) { }
        private TableX TX => Item as TableX;
        internal override IdKey IdKey => IdKey.ParentRelatationListModel_663;

        public override bool CanExpandLeft => TotalCount > 0;
        public override bool CanFilter => true;
        public override bool CanSort => true;

        internal override string GetFilterSortId(Root root) => GetNameId(root);
        public override int TotalCount => DataRoot.Get<Relation_StoreX_ParentRelation>().ChildCount(Item);

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            {
                IsExpandedLeft = true;

                if (root.Get<Relation_StoreX_ParentRelation>().TryGetChildren(Item, out IList<Relation> cxList))
                {
                    foreach (var rx in cxList)
                    {
                        new ParentRelationModel_672(this, rx);
                    }
                }
            }

            return true;
        }
        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => AddNewParentRelation(root)));
        }
        private void AddNewParentRelation(Root root)
        {
            var rx = new RelationX_RowX_RowX(root.Get<RelationXRoot>(), true);

            // the data root implements undo/redo functionality
            ItemCreated.Record(root, rx);
            ItemLinked.Record(root, root.Get<Relation_StoreX_ParentRelation>(), TX, rx);
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

                    if (!root.Get<Relation_StoreX_ParentRelation>().TryGetChildren(Item, out IList<Relation> rxList))
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
                            new ParentRelationModel_672(this, rx);
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
