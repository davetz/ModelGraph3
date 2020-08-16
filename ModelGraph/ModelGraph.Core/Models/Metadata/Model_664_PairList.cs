using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_664_PairList : List2ModelOf<EnumX, PairX>
    {
        internal Model_664_PairList(Model_653_Enum owner, EnumX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_664_PairList;
        public override string GetNameId() => Item.Owner.Owner.GetNameId(IdKey);
        public override bool CanExpandAll => true;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => Item.Count;
        protected override IList<PairX> GetChildItems() => Item.Items;
        protected override void CreateChildModel(PairX childItem)
        {
            new Model_652_Pair(this, childItem);
        }
        #endregion

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => ItemCreated.Record(root, new PairX(Item, true))));
        }
    }
}
