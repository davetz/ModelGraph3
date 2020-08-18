
using System;

namespace ModelGraph.Core
{
    public class MakeGraphLinkCommand : LineCommand
    {
        internal override IdKey IdKey => IdKey.MakeGraphLinkCommand;

        internal MakeGraphLinkCommand(LineModel owner, Action action) : base(owner, action) { }
    }
}
