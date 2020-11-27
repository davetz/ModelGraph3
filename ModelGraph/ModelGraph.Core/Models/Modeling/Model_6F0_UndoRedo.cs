
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6F0_UndoRedo : ItemModelOf<GraphModel>
    {
        internal Model_6F0_UndoRedo(Model_6DB_MoveNodeMenu owner, GraphModel item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_6F0_UndoRedo;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            var pageModel = GetPageModel();
            list.Clear();
            list.Add(new DrawCommand(this, IdKey.GraphUndoCommand, Item.Graph.TryUndo));
            list.Add(new DrawCommand(this, IdKey.GraphRedoCommand, Item.Graph.TryRedo));
        }
    }
}
