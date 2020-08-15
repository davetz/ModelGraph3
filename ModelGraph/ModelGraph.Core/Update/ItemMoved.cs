namespace ModelGraph.Core
{
    public class ItemMoved : ItemChange
    {
        internal Item Item;
        internal int Index1; // the initial location before the move
        internal int Index2; // the ending location after the move
        internal override IdKey IdKey => IdKey.ItemMoved;

        #region Constructor  ==================================================
        private ItemMoved(ChangeSet owner, Item item, int index1, int index2, string name)
        {
            Owner = owner;
            _name = name;

            Item = item;
            Index1 = index1;
            Index2 = index2;

            owner.Add(this);
            UpdateDelta();
        }
        #endregion

        #region Record  =======================================================
        internal static void Record(Root root, Item item, int index1, int index2)
        {
            var n1 = index1 + 1;
            var n2 = index2 + 1;
            var name = $"{item.GetFullNameId()}     {n1}->{n2}";
            var cs = new ItemMoved(root.Get<ChangeManager>().ChangeSet, item, index1, index2, name);
            cs.DoNow();
        }
        #endregion

        #region Undo/Redo  ====================================================
        internal override void Undo()
        {
            var item = Item;
            var store = item.GetOwner() as Store;
            store.Move(item, Index1);
            store.ChildDelta++;
            IsUndone = true;
        }

        internal override void Redo()
        {
            var item = Item;
            var store = item.GetOwner() as Store;
            store.Move(item, Index2);
            store.ChildDelta++;
            IsUndone = false;
        }
        #endregion
    }
}
