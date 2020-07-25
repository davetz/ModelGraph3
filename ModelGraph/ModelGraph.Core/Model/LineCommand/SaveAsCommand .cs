
using System;

namespace ModelGraph.Core
{
    public class SaveAsCommand : LineCommand
    {
        internal override IdKey IdKey => IdKey.SaveAsCommand;

        internal SaveAsCommand(LineModel owner, Action action) : base(owner, action) { }
    }
}
