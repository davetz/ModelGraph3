using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Shapes;

namespace ModelGraph.Core
{
    public abstract class StaticModelOf<T> : LineModelOf<T> where T : Item
    { 
        private readonly List<LineModel> _items;
        internal override List<LineModel> Items => _items;
        internal override int Count => _items.Count;
        internal override void Add(LineModel child) => _items.Add(child);
        internal override void Remove(LineModel child) => _items.Remove(child);
        internal override void Clear() => _items.Clear();
        internal void SetCapacity(int count) { if (count > _items.Capacity) _items.Capacity = count; }

        internal StaticModelOf(LineModel owner, T item, int size = 10) : base(owner, item) 
        { 
            _items = new List<LineModel>(size);
        }
    }
}
