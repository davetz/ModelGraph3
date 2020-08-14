
using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;

namespace ModelGraph.Core
{
    public abstract class List3ModelOf<T1> : List1ModelOf<T1> where T1 : Item
    {
        internal List3ModelOf(LineModel owner, T1 item) : base(owner, item) { }

        public override bool CanExpandLeft => TotalCount > 0;
        public override bool CanFilter => TotalCount > 1;
        public override bool CanSort => TotalCount > 1;
        public override int TotalCount => GetTotalCount();

        #region RequiredMethodes  =============================================
        protected abstract int GetTotalCount();
        protected abstract IList<(Item,Item)> GetChildItems();
        protected abstract (Item, Item) GetItemPair(LineModel child);
        protected abstract void CreateChildModel((Item,Item) childItem);
        #endregion

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            var N = TotalCount;
            if (N > 0)
            {
                IsExpandedLeft = true;
                SetCapacity(N);

                foreach (var itm in GetChildItems())
                {
                    CreateChildModel(itm);
                }
            }
            return true;
        }
        internal override bool Validate(Root root, Dictionary<Item, LineModel> prev)
        {
            var prev2 = new Dictionary<(Item, Item), LineModel>(Count);

            var viewListChanged = false;
            if (IsExpandedLeft || AutoExpandLeft)
            {
                AutoExpandLeft = false;
                IsExpandedLeft = true;

                if (ChildDelta != Item.ChildDelta)
                {
                    ChildDelta = Item.ChildDelta;

                    prev2.Clear();
                    foreach (var child in Items)
                    {
                        prev2[GetItemPair(child)] = child;
                    }
                    Clear();

                    SetCapacity(GetTotalCount());
                    foreach (var itm in GetChildItems())
                    {
                        if (prev2.TryGetValue(itm, out LineModel m))
                        {
                            Add(m);
                            prev2.Remove(GetItemPair(m));
                        }
                        else
                        {
                            CreateChildModel(itm);
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
            viewListChanged |= base.Validate(root, prev);
            return viewListChanged;
        }

    }
}
