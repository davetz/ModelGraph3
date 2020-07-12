
using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class ChangeRootModel_622 : LineModel
    {
        internal ChangeRootModel_622(RootModel_612 owner, ChangeRoot item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ChangeRootModel_622;

        public override bool CanExpandLeft => TotalCount > 0;
        public override int TotalCount => ItemStore.Count;

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;

            IsExpandedLeft = true;

            var csStore = Item as ChangeRoot;
            foreach (var cs in csStore.Items.Reverse())
            {
                new ChangeSetModel_628(this, cs);
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
                    foreach (var child in Items.Reverse())
                    {
                        prev[child.Item] = child;
                    }
                    CovertClear();

                    var st = Item as ChangeRoot;
                    foreach (var cs in st.Items)
                    {
                        if (prev.TryGetValue(cs, out LineModel m))
                        {
                            CovertAdd(m);
                            prev.Remove(m.Item);
                        }
                        else
                        {
                            new ChangeSetModel_628(this, cs);
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
