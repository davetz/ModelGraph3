
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class ComputeListModel_666 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal ComputeListModel_666(TableModel_654 owner, TableX item) : base(owner, item) { }
        private TableX TX => Item as TableX;
        internal override IdKey IdKey => IdKey.ComputeListModel_666;

        public override bool CanExpandLeft => TotalCount > 0;
        public override bool CanFilter => true;
        public override bool CanSort => true;
        public override bool CanDrag => true;

        internal override string GetFilterSortId(Root root) => GetNameId(root);
        public override int TotalCount => DataRoot.Get<Relation_Store_ComputeX>().ChildCount(Item);

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            {
                IsExpandedLeft = true;

                if (root.Get<Relation_Store_ComputeX>().TryGetChildren(Item, out IList<ComputeX> cxList))
                {
                    foreach (var cx in cxList)
                    {
                        new ComputeModel_658(this, cx);
                    }
                }
            }

            return true;
        }
        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => AddNewComputeX(root)));
        }
        private void AddNewComputeX(Root root)
        {
            var cx = new ComputeX(root.Get<ComputeXRoot>(), true);

            // the data root implements undo/redo functionality
            ItemCreated.Record(root, cx);
            ItemLinked.Record(root, root.Get<Relation_Store_ComputeX>(), TX, cx);
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

                    if (!root.Get<Relation_Store_ComputeX>().TryGetChildren(Item, out IList<ComputeX> cxList))
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

                    foreach (var cx in cxList)
                    {
                        if (prev.TryGetValue(cx, out LineModel m))
                        {
                            CovertAdd(m);
                            prev.Remove(m.Item);
                        }
                        else
                        {
                            new ComputeModel_658(this, cx);
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
