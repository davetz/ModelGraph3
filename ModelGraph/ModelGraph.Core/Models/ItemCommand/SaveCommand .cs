
using System;

namespace ModelGraph.Core
{
    public class SaveCommand : ItemCommand
    {
        internal override IdKey IdKey => IdKey.SaveCommand;

        internal SaveCommand(ItemModel owner, Action action) : base(owner, action) { }
    }
}
