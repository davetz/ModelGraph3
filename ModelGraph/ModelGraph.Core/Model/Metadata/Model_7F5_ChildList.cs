using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_7F5_ChildList : StaticModelOf<Relation>
    {
        internal Model_7F5_ChildList(Model_7F4_Relation owner, Relation item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_7F5_ChildList;

        public override bool CanExpandLeft => TotalCount > 0;
        public override bool CanFilter => TotalCount > 1;
        public override bool CanSort => TotalCount > 1;
        public override int TotalCount => base.Item.GetChildLinkPairCount();


        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            if (TotalCount > 0)
            {
                IsExpandedLeft = true;
                SetCapacity(TotalCount);

                foreach (var pair in Item.GetChildLinkPairList())
                {
                    new Model_7F7_ParentChild(this, Item, pair);
                }
            }
            return true;
        }

        internal override bool Validate(Root root, Dictionary<Item, LineModel> prev)
        {
            var prev2 = new Dictionary<(Item, Item), LineModel>(Count);
            
            var viewListChanged = false;
            if (IsExpanded || AutoExpandLeft)
            {
                AutoExpandLeft = false;
                IsExpandedLeft = true;

                if (ChildDelta != base.Item.ChildDelta)
                {
                    ChildDelta = base.Item.ChildDelta;

                    prev.Clear();
                    foreach (var child in base.Items)
                    {
                        var pc = child as Model_7F7_ParentChild;
                        prev2[pc.ItemPair] = pc;
                    }
                    base.Clear();

                    if (TotalCount > 0)
                    {
                        SetCapacity(TotalCount);

                        foreach (var pair in Item.GetChildLinkPairList())
                        {

                            if (prev2.TryGetValue(pair, out LineModel m))
                            {
                                base.Add(m);
                                prev.Remove(m.GetItem());
                            }
                            else
                            {
                                new Model_7F7_ParentChild(this, Item, pair);
                                viewListChanged = true;
                            }
                        }
                    }

                    if (prev2.Count > 0)
                    {
                        viewListChanged = true;
                        foreach (var model in prev2.Values) { model.Discard(); }
                    }
                }
            }
            return viewListChanged || base.Validate(root, prev);
        }
    }
}
