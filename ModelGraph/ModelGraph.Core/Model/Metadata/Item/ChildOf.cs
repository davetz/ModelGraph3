
namespace ModelGraph.Core
{
    public abstract class ChildOf<T> : Item where T : Item
    {
        internal T Owner;
        internal override Item GetOwner() => Owner;
    }
}
