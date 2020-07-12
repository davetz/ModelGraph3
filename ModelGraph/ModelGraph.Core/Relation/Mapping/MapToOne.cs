using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class MapToOne<T> : Dictionary<Item, T> where T : Item
    {
        internal MapToOne(int capacity = 0) : base(capacity) { }

        #region Serializer  ===================================================
        internal MapToOne((int, int)[] items, Item[] itemArray) : base(items.Length)
        {
            foreach (var (ix1, ix2) in items)
            {
                if (ix1 < 0 || ix1 > itemArray.Length)
                    throw new Exception($"MapToMany invalid index1: {ix1}");

                var p = itemArray[ix1];
                if (p is null)
                    throw new Exception($"MapToMany item1 is null for index1: {ix1}");

                if (ix2 < 0 || ix2 > itemArray.Length)
                    throw new Exception($"MapToMany invalid index2: {ix2}");

                if (!(itemArray[ix2] is T c))
                    throw new Exception($"MapToMany item2 is null for index2: {ix2}");

                this[p] = c;
            }
        }
        internal (int, int)[] GetItems(Dictionary<Item, int> itemIndex)
        {
            var items = new (int, int)[Count];
            var i = 0;
            foreach (var e in this)
            {
                if (!itemIndex.TryGetValue(e.Key, out int ix1))
                    throw new Exception("MaptoMany GetItems: item not in itemIndex dictionary");

                if (!itemIndex.TryGetValue(e.Value, out int ix2))
                    throw new Exception("MaptoMany GetItems: item not in itemIndex dictionary");

                items[i++] = (ix1, ix2);
            }
            return items;
        }
        #endregion

        internal int KeyCount { get { return Count; } }
        internal int ValueCount { get { return Count; } }

        internal int GetLinksCount()
        {
            var count = 0;
            foreach (var val in this)
            {
                if (val.Value.IsExternal || val.Key.IsExternal) count += 1;
            }
            return count;
        }

        internal int GetLinks(out List<Item> parents, out List<Item> children)
        {
            var count = GetLinksCount();
            children = new List<Item>(count);
            parents = new List<Item>(count);

            foreach (var val in this)
            {
                if (val.Value.IsExternal || val.Key.IsExternal)
                {
                    children.Add(val.Value);
                    parents.Add(val.Key);
                }
            }
            return count;
        }

        internal void SetLink(Item key, T val, int capacity = 0)
        {
            this[key] = val;
        }

        internal int GetValCount(Item key)
        {
            return ContainsKey(key) ? 1 : 0;
        }

        internal void AppendLink(Item key, T val)
        {
            SetLink(key, val);
        }

        internal void InsertLink(Item key, T val, int index)
        {
            SetLink(key, val);
        }
        internal int GetIndex(Item key, T val)
        {
            return (TryGetValue(key, out T value) && value == val) ? 0 : -1;
        }

        internal void RemoveLink(Item key, T val)
        {
            if (TryGetValue(key, out T value) && val == value) Remove(key);
        }

        internal bool TryGetVal(Item key, out T val) => TryGetValue(key, out val);

        internal bool TryGetVals(Item key, out List<Item> vals)
        {
            if (TryGetValue(key, out T value))
            {
                vals = new List<Item>(1) { value };
                return true;
            }
            vals = null;
            return false;
        }

        internal bool TryGetVals(Item key, out List<T> vals)
        {
            if (TryGetValue(key, out T value))
            {
                vals = new List<T>(1) { value };
                return true;
            }
            vals = null;
            return false;
        }

        internal bool ContainsLink(Item key, T val)
        {
            return (TryGetValue(key, out T value) && value == val);
        }

        internal bool CanMapToOne { get { return true; } }


        // used by DiagChildListModel_7F5 and DiagParentListModel_7F6
        internal int GetLinkPairCount() => Count;

        internal List<(Item, Item)> GetLinkPairList()
        {
            var list = new List<(Item, Item)>(Count);
            foreach (var val in this)
            {
                list.Add((val.Key, val.Value));
            }
            return list;
        }
    }
}
