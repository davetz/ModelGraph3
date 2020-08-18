
using System;

namespace ModelGraph.Core
{
    public class MakePathHeadCommand : LineCommand
    {
        internal override IdKey IdKey => IdKey.MakePathHeadCommand;

        internal MakePathHeadCommand(LineModel owner, Action action) : base(owner, action) { }
    }
}
