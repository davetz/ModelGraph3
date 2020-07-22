
namespace ModelGraph.Core
{
    public abstract class ItemOf<T> : Item where T : Item
    {
        internal T Owner;
        internal override Item GetOwner() => Owner;
    }
}
