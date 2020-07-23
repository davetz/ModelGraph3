
using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;

namespace ModelGraph.Core
{
    public abstract class ListModelOf<T1, T2> : LineModelOf<T1> where T1 : Item where T2 : Item
    {
        private List<LineModel> _items = new List<LineModel>();
        internal override List<LineModel> Items => _items;
        internal override int Count => _items.Count;
        internal override void Add(LineModel child) => _items.Add(child);
        internal override void Remove(LineModel child) => _items.Remove(child);
        internal override void Clear() => _items.Clear();
        internal void SetCapacity(int count) { if (count > _items.Capacity) _items.Capacity = count; }

        internal ListModelOf(LineModel owner, T1 item) : base(owner, item) { }

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
            if (IsExpandedLeft) return false;

            IsExpandedLeft = true;

            foreach (var itm in GetChildItems())
            {
                CreateChildModel(itm);
            }

            return true;
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

                    prev.Clear();
                    foreach (var child in Items)
                    {
                        prev[child.GetItem()] = child;
                    }
                    Clear();

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
