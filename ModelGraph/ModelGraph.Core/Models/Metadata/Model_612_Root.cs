using System.Collections.Generic;
using Windows.Media.Core;

namespace ModelGraph.Core
{
    public class Model_612_Root : List1ModelOf<Root>
    {
        internal override IdKey IdKey => IdKey.Model_612_Root;
        public override bool CanExpandLeft => true;


        internal Model_612_Root(ItemModel owner, Root root) : base(owner, root)
        { 
            new Model_620_RootParm(this, Item);
            new Model_621_ErrorRoot(this, root.Get<ErrorRoot>());
            new Model_622_ChangeRoot(this, root.Get<ChangeManager>());
            new Model_623_MetadataRoot(this, Item);
            new Model_624_ModelingRoot(this, Item);

            IsExpandedLeft = true;
        }

        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            var pageModel = (Owner as TreeModel).PageModel;
            list.Clear();
            if (root.Repository.HasNoStorage)
                list.Add(new SaveAsCommand(this, pageModel.SaveAs));
            else
                list.Add(new SaveCommand(this, pageModel.Save));
            list.Add(new RelaodCommand(this, pageModel.Reload));
            list.Add(new CloseCommand(this, pageModel.Close));
        }
    }
}
