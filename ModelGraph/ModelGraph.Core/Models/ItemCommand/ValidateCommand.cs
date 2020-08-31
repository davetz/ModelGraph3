
using System;

namespace ModelGraph.Core
{
    public class ValidateCommand : ItemCommand
    {
        internal override IdKey IdKey => IdKey.ValidateCommand;

        internal ValidateCommand(ItemModel owner, Action action) : base(owner, action) { }
    }
}
