using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class ChangeRoot : StoreOf<ChangeSet>
    {
        internal ChangeSet ChangeSet { get; private set; } //aggragates all changes made durring ModelRequest(Action)
        private string _infoText;
        private Item _infoItem;
        private int _infoCount;

        internal override IdKey IdKey => IdKey.ChangeRoot;

        #region Constructor  ==================================================
        internal ChangeRoot(Root root)
        {
            Owner = root;
            ChangeSet = new ChangeSet(this);
        }
        #endregion

        #region RecordChanges  ================================================
        /// <summary>Check for updates and save them</summary>
        internal void RecordChanges()
        {
            if (ChangeSet.Count > 0)
            {
                Add(ChangeSet);
                ChangeSet = new ChangeSet(this);
            }
        }
        #endregion


        #region Undo  =========================================================
        internal bool CanUndo(ChangeSet chg)
        {
            var last = Count - 1;
            for (int i = last; i >= 0; i--)
            {
                var item = Items[i];
                if (item == chg)
                    return item.CanUndo;
                else if (!item.CanRedo)
                    return false;
            }
            return false;
        }
        #endregion

        #region TryMerge  =====================================================
        internal void Mege(ChangeSet chg) => TryMerge(chg);
        internal bool CanMerge(ChangeSet chg) => TryMerge(chg, true);

        private bool TryMerge(ChangeSet cs, bool testIfCanMerge = false)
        {
            if (cs.IsCongealed) return false;

            var index = IndexOf(cs);
            if (index < 0) return false;
            var prev = index - 1;
            if (prev < 0) return false;

            var cs2 = Items[prev];
            if (cs2.IsCongealed) return false;
            if (cs2.IsUndone != cs.IsUndone) return false;

            if (testIfCanMerge) return true;

            cs.Owner.ModelDelta++;

            var items = cs2.Items;
            foreach (var item in items) { item.Reparent(cs); }
            Remove(cs2);

            //foreach (var cs3 in Items)
            //{
            //    cs3.ChildDelta++;
            //    cs3.ModelDelta++;
            //}
            return true;
        }
        #endregion

        #region CongealChanges  ===============================================
        internal void CongealChanges()
        {
            if (Count > 0)
            {
                ModelDelta++;
                ChildDelta++;
                ChangeSet save = null;
                var last = Count - 1;
                for (int i = last; i >= 0; i--)
                {
                    var cs = Items[i];
                    if (cs.IsCongealed) continue;
                    if (cs.IsUndone)
                        Remove(cs);
                    else if (save == null)
                        save = cs;
                }
                if (save != null)
                {
                    while (TryMerge(save)) { }
                    save.IsCongealed = true;
                }
            }
        }
        #endregion

        #region AutoExpandChanges  ============================================
        internal void AutoExpandChanges()
        {
            foreach (var chg in Items)
            {
                if (chg.IsCongealed) break;
                if (!chg.IsNew) break;
                chg.AutoExpandLeft = true;
                foreach (var item in chg.Items)
                {
                    item.AutoExpandLeft = true;
                }
            }
        }
        #endregion

        #region RemoveItem  ===================================================
        internal void RemoveItem(Item target)
        {
            var root = DataRoot;// big daddy
            var hitList = new List<Item>();//======================== dependant items that also need to be killed off
            var stoCRels = root.Get<Relation_Store_ChildRelation>();//==== souce1 of relational integrity
            var stoPRels = root.Get<Relation_Store_ParentRelation>();//==== souce2 of relational integrity
            var stoXCRels = root.Get<Relation_StoreX_ChildRelation>();//==== souce1 of relational integrity
            var stoXPRels = root.Get<Relation_StoreX_ParentRelation>();//==== souce2 of relational integrity
            var stoCols = root.Get<Relation_Store_ColumnX>(); //======= reference to user created columns
            var history = new Dictionary<Relation, Dictionary<Item, List<Item>>>(); //history of unlinked relationships

            FindDependents(target, hitList, stoCRels, stoXCRels);
            hitList.Reverse();

            foreach (var item in hitList)
            {
                if (item is Relation r)
                {
                    var N = r.GetLinks(out List<Item> parents, out List<Item> children);

                    for (int i = 0; i < N; i++) { ItemUnLinked.Record(ChangeSet, root, r, parents[i], children[i], history); }
                }
                if (TryGetParentRelations(item, out IList<Relation> relations, stoPRels, stoXPRels))
                {
                    foreach (var rel in relations)
                    {
                        if (!rel.TryGetParents(item, out List<Item> parents)) continue;

                        foreach (var parent in parents) { ItemUnLinked.Record(ChangeSet, root, rel, parent, item, history); }
                    }
                }
                if (TryGetChildRelations(item, out relations, stoCRels, stoXCRels))
                {
                    foreach (var rel in relations)
                    {
                        if (!rel.TryGetChildren(item, out List<Item> children)) continue;

                        foreach (var child in children) { ItemUnLinked.Record(ChangeSet, root, rel, item, child, history); }
                    }
                }
            }

            foreach (var item in hitList) { ItemRemoved.Record(ChangeSet, root, item); }

            ChangeSet.Redo(); //now finally do all changes in the correct order
        }
        #region PrivateMethods  ===========================================

        void FindDependents(Item target2, List<Item> hitList, Relation_Store_ChildRelation stoCRels, Relation_StoreX_ChildRelation stoXCRels)
        {
            hitList.Add(target2);
            if (target2 is Store store)
            {
                var items = store.GetItems();
                foreach (var item in items) FindDependents(item, hitList, stoCRels, stoXCRels);
            }
            if (TryGetChildRelations(target2, out IList<Relation> relations, stoCRels, stoXCRels))
            {
                foreach (var rel in relations)
                {
                    if (rel.IsRequired && rel.TryGetChildren(target2, out List<Item> children))
                    {
                        foreach (var child in children)
                        {
                            FindDependents(child, hitList, stoCRels, stoXCRels);
                        }
                    }
                }
            }
        }

        bool TryGetChildRelations(Item item, out IList<Relation> relations, Relation_Store_ChildRelation stoCRels, Relation_StoreX_ChildRelation stoXCRels)
        {
            if (item.Owner.IsExternal)
            {
                if (stoXCRels.TryGetChildren(item, out IList<Relation> txRelations))
                {
                    relations = new List<Relation>(txRelations);
                    return true;
                }
                relations = null;
                return false;
            }
            return stoCRels.TryGetChildren(item.Owner, out relations);
        }

        bool TryGetParentRelations(Item item, out IList<Relation> relations, Relation_Store_ParentRelation stoPRels, Relation_StoreX_ParentRelation stoXPRels)
        {
            if (item.Owner.IsExternal)
            {
                if (stoXPRels.TryGetChildren(item, out IList<Relation> txRelations))
                {
                    relations = new List<Relation>(txRelations);
                    return true;
                }
                relations = null;
                return false;
            }
            return stoPRels.TryGetChildren(item.Owner, out relations);
        }

        #endregion
        #endregion
    }
}
