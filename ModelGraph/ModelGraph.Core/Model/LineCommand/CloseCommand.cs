
using System;

namespace ModelGraph.Core
{
    public class CloseCommand : LineCommand
    {
        internal override IdKey IdKey => IdKey.CloseCommand;

        internal CloseCommand(LineModel owner, Action action) : base(owner, action) { }
    }
}
