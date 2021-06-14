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

        public bool IsRemoveCommand => IdKey == IdKey.RemoveCommand; //used by the UI for Delete acceleratorKey
        public bool IsInsertCommand => IdKey == IdKey.InsertCommand; //used by the UI for Insert acceleratorKey
    }
}
