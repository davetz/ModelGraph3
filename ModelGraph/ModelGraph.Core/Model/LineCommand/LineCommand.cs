using System;

namespace ModelGraph.Core
{

    public abstract class LineCommand : ChildOf<LineModel>
    {
        internal Action Action { get; }

        internal LineCommand(LineModel model, Action action)
        {
            Owner = model;
            Action = action;
        }
        public void Execute() 
        {
            if (IsValid(GetOwner()))
                DataRoot.PostCommand(this);
            if (IsInsertCommand)
            {
                Owner.AutoExpandLeft = true;
                Owner.ChildDelta -= 2;
            }
        }

        public virtual bool IsRemoveCommand => false;
        public virtual bool IsInsertCommand => false;
    }
}
