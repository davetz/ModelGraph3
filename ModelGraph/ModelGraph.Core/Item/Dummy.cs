
namespace ModelGraph.Core
{
    public class DummyItem : Item
    {
        internal override IdKey IdKey => IdKey.DummyItem;
        internal DummyItem(Root owner)
        {
            Owner = owner;
        }
    }
}
