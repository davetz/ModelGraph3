
using System;

namespace ModelGraph.Core
{
    public class ValidateCommand : LineCommand
    {
        internal override IdKey IdKey => IdKey.ValidateCommand;

        internal ValidateCommand(LineModel owner, Action action) : base(owner, action) { }
    }
}
