
using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public abstract class Store : Item
    {
        internal abstract void Add(Item item);
        internal abstract void Move(Item item, int index);
        internal abstract void Insert(Item item, int index);
        public abstract bool Remove(Item item);
        internal abstract bool IsValidOwnerOf(Item item);
        internal abstract void RemoveAll();
        internal abstract int IndexOf(Item item);
        internal abstract List<Item> GetItems();
        internal abstract int Count { get; }
        internal abstract Type GetChildType();

        internal bool TryLookUpProperty(string name, out Property property)
        {
            return DataRoot.TryLookUpProperty(this, name, out property);
        }
    }
}
