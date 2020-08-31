using System.Collections.Generic;

namespace ModelGraph.Core
{
    public abstract class List1ModelOf<T> : ItemModelOf<T> where T : Item
    {
        #region List<ItemModel> and Methods  ==================================
        private readonly List<ItemModel> _items = new List<ItemModel>(5);
        internal override List<ItemModel> Items => _items;
        public override int Count => _items.Count;
        internal override void Add(ItemModel child) => _items.Add(child);
        internal override void Remove(ItemModel child) => _items.Remove(child);
        internal override void RemoveAt(int index) => _items.RemoveAt(index);
        internal override void Clear() => _items.Clear();
        internal void SetCapacity(int count) { if (count > _items.Capacity) _items.Capacity = count; }
        internal override void AddPropertyModel(PropertyModel model)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_items[i] is PropertyModel) continue;
                _items.Insert(i, model);
                return;
            }
            Add(model);
        }
        #endregion

        internal List1ModelOf(ItemModel owner, T item) : base(owner, item) { }
    }
}
