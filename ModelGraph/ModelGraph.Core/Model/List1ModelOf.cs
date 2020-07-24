using System.Collections.Generic;

namespace ModelGraph.Core
{
    public abstract class List1ModelOf<T> : ItemModelOf<T> where T : Item
    { 
        private readonly List<LineModel> _items;
        internal override List<LineModel> Items => _items;
        internal override int Count => _items.Count;
        internal override void Add(LineModel child) => _items.Add(child);
        internal override void Remove(LineModel child) => _items.Remove(child);
        internal override void RemoveAt(int index) => _items.RemoveAt(index);
        internal override void Clear() => _items.Clear();
        internal void SetCapacity(int count) { if (count > _items.Capacity) _items.Capacity = count; }

        internal override void AddPropertyModel(PropertyModel child)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_items[i] is PropertyModel) continue;
                _items.Insert(i, child);
                return;
            }
            Add(child);
        }

        internal List1ModelOf(LineModel owner, T item, int size = 10) : base(owner, item) 
        { 
            _items = new List<LineModel>(size);
        }
    }
}
