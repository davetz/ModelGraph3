
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public abstract class InternalManager<T1, T2> : ChildOfStoreOf<T1, T2>, ISerializer where T1 : Item where T2 : Item
    {
        public int GetSerializerItemCount()
        {
            return 1 + Count; //===================count my self and my children
        }

        public void PopulateItemIndex(Dictionary<Item, int> itemIndex)
        {
            itemIndex[this] = 0; //====================enter my self
            foreach (var itm in Items)
            {
                if (itm.IsReference) itemIndex[itm] = 0; //================enter my children
            }
        }

        public void RegisterInternal(Dictionary<int, Item> internalItems)
        {
            internalItems.Add(ItemKey, this);
            foreach (var item in Items)
            {
                if (item.IsReference) internalItems.Add(item.ItemKey, item);
            }
        }

        public bool HasData() => false;
        public void ReadData(DataReader r, Item[] items) { }
        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex) { }

    }
}
