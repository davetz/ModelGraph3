using System;

namespace ModelGraph.Core
{

    public abstract class ItemCommand : ChildOf<ItemModel>
    {
        internal Action Action { get; }

        internal ItemCommand(ItemModel model, Action action)
        {
            Owner = model;
            Action = action;
        }
        public void Execute() 
        {
            if (IsValid(GetOwner()))
                GetRoot().PostCommand(this);
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
