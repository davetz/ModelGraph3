using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_647_TableList : List2ModelOf<TableXRoot, TableX>
    {
        internal Model_647_TableList(Model_624_ModelingRoot owner, TableXRoot item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_647_TableList;
        public override string GetNameId() => Root.GetNameId(IdKey);

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => Item.Count;
        protected override IList<TableX> GetChildItems() => Item.Items;
        protected override void CreateChildModel(TableX childItem)
        {
            new Model_6A4_Table(this, childItem);
        }
        #endregion
    }
}
