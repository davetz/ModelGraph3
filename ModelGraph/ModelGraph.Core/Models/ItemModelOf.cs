
namespace ModelGraph.Core
{
    /// <summary>Does not have any child models</summary>
    public abstract class ItemModelOf<T> : ItemModel where T : Item
    {
        internal T Item;
        internal override Item GetItem() => Item;
        public override string GetKindId() => Item.GetKindId();
        public override string GetNameId() => Item.GetNameId();
        public override string GetSummaryId() => Item.GetSummaryId();
        public override string GetDescriptionId() => Item.GetDescriptionId();

        internal ItemModelOf() { }
        internal ItemModelOf(ItemModel owner, T item)
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
