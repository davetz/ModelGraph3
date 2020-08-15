using System.Collections.Generic;

namespace ModelGraph.Core
{
    public abstract class List1ModelOf<T> : ItemModelOf<T> where T : Item
    {
        #region List<LineModel> and Methods  ==================================
        private readonly List<LineModel> _items = new List<LineModel>(5);
        internal override List<LineModel> Items => _items;
        internal override int Count => _items.Count;
        internal override void Add(LineModel child) => _items.Add(child);
        internal override void Remove(LineModel child) => _items.Remove(child);
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

        internal List1ModelOf(LineModel owner, T item) : base(owner, item) { }
    }
}
