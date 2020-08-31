
namespace ModelGraph.Core
{
    public class Model_ItemPair : ItemModelOf<Graph>
    {
        internal readonly (Query, Query) ItemPair;
        internal Model_ItemPair(ItemModel owner, Graph item, (Query, Query) itemPair) : base(owner, item) { ItemPair = itemPair; }

        public override string GetNameId()
        {
            var head = ItemPair.Item1.GetParent();
            var tail = ItemPair.Item2.GetParent();
            var headId = head.IsExternal ? head.GetNameId() : head.GetKindId();
            var tailId = tail.IsExternal ? tail.GetNameId() : tail.GetKindId();

            return $"({headId} : {ItemPair.Item1.GetNameId()}) --> ({tailId} : {ItemPair.Item2.GetNameId()})";
        }
    }
}
