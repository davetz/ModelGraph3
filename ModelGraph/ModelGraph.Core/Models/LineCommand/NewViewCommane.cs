
using System;

namespace ModelGraph.Core
{
    public class NewViewCommand : LineCommand
    {
        internal override IdKey IdKey => IdKey.ViewCommand;

        internal NewViewCommand(LineModel owner, Action action) : base(owner, action) { }
    }
}
