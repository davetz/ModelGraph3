namespace ModelGraph.Core
{
    public class ItemCreated : ItemChange
    {
        internal Item Item;
        internal int AtIndex; // remember item's location in it's parent.Items list 
        internal override IdKey IdKey => IdKey.ItemCreated;

        #region Constructor  ==================================================
        private ItemCreated(ChangeSet owner, Item item, int index, string name)
        {
            Owner = owner;
            _name = name;
            Item = item;
            AtIndex = index;

            owner.Add(this);
            UpdateDelta();
        }
        #endregion

        #region Record  =======================================================
        /// <summary>Record the new Item created event</summary>
        internal static void Record(Root root, Item item)
        {
            var cs = root.Get<ChangeRoot>().ChangeSet;

            string name = item.GetChangeLogId(root);
            var store = item.GetOwner() as Store;

            new ItemCreated(cs, item, store.IndexOf(item), name);
        }
        #endregion

        #region Undo/Redo  ====================================================
        internal override void Undo()
        {
            var item = Item;

            var store = item.GetOwner() as Store;
            store.Remove(item);
            IsUndone = true;
        }
        internal override void Redo()
        {
            var item = Item;
            var index = AtIndex;

            var store = item.GetOwner() as Store;
            store.Insert(item, index);
            IsUndone = false;
        }
        #endregion
    }
}
