using System;

namespace ModelGraph.Core
{
    public class ChangeSet : StoreOf<ItemChange>
    {
        private static int SequenceCount;
        internal DateTime DateTime;
        internal ushort Sequence { get; private set; }
        internal override State State { get; set; }

        internal override IdKey IdKey => IdKey.ChangeSet;
        internal override string Name => $"#{Sequence}";

        #region Constructor  ==================================================
        internal ChangeSet(ChangeRoot owner)
        {
            Owner = owner;
            DateTime = DateTime.Now;
            Sequence = (ushort)++SequenceCount;
            IsVirgin = true;
            SetCapacity(7);
        }
        #endregion

        #region Properties/Methods  ===========================================
        internal ChangeRoot ChangeRoot => Owner as ChangeRoot;
        internal bool CanUndo => (!IsCongealed && !IsUndone);
        internal bool CanRedo => (!IsCongealed && IsUndone);
        internal bool CanMerge => ChangeRoot.CanMerge(this); 
        internal void Merge() { ChangeRoot.Mege(this); }

        internal void Undo()
        {
            foreach (var item in Items)
            {
                item.Undo();
            }
            IsUndone = true;
        }

        internal void Redo()
        {
            foreach (var item in Items)
            {
                item.Redo();
            }
            IsUndone = false;
        }
        #endregion
    }
}
