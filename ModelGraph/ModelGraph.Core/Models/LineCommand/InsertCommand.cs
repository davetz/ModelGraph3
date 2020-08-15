
using System;

namespace ModelGraph.Core
{
    public class InsertCommand : LineCommand
    {
        internal override IdKey IdKey => IdKey.InsertCommand;
        public override bool IsInsertCommand => true;

        internal InsertCommand(LineModel owner, Action action) : base(owner, action) { }
    }
}
