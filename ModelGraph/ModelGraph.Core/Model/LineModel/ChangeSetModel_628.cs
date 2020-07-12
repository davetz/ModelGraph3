
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class ChangeSetModel_628 : LineModel
    {
        internal ChangeSetModel_628(ChangeRootModel_622 owner, ChangeSet item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ChangeSetModel_628;

        public override bool CanExpandLeft => TotalCount > 0;
        public override int TotalCount => ItemStore.Count;

        public override (string, string) GetKindNameId(Root root) => (string.Empty, ChangeSet.Name);

        public override string GetSummaryId(Root root) => $"{ChangeSet.Name}   -   {ChangeSet.DateTime:T}";

        ChangeSet ChangeSet => Item as ChangeSet;

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;

            IsExpandedLeft = true;

            var sto = Item as ChangeSet;
            foreach (var cs in sto.Items)
            {
                new ChangeItemModel_629(this, cs);
            }

            return true;
        }
        internal override bool Validate(Root root, Dictionary<Item, LineModel> prev)
        {
            var viewListChanged = false;
            if (IsExpanded || AutoExpandLeft)
            {
                AutoExpandLeft = false;
                IsExpandedLeft = true;

                if (ChildDelta != Item.ChildDelta)
                {
                    ChildDelta = Item.ChildDelta;

                    prev.Clear();
                    foreach (var child in Items)
                    {
                        prev[child.Item] = child;
                    }
                    CovertClear();

                    var st = Item as ChangeSet;
                    foreach (var cs in st.Items)
                    {
                        if (prev.TryGetValue(cs, out LineModel m))
                        {
                            CovertAdd(m);
                            prev.Remove(m.Item);
                        }
                        else
                        {
                            new ChangeItemModel_629(this, cs);
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
