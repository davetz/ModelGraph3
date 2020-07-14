
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class EnumListModel_624 : ListModelOf<EnumX>
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal EnumListModel_624(MetadataRootModel_623 owner, EnumXRoot item) : base(owner, item) { }
        private EnumXRoot EXR => Item as EnumXRoot;
        internal override IdKey IdKey => IdKey.EnumListModel_624;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => EXR.Count;
        protected override IList<EnumX> GetChildItems() => EXR.Items;
        protected override void CreateChildModel(EnumX childItem)
        {
            new EnumModel_653(this, childItem);
        }
        #endregion

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => ItemCreated.Record(root, new EnumX(EXR, true))));
        }

    }
}
