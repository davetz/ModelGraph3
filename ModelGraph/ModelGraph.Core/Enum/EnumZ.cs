
namespace ModelGraph.Core
{
    public abstract class EnumZ : StoreOf<PairZ>
    {
        internal EnumZ() { }

        #region GetEnumKey  ==================================================
        internal int GetKey(Root root, string name)
        {
            foreach (var pz in Items)
            {
                if (name == pz.GetNameId(root)) return pz.EnumKey;
            }
            return 0;
        }
        internal int GetActualValueAt(int index) => (index < 0 || index >= Count) ? 0 : Items[index].EnumKey;
        #endregion

        #region GetEnumIndex  ================================================
        internal int GetEnumIndex(Root root, string name)
        {
            for (int i = 0; i < Count; i++)
            {
                var pz = Items[i];
                if (name == pz.GetNameId(root)) return i;
            }
            return 0;
        }
        internal int GetEnumIndex(int key)
        {
            for (int i = 0; i < Count; i++)
            {
                var pz = Items[i];
                if (key == (int)(pz.IdKey & IdKey.EnumMask)) return i;
            }
            return 0;
        }
        #endregion

        #region GetEnumName  =================================================
        internal string GetEnumValueName(Root root, int key)
        {
            foreach (var pz in Items)
            {
                if (pz.EnumKey == key) return pz.GetNameId(root);
            }
            return InvalidItem;
        }
        internal string GetEnumIndexName(Root root, int index) =>  (index >= 0 && index < Count) ? Items[index].GetNameId(root) : InvalidItem;
        #endregion

        #region GetEnumNames  ================================================
        internal string[] GetEnumNames(Root root)
        {
            var s = new string[Count];

            for (int i = 0; i < Count; i++)
            {
                s[i] = Items[i].GetNameId(root);
            }
            return s;
        }
        #endregion
    }
}
