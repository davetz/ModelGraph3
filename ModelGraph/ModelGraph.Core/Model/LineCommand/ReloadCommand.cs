
using System;

namespace ModelGraph.Core
{
    public class RelaodCommand : LineCommand
    {
        internal override IdKey IdKey => IdKey.ReloadCommand;

        internal RelaodCommand(LineModel owner, Action action) : base(owner, action) { }
    }
}
