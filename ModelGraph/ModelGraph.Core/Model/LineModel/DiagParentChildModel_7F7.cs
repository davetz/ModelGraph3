
namespace ModelGraph.Core
{
    public class DiagParentChildModel_7F7 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal readonly (Item, Item) ItemPair;
        internal DiagParentChildModel_7F7(DiagChildListModel_7F5 owner, Relation item, (Item, Item) itemPair) : base(owner, item) { ItemPair = itemPair; }
        internal override IdKey IdKey => IdKey.DiagParentChildModel_7F7;

        public override (string, string) GetKindNameId(Root root) => (GetKindId(root), $"({ItemPair.Item1.GetParentId(root)} : {ItemPair.Item1.GetNameId(root)}) --> ({ItemPair.Item2.GetParentId(root)} : {ItemPair.Item2.GetNameId(root)})");
    }
}
