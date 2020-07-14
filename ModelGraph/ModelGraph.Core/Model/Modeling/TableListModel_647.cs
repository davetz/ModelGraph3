
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class TableListModel_647 : LineModel
    {//============================================== In the ModelingRoot hierarchy  ==============
        internal TableListModel_647(ModelingRootModel_624 owner, TableXRoot item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.TableListModel_647;

        public override bool CanExpandLeft => TotalCount > 0;
        public override bool CanFilter => TotalCount > 1;
        public override bool CanSort => TotalCount > 1;
        public override int TotalCount => ItemStore.Count;

        private TableXRoot TableXRoot => Item as TableXRoot;


        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;

            IsExpandedLeft = true;

            foreach (var tx in TableXRoot.Items)
            {
                new TableModel_6A4(this, tx);
            }

            return true;
        }

        internal override bool Validate(Root root, Dictionary<Item, LineModel> prev)
        {
            var viewListChanged = false;
            if (IsExpanded)
            {
                if (ChildDelta != Item.ChildDelta)
                {
                    ChildDelta = Item.ChildDelta;

                    prev.Clear();
                    foreach (var child in Items)
                    {
                        prev[child.Item] = child;
                    }
                    CovertClear();

                    foreach (var tx in TableXRoot.Items)
                    {
                        if (prev.TryGetValue(tx, out LineModel m))
                        {
                            CovertAdd(m);
                            prev.Remove(m.Item);
                        }
                        else
                        {
                            new TableModel_6A4(this, tx);
                            viewListChanged = true;
                        }
                    }

                    if (prev.Count > 0)
                    {
                        viewListChanged = true;
                        foreach (var model in prev.Values) { model.Discard(); }
                    }
                }
            }
            return viewListChanged || base.Validate(root, prev);
        }
    }
}
