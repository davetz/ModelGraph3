namespace ModelGraph.Core
{
    public class ItemLinked : ItemChange
    {
        private Item _child;
        private Item _parent;
        private Relation _relation;
        private int _parentIndex;
        private int _childIndex;
        internal override IdKey IdKey =>  IdKey.ItemLinked;

        #region Constructor  ==================================================
        internal ItemLinked(ChangeSet owner, Relation relation, Item parent, Item child, int parentIndex, int childIndex, string name)
        {
            Owner = owner;
            _name = name;

            _child = child;
            _parent = parent;
            _relation = relation;
            _childIndex = parentIndex;
            _parentIndex = parentIndex;

            owner.Add(this);
        }
        #endregion

        #region Record  =======================================================
        static internal void Record(Root root, Relation rel, Item item1, Item item2)
        {
            var nam1 = item1.GetDoubleNameId(root);
            var nam2 = item2.GetDoubleNameId(root);
            var rnam = rel.GetNameId(root);

            var name = $" [{rnam}]   ({nam1}) --> ({nam2})";
            (int parentIndex, int chilldIndex) = rel.AppendLink(item1, item2);
            new ItemLinked(root.Get<ChangeRoot>().ChangeSet, rel, item1, item2, parentIndex, chilldIndex, name);
        }
        #endregion

        #region Undo/Redo  ====================================================
        override internal void Undo()
        {
            _relation.RemoveLink(_parent, _child);
            IsUndone = true;
        }

        override internal void Redo()
        {
            _relation.InsertLink(_parent, _child, _parentIndex, _childIndex);
            IsUndone = false;
        }
        #endregion
    }
}
