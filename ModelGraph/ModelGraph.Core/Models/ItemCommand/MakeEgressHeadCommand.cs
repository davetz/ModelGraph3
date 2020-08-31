
using System;

namespace ModelGraph.Core
{
    public class MakeEgressHeadCommand : ItemCommand
    {
        internal override IdKey IdKey => IdKey.MakeEgressHeadCommand;

        internal MakeEgressHeadCommand(ItemModel owner, Action action) : base(owner, action) { }
    }
}
