
using System;

namespace ModelGraph.Core
{
    public class MakeGroupHeadCommand : LineCommand
    {
        internal override IdKey IdKey => IdKey.MakeGroupHeadCommand;

        internal MakeGroupHeadCommand(LineModel owner, Action action) : base(owner, action) { }
    }
}
