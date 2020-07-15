
namespace ModelGraph.Core
{
    public class Model_7F7_ParentChild : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal readonly (Item, Item) ItemPair;
        internal Model_7F7_ParentChild(Model_7F5_ChildList owner, Relation item, (Item, Item) itemPair) : base(owner, item) { ItemPair = itemPair; }
        internal override IdKey IdKey => IdKey.Model_7F7_ParentChild;

        public override (string, string) GetKindNameId(Root root) => (GetKindId(root), $"({ItemPair.Item1.GetParentId(root)} : {ItemPair.Item1.GetNameId(root)}) --> ({ItemPair.Item2.GetParentId(root)} : {ItemPair.Item2.GetNameId(root)})");
    }
}
