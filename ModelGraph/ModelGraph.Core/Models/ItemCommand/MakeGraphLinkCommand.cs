
using System;

namespace ModelGraph.Core
{
    public class MakeGraphLinkCommand : ItemCommand
    {
        internal override IdKey IdKey => IdKey.MakeGraphLinkCommand;

        internal MakeGraphLinkCommand(ItemModel owner, Action action) : base(owner, action) { }
    }
}
