
using System;

namespace ModelGraph.Core
{
    public class CreateCommand : LineCommand
    {
        internal override IdKey IdKey => IdKey.CreateCommand;

        internal CreateCommand(LineModel owner, Action action) : base(owner, action) { }
    }
}
