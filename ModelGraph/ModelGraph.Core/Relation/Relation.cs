using System.Collections.Generic;

namespace ModelGraph.Core
{
    public abstract class Relation : Item 
    {
        internal override State State { get; set; }
        internal Pairing Pairing;

        #region Serializer  ===================================================
        internal abstract (int, int)[] GetChildren1Items(Dictionary<Item, int> itemIndex);
        internal abstract (int, int)[] GetParent1Items(Dictionary<Item, int> itemIndex);
        internal abstract (int, int[])[] GetChildren2Items(Dictionary<Item, int> itemIndex);
        internal abstract (int, int[])[] GetParents2Items(Dictionary<Item, int> itemIndex);
        internal abstract void SetChildren1((int, int)[] items, Item[] itemArray);
        internal abstract void SetParents1((int, int)[] items, Item[] itemArray);
        internal abstract void SetChildren2((int, int[])[] items, Item[] itemArray);
        internal abstract void SetParents2((int, int[])[] items, Item[] itemArray);
        internal abstract bool HasLinks { get; }
        #endregion

 
        public override string GetKindId() => GetRoot().GetKindId(IdKey.Relation);

        #region RequiredMethods  ==============================================
        internal bool HasNoParent(Item key)
        {
            return !HasParentLink(key);
        }
        internal bool HasNoChildren(Item key)
        {
            return !HasChildLink(key);
        }
        internal abstract bool HasChildLink(Item key);
        internal abstract bool HasParentLink(Item key);

        internal abstract void TrySetPairing(Pairing pairing);
        internal abstract bool IsValidParentChild(Item parentItem, Item childItem);
        internal abstract int ChildCount(Item key);
        internal abstract int ParentCount(Item key);
        internal abstract bool RelationExists(Item key, Item childItem);
        internal abstract void InsertLink(Item parentItem, Item childItem, int parentIndex, int childIndex);
        internal abstract (int ParentIndex, int ChildIndex) AppendLink(Item parentItem, Item childItem);
        internal abstract (int ParentIndex, int ChildIndex) GetIndex(Item parentItem, Item childItem);
        internal abstract void RemoveLink(Item parentItem, Item childItem);
        internal abstract void MoveChild(Item key, Item item, int index);
        internal abstract void MoveParent(Item key, Item item, int index);
        internal abstract (int Index1, int Index2) GetChildrenIndex(Item key, Item item1, Item item2);
        internal abstract (int Index1, int Index2) GetParentsIndex(Item key, Item item1, Item item2);
        internal abstract int GetLinks(out List<Item> parents, out List<Item> children);
        internal abstract int GetLinksCount();
        internal abstract void SetLink(Item key, Item val, int capacity = 0); // used by storage file load
        internal abstract bool TryGetParents(Item key, out List<Item> parents);
        internal abstract bool TryGetChildren(Item key, out List<Item> children);
        internal abstract bool HasKey1(Item key);
        internal abstract bool HasKey2(Item key);
        internal abstract int KeyCount { get; }
        internal abstract int ValueCount { get; }

        internal abstract int GetChildLinkPairCount();
        internal abstract int GetParentLinkPairCount();
        internal abstract List<(Item, Item)> GetChildLinkPairList();
        internal abstract List<(Item, Item)> GetParentLinkPairList();

        internal abstract (Store, Store) GetHeadTail();

        #endregion
    }
}
