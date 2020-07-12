namespace ModelGraph.Core
{/*

 */
    public class ItemChildMoved : ItemChange
    {
        internal Item Key;
        internal Item Item;
        internal Relation Relation;
        internal int Index1;
        internal int Index2;

        internal override IdKey IdKey => IdKey.ItemChildMoved;

        #region Constructor  ==================================================
        private ItemChildMoved(ChangeSet owner, Relation relation, Item key, Item item, int index1, int index2, string name)
        {
            Owner = owner;
            _name = name;

            Key = key;
            Item = item;
            Relation = relation;
            Index1 = index1;
            Index2 = index2;

            owner.Add(this);
            UpdateDelta();
        }
        #endregion

        #region ItemChildMoved  ===============================================
        static internal void Record(Root root, Relation relation, Item key, Item item, int index1, int index2)
        {
            var n1 = index1 + 1;
            var n2 = index2 + 1;

            var name = $" [{relation.GetNameId(root)}]     {item.GetDoubleNameId(root)}     {n1}->{n2}";
            var chg = new ItemChildMoved(root.Get<ChangeRoot>().ChangeSet, relation, key, item, index1, index2, name);
            chg.Redo();
        }
        #endregion

        #region Undo/Redo  ====================================================
        override internal void Undo()
        {
            Relation.MoveChild(Key, Item, Index1);
            IsUndone = true;
        }

        override internal void Redo()
        {
            Relation.MoveChild(Key, Item, Index2);
            IsUndone = false;
        }
        #endregion
    }
}
