using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_642_EnumList : List2ModelOf<EnumXRoot,EnumX>
    {
        internal Model_642_EnumList(Model_623_MetadataRoot owner, EnumXRoot item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_642_EnumList;
        public override string GetNameId() => Item.Owner.GetNameId(IdKey);

        protected override int GetTotalCount() => Item.Count;
        protected override IList<EnumX> GetChildItems() => Item.Items;
        protected override void CreateChildModel(EnumX childItem)
        {
            new Model_653_Enum(this, childItem);
        }

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => ItemCreated.Record(root, new EnumX(Item, true))));
        }

    }
}
