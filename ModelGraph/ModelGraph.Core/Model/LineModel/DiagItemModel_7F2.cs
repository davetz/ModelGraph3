
namespace ModelGraph.Core
{
    public class DiagItemModel_7F2 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal DiagItemModel_7F2(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.DiagItemModel_7F2;

        public override (string, string) GetKindNameId(Root root) => (Item.GetKindId(root), Item.GetDoubleNameId(root)) ;
        internal override string GetFilterSortId(Root root) => $"{Item.GetKindId(root)} {Item.GetDoubleNameId(root)}";
    }
}
