
using System;

namespace ModelGraph.Core
{
    public class CloseCommand : ItemCommand
    {
        internal override IdKey IdKey => IdKey.CloseCommand;

        internal CloseCommand(ItemModel owner, Action action) : base(owner, action) { }
    }
}
