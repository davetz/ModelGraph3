
using System;

namespace ModelGraph.Core
{
    public class SaveCommand : LineCommand
    {
        internal override IdKey IdKey => IdKey.SaveCommand;

        internal SaveCommand(LineModel owner, Action action) : base(owner, action) { }
    }
}
