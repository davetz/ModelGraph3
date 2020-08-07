
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_623_MetadataRoot : List1ModelOf<Root>
    {
        internal Model_623_MetadataRoot(LineModel owner, Root item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_623_MetadataRoot;

        public override string GetNameId() => Item.GetNameId(IdKey);

        public override bool CanExpandLeft => true;

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;
            IsExpandedLeft = true;

            new Model_631_ViewList(this, root.Get<ViewXRoot>());
            new Model_642_EnumList(this, root.Get<EnumXRoot>());
            new Model_643_TableList(this, root.Get<TableXRoot>());
            new Model_644_GraphList(this, root.Get<GraphXRoot>());
            new Model_7F0_Root(this, root);

            return true;
        }
        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            var treeModel = GetTreeModel();
            list.Clear();
            list.Add(new NewViewCommand(this, () => { treeModel.NewView( (m) => { new Model_623_MetadataRoot(m, Item); }); }));
        }
    }
}
