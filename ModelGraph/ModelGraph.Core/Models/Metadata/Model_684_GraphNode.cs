using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_684_GraphNode : List2ModelOf<Store, SymbolX>
    {
        internal Model_684_GraphNode(Model_683_GraphNodeList owner, Store item) : base(owner, item) { } 
        internal override IdKey IdKey => IdKey.Model_684_GraphNode;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => 0;
        protected override IList<SymbolX> GetChildItems() => new SymbolX[0];
        protected override void CreateChildModel(SymbolX childItem)
        {
        }
        #endregion
    }
}
