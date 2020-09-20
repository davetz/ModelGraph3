
namespace ModelGraph.Core
{
    public class Model_600_Dummy : ItemModelOf<TreeModel>
    {
        
        internal Model_600_Dummy(TreeModel owner) : base(owner, owner) {}
        internal override IdKey IdKey => IdKey.Model_600_Dummy;

        public override string GetNameId() => "Dummy Model";
        public override string GetSummaryId() => "The treeModel has no HeaderModel";
    }
}
