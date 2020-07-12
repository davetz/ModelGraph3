using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class ItemUnLinked : ItemChange
    {
        internal Item Child;
        internal Item Parent;
        internal Relation Relation;
        internal int ChildIndex;
        internal int ParentIndex;
        internal override IdKey IdKey => IdKey.ItemUnlinked;

        #region Constructor  ==================================================
        private ItemUnLinked(ChangeSet owner, Relation relation, Item parent, Item child, int parentIndex, int childIndex, string name)
        {
            Owner = owner;
            _name = name;

            Child = child;
            Parent = parent;
            Relation = relation;
            ChildIndex = parentIndex;
            ParentIndex = parentIndex;

            owner.Add(this);
            UpdateDelta();
        }
        #endregion

        #region Record  =======================================================
        internal static bool Record(ChangeSet owner, Root root, Relation rel, Item item1, Item item2, Dictionary<Relation, Dictionary<Item, List<Item>>> history)
        {
            #region track/check the history  ==================================
            // Avoid attempts to unlink the same relationship multiple times,
            // also update the historic record.
            List<Item> items;

            if (history.TryGetValue(rel, out Dictionary<Item, List<Item>> itemItems))
            {//======================================================================this relation has been here before
                if (itemItems.TryGetValue(item1, out items))
                {//============================================the parent item has been here before 
                    if (items.Contains(item2)) return false; //and the child item has already been unlinked, so there is nothing to do
                    items.Add(item2); //===================update the historic record
                }
                else
                {//============================================the parent item has not been here before 
                    items = new List<Item>(2) { item2 };
                    itemItems.Add(item1, items);//=========update the historic record
                }
            }
            else
            {//======================================================================this relation has not been here before
                itemItems = new Dictionary<Item, List<Item>>(4);
                items = new List<Item>(2) { item2 };
                itemItems.Add(item1, items);
                history.Add(rel, itemItems);//=========update the historic record
            }
            #endregion

            (int parentIndex, int childIndex) = rel.GetIndex(item1, item2);
            if (parentIndex < 0 || childIndex < 0) return false; //appearently the relationship doesn't exists

            var nam1 = item1.GetDoubleNameId(root);
            var nam2 = item2.GetDoubleNameId(root);
            var rnam = rel.GetNameId(root);

            var name = $" [{rnam}]   ({nam1}) --> ({nam2})";
            new ItemUnLinked(owner, rel, item1, item2, parentIndex, childIndex, name);

            return true;
        }
        internal static bool Record(Root root, Relation rel, Item item1, Item item2)
        {
            (int parentIndex, int childIndex) = rel.GetIndex(item1, item2);
            if (parentIndex < 0 || childIndex < 0) return false; //appearently the relationship doesn't exists

            var nam1 = item1.GetDoubleNameId(root);
            var nam2 = item2.GetDoubleNameId(root);
            var rnam = rel.GetNameId(root);

            var name = $" [{rnam}]   ({nam1}) --> ({nam2})";

            var ci = new ItemUnLinked(root.Get<ChangeRoot>().ChangeSet, rel, item1, item2, parentIndex, childIndex, name);
            ci.DoNow();

            return true;
        }
        #endregion

        #region Undo/Redo  ====================================================
        internal override void Redo()
        {
            Relation.RemoveLink(Parent, Child);
            IsUndone = false;
        }

        internal override void Undo()
        {
            Relation.InsertLink(Parent, Child, ParentIndex, ChildIndex);
            IsUndone = true;
        }
        #endregion
    }
}
