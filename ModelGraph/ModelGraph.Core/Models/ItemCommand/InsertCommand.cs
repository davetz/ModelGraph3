
using System;

namespace ModelGraph.Core
{
    public class InsertCommand : ItemCommand
    {
        internal override IdKey IdKey => IdKey.InsertCommand;
        internal InsertCommand(ItemModel owner, Action action) : base(owner, action) { }
    }
}
