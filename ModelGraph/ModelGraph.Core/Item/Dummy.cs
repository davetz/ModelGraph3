
namespace ModelGraph.Core
{
    public class DummyItem : ItemOf<Root>
    {
        internal override IdKey IdKey => IdKey.DummyItem;
        internal DummyItem(Root owner)
        {
            Owner = owner;
        }
    }
}
