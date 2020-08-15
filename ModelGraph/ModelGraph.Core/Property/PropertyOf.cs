namespace ModelGraph.Core
{
    public abstract class PropertyOf<T1, T2> : Property, IValueStore<T2> where T1 : Item
    {
        internal virtual T2 GetValue(Item item) => default;
        internal virtual void SetValue(Item item, T2 val) { }

        #region Constructor  ==================================================
        internal PropertyOf(PropertyManager owner)
        {
            Owner = owner;
            owner.Add(this);
        }
        internal PropertyManager Owner;
        internal override Item GetOwner() => Owner;

        internal override void TriggerItemRefresh() => Owner.Owner.AddRefreshTriggerItem(this);
        #endregion

        #region Cast  =========================================================
        internal T1 Cast(Item item) { return item as T1; }
        #endregion

        #region IValueStore  ==================================================
        public int Count => 0;
        public void Clear() { }
        public bool IsSpecific(Item key) => true;
        public void Remove(Item key) { }
        public void SetOwner(Property p) { }

        public bool GetVal(Item key, out T2 val) { val = GetValue(key); return true; }
        public bool SetVal(Item key, T2 value) { if (IsReadonly) return false; SetValue(key, value); return true; }
        #endregion
    }
}

