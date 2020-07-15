
namespace ModelGraph.Core
{
    public class Model_667_Column : LineModel
    {
        internal Model_667_Column(Model_665_ColumnList owner, ColumnX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_667_Column;
        public override bool CanDrag => true;

        public override (string, string) GetKindNameId(Root root) => (Item.GetKindId(root), Item.GetDoubleNameId(root));
    }
}
