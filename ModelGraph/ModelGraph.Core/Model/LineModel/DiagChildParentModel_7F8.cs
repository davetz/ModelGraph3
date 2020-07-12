
namespace ModelGraph.Core
{
    public class DiagChildParentModel_7F8 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal readonly (Item, Item) ItemPair;
        internal DiagChildParentModel_7F8(DiagParentListModel_7F6 owner, Relation item, (Item, Item) itemPair) : base(owner, item) { ItemPair = itemPair; }
        internal override IdKey IdKey => IdKey.DiagChildParentModel_7F8;
        public override (string, string) GetKindNameId(Root root) => (GetKindId(root), $"({ItemPair.Item1.GetParentId(root)} : {ItemPair.Item1.GetNameId(root)}) --> ({ItemPair.Item2.GetParentId(root)} : {ItemPair.Item2.GetNameId(root)})");
    }
}
