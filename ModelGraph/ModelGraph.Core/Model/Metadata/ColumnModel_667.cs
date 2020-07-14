
namespace ModelGraph.Core
{
    public class ColumnModel_667 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal ColumnModel_667(ColumnListModel_665 owner, ColumnX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ColumnModel_667;
        public override bool CanDrag => true;

        public override (string, string) GetKindNameId(Root root) => (Item.GetKindId(root), Item.GetDoubleNameId(root));
    }
}
