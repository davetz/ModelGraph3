using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_664_PairList : ListModelOf<PairX>
    {
        internal Model_664_PairList(Model_653_Enum owner, EnumX item) : base(owner, item) { }
        private EnumX EX => Item as EnumX;
        internal override IdKey IdKey => IdKey.Model_664_PairList;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => EX.Count;
        protected override IList<PairX> GetChildItems() => EX.Items;
        protected override void CreateChildModel(PairX childItem)
        {
            new Model_652_Pair(this, childItem);
        }
        #endregion

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => ItemCreated.Record(root, new PairX(EX, true))));
        }
    }
}
