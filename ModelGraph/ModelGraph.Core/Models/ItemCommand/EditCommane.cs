
using System;

namespace ModelGraph.Core
{
    public class EditCommand : ItemCommand
    {
        internal override IdKey IdKey => IdKey.EditCommand;

        internal EditCommand(ItemModel owner, Action action) : base(owner, action) { }
    }
}
