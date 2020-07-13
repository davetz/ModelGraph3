
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class EnumListModel_624 : StoreModelOf<EnumX>
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal EnumListModel_624(MetadataRootModel_623 owner, EnumXRoot item) : base(owner, item) { }
        private EnumXRoot EXR => Item as EnumXRoot;
        internal override IdKey IdKey => IdKey.EnumListModel_624;

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => ItemCreated.Record(root, new EnumX(EXR, true))));
        }

        internal override void CreateChildModel(LineModel parentModel, EnumX childItem)
        {
            new EnumXModel_653(parentModel, childItem);
        }
    }
}
