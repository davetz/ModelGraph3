
using System;

namespace ModelGraph.Core
{
    public class NewViewCommand : ItemCommand
    {
        internal override IdKey IdKey => IdKey.ViewCommand;

        internal NewViewCommand(ItemModel owner, Action action) : base(owner, action) { }
    }
}
