
namespace ModelGraph.Core
{
    public abstract class ItemChange : Item
    {
        internal string _name;
        internal override string Name { get => _name; }
        internal override State State { get; set; }


        internal ChangeSet Change => Owner as ChangeSet;
        internal bool CanUndo => !IsUndone;
        internal bool CanRedo => IsUndone;

        abstract internal void Undo();
        abstract internal void Redo();

        protected void DoNow() => Redo(); //Initial act of doing the requested change, it's the same as Redo

        protected void UpdateDelta()
        {
            Owner.ModelDelta++;
            Owner.Owner.ModelDelta++;
        }
    }
}
