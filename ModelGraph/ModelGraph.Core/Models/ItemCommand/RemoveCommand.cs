
using System;

namespace ModelGraph.Core
{
    public class RemoveCommand : ItemCommand
    {
        internal override IdKey IdKey => IdKey.RemoveCommand;
        internal RemoveCommand(ItemModel owner, Action action) : base(owner, action) { }
    }
}
