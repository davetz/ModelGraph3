using System;

namespace ModelGraph.Core
{

    public class DrawCommand : ItemCommand
    {
        private readonly IdKey _idKey;
        internal override IdKey IdKey => _idKey;
        internal DrawCommand(ItemModel owner, IdKey idKey, Action action) : base(owner, action) { _idKey = idKey; }
    }
}
