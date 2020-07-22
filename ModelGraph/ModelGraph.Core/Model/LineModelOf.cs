using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGraph.Core
{
    public abstract class LineModelOf<T> : LineModel where T : Item
    {
        internal T Item;
        internal override Item GetItem() => Item;

        internal LineModelOf() { }

        internal LineModelOf(LineModel owner, T item)
        {
            Item = item;
            Owner = owner;
            Depth = (byte)(owner.Depth + 1);

            if (item.AutoExpandRight)
            {
                item.AutoExpandRight = false;
                ExpandRight(item.GetRoot());
            }

            owner.Add(this);
        }
    }
}
