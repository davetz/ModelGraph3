using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_666_ComputeList : List2ModelOf<TableX, ComputeX>
    {
        internal Model_666_ComputeList(Model_654_Table owner, TableX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_666_ComputeList;

        public override bool CanDrag => true;

        public override string GetNameId() => Item.Owner.Owner.GetNameId(IdKey);
        public override string GetKindId() => string.Empty;

        protected override int GetTotalCount() => Item.Owner.GetTotalCount(this);
        protected override IList<ComputeX> GetChildItems() => Item.Owner.GetChildItems(this);
        protected override void CreateChildModel(ComputeX cx) => new Model_658_Compute(this, cx);

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => Item.Owner.AddNewComputeX(this)));
        }
    }
}
