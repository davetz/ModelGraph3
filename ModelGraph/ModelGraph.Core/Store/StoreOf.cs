using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public abstract class StoreOf<T> : Store where T : Item
    {
        private List<T> _items = new List<T>(0);    // list of child items

        #region Count/Items/GetItems  =========================================
        public IList<T> Items => (_items is null) ? new List<T>(0).AsReadOnly() : _items.AsReadOnly(); // protected from accidental corruption
        internal override int Count => (_items is null) ? 0 : _items.Count;
        internal override List<Item> GetItems() => new List<Item>(_items);
        internal override void RemoveAll() { _items.Clear(); UpdateDelta(); }
        internal override Type GetChildType() => typeof(T);
        #endregion

        #region Methods  ======================================================
        internal override bool IsValidOwnerOf(Item item) => item is T;
        private T Cast(Item item) => (item is T child) ? child : throw new InvalidCastException("StoreOf");
        private void UpdateDelta() { ModelDelta++; ChildDelta++; }

        internal void SetCapacity(int exactCount)
        {
            if (_items is null) return;
            if (exactCount > 0)
            {
                var cap = (int)((exactCount + 1) * 1.1); // allow for modest expansion

                _items.Capacity = cap;
            }
        }

        // Covert  ============================================================
        internal void CovertAdd(T item) => _items.Add(item);
        internal void CovertRemove(T item) => _items.Remove(item);
        internal void CovertInsert(T item) => _items.Insert(0,item);
        internal void CovertClear() => _items.Clear();

        // Add  =============================================================
        internal void Add(T item)
        {
            _items.Add(item);
            UpdateDelta();
        }
        internal override void Add(Item item) => Add(Cast(item));

        // Remove  ==========================================================
        internal bool Remove(T item)
        {
            UpdateDelta();
            return _items.Remove(item);
        }
        // DiscardChildren  ===================================================
        /// <summary>Recursivly discard all child Items</summary>
        internal override void DiscardChildren()
        {
            foreach (var item in _items) 
            { 
                item.Discard(); 
            }
            _items.Clear();
        }
        public override bool Remove(Item item) => Remove(Cast(item));

        // Insert  ============================================================
        internal void Insert(T item, int index)
        {
            var i = (index < 0) ? 0 : index;

            UpdateDelta();
            if (i < _items.Count)
                _items.Insert(i, item);
            else
                _items.Add(item);
        }
        internal override void Insert(Item item, int index) => Insert(Cast(item), index); 

        // IndexOf  ===========================================================
        internal int IndexOf(T item) => (_items is null) ? -1 : _items.IndexOf(item);
        internal override int IndexOf(Item item) => IndexOf(Cast(item));

        // Move  ==============================================================
        internal void Move(T item, int index)
        {
            if (_items.Remove(item))
            {
                UpdateDelta();
                if (index < 0)
                    _items.Insert(0, item);
                else if (index < _items.Count)
                    _items.Insert(index, item);
                else
                    _items.Add(item);
            }
        }
        internal override void Move(Item item, int index) => Move(Cast(item), index);

        #endregion
    }
}
