
using System;

namespace ModelGraph.Core
{
    public class RemoveCommand : LineCommand
    {
        internal override IdKey IdKey => IdKey.RemoveCommand;
        public override bool IsRemoveCommand => true;

        internal RemoveCommand(LineModel owner, Action action) : base(owner, action) { }
    }
}
