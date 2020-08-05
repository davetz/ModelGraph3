
namespace ModelGraph.Core
{
    /// <summary>Does not have any child models</summary>
    public abstract class ItemModelOf<T> : LineModel where T : Item
    {
        internal T Item;
        internal override Item GetItem() => Item;

        internal ItemModelOf() { }

        public override (string, string) GetKindNameId() => Item.GetKindNameId();
        internal override string GetFilterSortId() => Item.GetNameId();

        internal ItemModelOf(LineModel owner, T item)
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
