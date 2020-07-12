using System;

namespace ModelGraph.Core
{
    public class ColumnX : Property
    {
        internal override string Name { get => _name; set => _name = value; }
        private string _name;
        internal override string Summary { get => _summary; set => _summary = value; }
        private string _summary;
        internal override string Description { get => _description; set => _description = value; }
        private string _description;
        internal override State State { get; set; }

        #region Constructors  =================================================
        internal ColumnX(StoreOf<ColumnX> owner, bool autoExpand = false)
        {
            Owner = owner;
            if (autoExpand) AutoExpandRight = true;

            Value = Value.Create(ValType.String);

            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.ColumnX;
        public override string GetNameId(Root root) => string.IsNullOrWhiteSpace(Name) ? BlankName : Name;
        public override string GetParentId(Root root) => root.Get<Relation_Store_ColumnX>().TryGetParent(this, out Store p) ? p.GetNameId(root) : GetKindId(root);
        public override string GetSummaryId(Root root) => Summary;
        public override string GetDescriptionId(Root root) => Description;
        #endregion

        #region TrySetValueType  ==============================================
        internal void TrySetValueType(ValType type)
        {
            var root = DataRoot;

            if (Value.ValType == type) return;

            var newGroup = Value.GetValGroup(type);
            var preGroup = Value.GetValGroup(Value.ValType);

            if (!root.Get<Relation_Store_ColumnX>().TryGetParent(this, out Store tbl)) return;

            var N = tbl.Count;

            if (N == 0)
            {
                Value = Value.Create(type); //no existing values so nothing to convert
                return;
            }

            //=================================================================
            // convert the existing values that are of a different data type
            //=================================================================

            if ((newGroup & ValGroup.ScalarGroup) != 0 && (preGroup & ValGroup.ScalarGroup) != 0)
            {
                var rows = tbl.GetItems();
                var value = Value.Create(type, N);

                switch (newGroup)
                {
                    case ValGroup.Bool:
                        for (int i = 0; i < N; i++)
                        {
                            var key = rows[i];
                            Value.GetValue(key, out bool v);
                            if (!value.SetValue(key, v)) return;
                        }
                        break;
                    case ValGroup.Long:
                        for (int i = 0; i < N; i++)
                        {
                            var key = rows[i];
                            Value.GetValue(key, out Int64 v);
                            if (!value.SetValue(key, v)) return;
                        }
                        break;
                    case ValGroup.String:
                        for (int i = 0; i < N; i++)
                        {
                            var key = rows[i];
                            Value.GetValue(key, out string v);
                            if (!value.SetValue(key, v)) return;
                        }
                        break;
                    case ValGroup.Double:
                        for (int i = 0; i < N; i++)
                        {
                            var key = rows[i];
                            Value.GetValue(key, out double v);
                            if (!value.SetValue(key, v)) return;
                        }
                        break;
                    default:
                        return;
                }
                Value = value;
                return;
            }
            return;
        }
        #endregion
    }
}
