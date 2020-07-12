using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class MapToMany<T> : Dictionary<Item, List<T>> where T : Item
    {
        internal MapToMany(int capacity = 0) : base(capacity) { }

        internal int KeyCount => Count;
        internal int ValueCount { get { var n = 0; foreach (var e in this) { n += e.Value.Count; } return n; } }

        #region Serializer  ===================================================
        internal MapToMany((int, int[])[] items, Item[] itemArray ) : base(items.Length)
        {
            foreach (var (ix1, ix2List) in items)
            {
                if (ix1 < 0 || ix1 > itemArray.Length)
                    throw new Exception($"MapToMany invalid index1: {ix1}");

                var p = itemArray[ix1];
                if (p is null)
                    throw new Exception($"MapToMany item1 is null for index1: {ix1}");

                var lst = new List<T>(ix2List.Length);
                foreach (var ix2 in ix2List)
                {
                    if (ix2 < 0 || ix2 > itemArray.Length)
                        throw new Exception($"MapToMany invalid index2: {ix2}");

                    if (!(itemArray[ix2] is T c))
                        throw new Exception($"MapToMany item2 is null for index2: {ix2}");

                    lst.Add(c);
                }
                this[p] = lst;
            }
        }

        internal (int, int[])[] GetItems(Dictionary<Item, int> itemIndex)
        {
            var items = new (int, int[])[Count];
            var i = 0;
            foreach (var e in this)
            {
                if (!itemIndex.TryGetValue(e.Key, out int ix1)) 
                    throw new Exception("MaptoMany GetItems: item not in itemIndex dictionary");

                if (e.Value != null && e.Value.Count > 0)
                {
                    var ix2List = new int[e.Value.Count];
                    var j = 0;
                    foreach (var item in e.Value)
                    {
                        if (!itemIndex.TryGetValue(item, out int ix2))
                            throw new Exception("MaptoMany GetItems: item not in itemIndex dictionary");

                        ix2List[j++] = ix2;
                    }
                    items[i++] = (ix1, ix2List);
                }
            }
            return items;
        }
        #endregion


        internal int GetLinksCount()
        {
            var n = 0;
            foreach (var e in this)
            {
                if (e.Key.IsExternal) n += e.Value.Count;
                else foreach (var v in e.Value) { if (v.IsExternal) n++; }
            }
            return n;
        }

        internal int GetLinks(out List<Item> parents, out List<Item> children)
        {
            var n = GetLinksCount();
            children = new List<Item>(n);
            parents = new List<Item>(n);

            foreach (var e in this)
            {
                foreach (var v in e.Value)
                {
                    if (e.Key.IsExternal || v.IsExternal)
                    {
                        children.Add(v);
                        parents.Add(e.Key);
                    }
                }
            }
            return n;
        }

        internal void SetLink(Item key, T val, int capacity = 0)
        {
            if (TryGetValue(key, out List<T> values))
            {
                values.Add(val);
                return;
            }

            values = (capacity > 0) ? new List<T>(capacity) : new List<T>(1);
            values.Add(val);
            Add(key, values);
        }

        internal int GetValCount(Item key) => TryGetValue(key, out List<T> vals) ? vals.Count : 0;

        internal void InsertLink(Item key, T val, int index)
        {
            if (TryGetValue(key, out List<T> values))
            {
                values.Remove(val);
                if (index < 0) values.Insert(0, val);
                else if (values.Count > index) values.Insert(index, val);
                else values.Add(val);
            }
            else
            {
                values = new List<T>(1) { val };
                Add(key, values);
            }
        }

        internal int GetIndex(Item key, T val) => TryGetValue(key, out List<T> vals) ? vals.IndexOf(val) : -1;

        internal void Move(Item key, T val, int index)
        {
            if (TryGetValue(key, out List<T> values))
            {
                if (values.Remove(val))
                {
                    if (index < 0)
                        values.Insert(0, val);
                    else if (index < values.Count)
                        values.Insert(index, val);
                    else
                        values.Add(val);
                }
            }
        }

        internal void RemoveLink(Item key, T val)
        {
            if (TryGetValue(key, out List<T> values))
            {
                values.Remove(val);
                if (values.Count == 0) Remove(key);
            }
        }

        internal bool TryGetVal(Item key, out T val)
        {
            if (TryGetValue(key, out List<T> values))
            {
                val = values[0];
                return true;
            }
            val = null;
            return false;
        }

        internal bool TryGetVals(Item key, out IList<T> vals)
        {
            if (TryGetValue(key, out List<T> values))
            {
                vals = values.AsReadOnly(); // protected from acidental corruption
                return true;
            }
            vals = null;
            return false;
        }

        internal bool ContainsLink(Item key, T val) => (TryGetValue(key, out List<T> values) && values.Contains(val));

        /// <summary>
        /// Can this mapToMany dictionary be replaced by a mapToOne dictionary
        /// </summary>
        internal bool CanMapToOne
        {
            get
            {
                foreach (var e in this) { if (e.Value.Count > 1) return false; }
                return true;
            }
        }

        // used by DiagChildListModel_7F5 and DiagParentListModel_7F6
        internal int GetLinkPairCount()
        {
            var n = 0;
            foreach (var e in this) { n += e.Value.Count; }
            return n;
        }

        internal List<(Item, Item)> GetLinkPairList()
        {
            var list = new List<(Item, Item)>(GetLinkPairCount());
            foreach (var e in this)
            {
                var itm1 = e.Key;
                foreach (var itm2 in e.Value) { list.Add((itm1, itm2)); }
            }
            return list;
        }
    }
}
