
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class EnumXModel_653 : StoreModelOf<PairX>
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal EnumXModel_653(LineModel owner, EnumX item) : base(owner, item) { }
        private EnumX EX => Item as EnumX;
        internal override IdKey IdKey => IdKey.EnumXModel_653;

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => ItemCreated.Record(root, new PairX(EX, true))));
        }

        internal override void CreateChildModel(LineModel parentModel, PairX childItem)
        {
            new PairXModel_652(parentModel, childItem);
        }
    }
}
