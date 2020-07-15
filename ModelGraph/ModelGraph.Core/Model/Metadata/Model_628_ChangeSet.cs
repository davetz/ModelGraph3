
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_628_ChangeSet : LineModel
    {
        internal Model_628_ChangeSet(Model_622_ChangeRoot owner, ChangeSet item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_628_ChangeSet;

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
                new Model_629_ChangeItem(this, cs);
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
                            new Model_629_ChangeItem(this, cs);
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
