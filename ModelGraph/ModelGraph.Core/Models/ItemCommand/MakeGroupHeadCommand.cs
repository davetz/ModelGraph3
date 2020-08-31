
using System;

namespace ModelGraph.Core
{
    public class MakeGroupHeadCommand : ItemCommand
    {
        internal override IdKey IdKey => IdKey.MakeGroupHeadCommand;

        internal MakeGroupHeadCommand(ItemModel owner, Action action) : base(owner, action) { }
    }
}
