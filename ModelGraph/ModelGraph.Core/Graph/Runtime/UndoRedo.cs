using System.Collections.Generic;

namespace ModelGraph.Core
{/*
 */
    public partial class Graph
    {
        private Stack<Snapshot> _undoStack = new Stack<Snapshot>();
        private Stack<Snapshot> _redoStack = new Stack<Snapshot>();

        internal void TakeSnapshot(Selector selector)
        {
            _undoStack.Push(new Snapshot(selector));
        }
        private void ClearUndoRedo()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }


        private bool CanUndo { get { return _undoStack.Count > 0; } }
        private bool CanRedo { get { return _redoStack.Count > 0; } }


        internal (bool canUndo, bool canRedo, int undoCount, int redoCount) UndoRedoParms => (CanUndo, CanRedo, _undoStack.Count, _redoStack.Count);

        internal void TryUndo()
        {
            if (CanUndo)
            {
                 var prev = _undoStack.Pop();

                _redoStack.Push(new Snapshot(prev));
                prev.Restore();
                UpdateModels();
            }
        }

        internal void TryRedo()
        {
            if (CanRedo)
            {
                var prev = _redoStack.Pop();

                _undoStack.Push(new Snapshot(prev));
                prev.Restore();
                UpdateModels();
            }
        }

        private void UpdateModels()
        {
            ModelDelta++;
            var root = Owner.Owner.Owner;
            foreach (var pm in root.Items)
            {
                if (pm.LeadModel is GraphModel gm && gm.Graph == this) gm.FullRefresh();
            }
        }

    }
}
