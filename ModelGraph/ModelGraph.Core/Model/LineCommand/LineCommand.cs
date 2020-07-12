using System;

namespace ModelGraph.Core
{

    public abstract class LineCommand : Item
    {
        internal Action Action { get; }

        internal LineCommand(LineModel model, Action action)
        {
            Owner = model;
            Action = action;
        }
        public void Execute() 
        {
            if (IsValid(Owner))
                DataRoot.PostCommand(this);
            if (IsInsertCommand)
            {
                var model = Owner as LineModel;
                model.AutoExpandLeft = true;
                model.ChildDelta -= 2;
            }
        }

        public virtual bool IsRemoveCommand => false;
        public virtual bool IsInsertCommand => false;
    }
}
