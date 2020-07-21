
namespace ModelGraph.Core
{
    public abstract class PropertyModelOf<T> : PropertyModel where T : Item
    {
        internal T Item;
        internal override Item GetItem() => Item;

        internal PropertyModelOf(LineModel owner, T item, Property prop)
        {
            Item = item;
            Owner = owner;
            Property = prop;

            Depth = (byte)(owner.Depth + 1);

            owner.CovertAdd(this);
        }
    }
}
