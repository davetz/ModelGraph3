
namespace ModelGraph.Core
{
    public abstract class EnumZ : ChildOfStoreOf<EnumRoot, PairZ>
    {
        internal EnumZ (EnumRoot owner)
        {
            Owner = owner;
            owner.Add(this);
        }

        #region GetEnumKey  ==================================================
        internal int GetKey(string name)
        {
            foreach (var pz in Items)
            {
                if (name == pz.GetNameId()) return pz.EnumKey;
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
                if (name == pz.GetNameId()) return i;
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
        internal string GetEnumValueName(int key)
        {
            foreach (var pz in Items)
            {
                if (pz.EnumKey == key) return pz.GetNameId();
            }
            return InvalidItem;
        }
        internal string GetEnumIndexName(int index) =>  (index >= 0 && index < Count) ? Items[index].GetNameId() : InvalidItem;
        #endregion

        #region GetEnumNames  ================================================
        internal string[] GetEnumNames(Root root)
        {
            var s = new string[Count];

            for (int i = 0; i < Count; i++)
            {
                s[i] = Items[i].GetNameId();
            }
            return s;
        }
        #endregion
    }
}
