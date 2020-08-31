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

        #region Constructors  =================================================
        internal ColumnX(ColumnXManager owner, bool autoExpand = false)
        {
            Owner = owner;
            if (autoExpand) AutoExpandRight = true;

            Value = Value.Create(ValType.String);

            owner.Add(this);
        }
        internal ColumnXManager Owner;
        internal override Item GetOwner() => Owner;
        internal override void TriggerItemRefresh() => Owner.Owner.AddRefreshTriggerItem(this);
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.ColumnX;
        public override string GetNameId() => string.IsNullOrWhiteSpace(Name) ? BlankName : Name;
        public override string GetSummaryId() => Summary;
        public override string GetDescriptionId() => Description;
        public override Item GetParent() => Owner.TryGetParent(this, out Store p) ? p : GetOwner();
        #endregion

        #region TrySetValueType  ==============================================
        internal void TrySetValueType(ValType type)
        {
            if (Value.ValType != type && Owner.TryGetParent(this, out Store tbl))
            {
                var N = tbl.Count;
                if (N == 0)
                {
                    Value = Value.Create(type); //no existing values so nothing to convert
                }
                else
                {
                    var newGroup = Value.GetValGroup(type);
                    var preGroup = Value.GetValGroup(Value.ValType);

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
                                    Value.GetValue(key, out long v);
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
            }
        }
        #endregion

        #region PropertyModel  ================================================
        internal override string[] GetlListValue(Root root) => Owner.GetEnumXlListValue(this);
        internal override int GetIndexValue(Item item) => Owner.GetEnumXlIndexValue(this, item);
        internal override void SetIndexValue(Item item, int index) => Owner.SetEnumXIndexValue(this, item, index);

        internal override void CreatePropertyModel(ItemModel owner, Item item)
        {
            if (Value.ValType == ValType.Bool)
                new Model_618_CheckProperty(owner, item, this);
            else if (HasEnumXRef)
            {
                var vg = Value.GetValGroup(Value.ValType);
                if (vg == ValGroup.Int || vg == ValGroup.Long || vg == ValGroup.String)
                    new Model_619_ComboProperty(owner, item, this);
                else
                    new Model_617_TextProperty(owner, item, this);
            }
            else
                new Model_617_TextProperty(owner, item, this);
        }
        #endregion
    }
}
