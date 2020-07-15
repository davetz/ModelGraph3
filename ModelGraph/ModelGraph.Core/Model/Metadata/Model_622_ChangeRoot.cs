
using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class Model_622_ChangeRoot : LineModel
    {
        internal Model_622_ChangeRoot(Model_612_Root owner, ChangeRoot item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_622_ChangeRoot;

        public override bool CanExpandLeft => TotalCount > 0;
        public override int TotalCount => ItemStore.Count;

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;

            IsExpandedLeft = true;

            var csStore = Item as ChangeRoot;
            foreach (var cs in csStore.Items.Reverse())
            {
                new Model_628_ChangeSet(this, cs);
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
                            new Model_628_ChangeSet(this, cs);
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
