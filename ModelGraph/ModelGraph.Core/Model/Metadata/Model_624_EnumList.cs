using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_624_EnumList : ListModelOf<EnumX>
    {
        internal Model_624_EnumList(Model_623_MetadataRoot owner, EnumXRoot item) : base(owner, item) { }
        private EnumXRoot EXR => Item as EnumXRoot;
        internal override IdKey IdKey => IdKey.Model_624_EnumList;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => EXR.Count;
        protected override IList<EnumX> GetChildItems() => EXR.Items;
        protected override void CreateChildModel(EnumX childItem)
        {
            new Model_653_Enum(this, childItem);
        }
        #endregion

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => ItemCreated.Record(root, new EnumX(EXR, true))));
        }

    }
}
