
using System;

namespace ModelGraph.Core
{
    public class RelaodCommand : ItemCommand
    {
        internal override IdKey IdKey => IdKey.ReloadCommand;

        internal RelaodCommand(ItemModel owner, Action action) : base(owner, action) { }
    }
}
