
using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;

namespace ModelGraph.Core
{
    public abstract class List2ModelOf<T1, T2> : List1ModelOf<T1> where T1 : Item where T2 : Item
    {
        internal List2ModelOf(LineModel owner, T1 item) : base(owner, item) { }

        public override bool CanExpandLeft => TotalCount > 0;
        public override bool CanFilter => TotalCount > 1;
        public override bool CanSort => TotalCount > 1;
        public override int TotalCount => GetTotalCount();

        #region RequiredMethodes  =============================================
        protected abstract int GetTotalCount();
        protected abstract IList<T2> GetChildItems();
        protected abstract void CreateChildModel(T2 childItem);
        #endregion

        internal override bool ExpandLeft(Root root)
        {
            if (!IsExpandedLeft)
            {
                var N = TotalCount;
                if (N > 0)
                {
                    IsExpandedLeft = true;
                    SetCapacity(N);
                    foreach (var itm in GetChildItems())
                    {
                        CreateChildModel(itm);
                    }
                    return true;
                }
            }
            return false;
        }
        internal override bool Validate(Root root, Dictionary<Item, LineModel> prev)
        {
            var viewListChanged = false;
            if (IsExpandedLeft || AutoExpandLeft)
            {
                AutoExpandLeft = false;
                IsExpandedLeft = true;

                if (ChildDelta != Item.ChildDelta)
                {
                    ChildDelta = Item.ChildDelta;
                    viewListChanged = true;

                    prev.Clear();
                    foreach (var child in Items)
                    {
                        prev[child.GetItem()] = child;
                    }
                    Clear();

                    SetCapacity(GetTotalCount());
                    foreach (var itm in GetChildItems())
                    {
                        if (prev.TryGetValue(itm, out LineModel m))
                        {
                            Add(m);
                            prev.Remove(m.GetItem());
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
            return viewListChanged || base.Validate(root, prev);
        }

    }
}
