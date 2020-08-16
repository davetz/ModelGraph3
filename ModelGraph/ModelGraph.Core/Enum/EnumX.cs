
namespace ModelGraph.Core
{
    public class EnumX : ChildOfStoreOf<EnumXManager, PairX>
    {
        internal override string Name { get => _name; set => _name = value; }
        private string _name;
        internal override string Summary { get => _summary; set => _summary = value; }
        private string _summary;
        internal override string Description { get => _description; set => _description = value; }
        private string _description;

        internal string GetActualValueAt(int index) => (index < 0 || index >= Count) ? InvalidItem : Items[index].ActualValue;

        #region Constructors  =================================================
        internal EnumX(EnumXManager owner, bool autoExpandRight = false)
        {
            Owner = owner;

            if (autoExpandRight) AutoExpandRight = true;
            owner.Add(this);
        }
        internal EnumXManager Owner;
        internal override Item GetOwner() => Owner;
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.EnumX;
        public override string GetNameId() => string.IsNullOrWhiteSpace(Name) ? BlankName : Name;
        public override string GetSummaryId() => Summary;
        public override string GetDescriptionId() => Description;
        #endregion

        internal string[] GetlListValue()
        {
            var list = new string[Count];
            for (int i = 0; i < Count; i++)
            {
                list[i] = Items[i].DisplayValue;
            }
            return list;
        }

        internal int GetIndexValue(ColumnX cx, Item item)
        {
            var val = cx.Value.GetString(item);
            for (int i = 0; i < Count; i++)
            {
                if (Items[i].ActualValue == val) return i;
            }
            return 0;
        }

        internal void SetIndexValue(ColumnX cx, Item item, int index)
        {
            if (index >= 0 || index < Count)
            {
                cx.Value.SetString(item, Items[index].ActualValue);
            }
        }
    }
}
