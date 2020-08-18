
using System;

namespace ModelGraph.Core
{
    public class MakeEgressHeadCommand : LineCommand
    {
        internal override IdKey IdKey => IdKey.MakeEgressHeadCommand;

        internal MakeEgressHeadCommand(LineModel owner, Action action) : base(owner, action) { }
    }
}
