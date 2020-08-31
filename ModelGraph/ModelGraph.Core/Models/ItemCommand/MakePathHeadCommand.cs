
using System;

namespace ModelGraph.Core
{
    public class MakePathHeadCommand : ItemCommand
    {
        internal override IdKey IdKey => IdKey.MakePathHeadCommand;

        internal MakePathHeadCommand(ItemModel owner, Action action) : base(owner, action) { }
    }
}
