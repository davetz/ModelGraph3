
using System;

namespace ModelGraph.Core
{
    public class CreateCommand : ItemCommand
    {
        internal override IdKey IdKey => IdKey.CreateCommand;

        internal CreateCommand(ItemModel owner, Action action) : base(owner, action) { }
    }
}
