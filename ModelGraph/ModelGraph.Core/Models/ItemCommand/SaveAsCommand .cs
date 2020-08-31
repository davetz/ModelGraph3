
using System;

namespace ModelGraph.Core
{
    public class SaveAsCommand : ItemCommand
    {
        internal override IdKey IdKey => IdKey.SaveAsCommand;

        internal SaveAsCommand(ItemModel owner, Action action) : base(owner, action) { }
    }
}
