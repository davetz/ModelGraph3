
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class PairListModel_664 : ListModelOf<PairX>
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal PairListModel_664(EnumModel_653 owner, EnumX item) : base(owner, item) { }
        private EnumX EX => Item as EnumX;
        internal override IdKey IdKey => IdKey.PairListModel_664;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => EX.Count;
        protected override IList<PairX> GetChildItems() => EX.Items;
        protected override void CreateChildModel(PairX childItem)
        {
            new PairModel_652(this, childItem);
        }
        #endregion

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => ItemCreated.Record(root, new PairX(EX, true))));
        }
    }
}
