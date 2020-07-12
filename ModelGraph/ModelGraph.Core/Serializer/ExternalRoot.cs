
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public abstract class ExternalRoot<T> : StoreOf<T> where T : Item
    {
        public bool HasData() => Count > 0;

        public int GetSerializerItemCount()
        {
            var count = 1 + Count; //===================count my self and my children
            foreach (var child in Items)
            {
                if (child is Store st2)
                {
                    count += st2.Count; //=============count my grandchildren
                    var grandchildren = st2.GetItems();
                    foreach (var grandchild in grandchildren)
                    {
                        if (grandchild is Store st3)
                        {
                            count += st3.Count; //=====count my greatgrandchildren
                        }
                    }
                }
            }
            return count;
        }

        public void PopulateItemIndex(Dictionary<Item, int> itemIndex)
        {
            itemIndex[this] = 0; //====================enter my self
            foreach (var child in Items)
            {
                itemIndex[child] = 0; //================enter my child
                if (child is Store st2)
                {
                    var grandchildren = st2.GetItems();
                    foreach (var grandchild in grandchildren)
                    {
                        itemIndex[grandchild] = 0; //========enter my grandchild
                        if (grandchild is Store st3)
                        {
                            var greatgrandchildren = st3.GetItems();
                            foreach (var greatgrandchild in greatgrandchildren)
                            {
                                itemIndex[greatgrandchild] = 0; //====enter my greatgrandchild
                            }
                        }
                    }
                }
            }
        }
    }
}
