using System;

namespace ModelGraph.Core
{
    public abstract class PropertyModel : ItemModel
    {
        internal Property Property;
        internal Item Item;
        internal override Item GetItem() => Item;

        internal override bool IsItemUsed => !(Property is ColumnX cx) || (cx.Value.IsSpecific(Item));

        internal PropertyModel(ItemModel owner, Item item, Property prop)
        {
            Item = item;
            Owner = owner;
            Property = prop;

            Depth = (byte)(owner.Depth + 1);

            if (owner.CanExpandRight)
                owner.AddPropertyModel(this);
            else
                Owner.Add(this);
        }
        internal override string GetFilterSortId() => Property.GetNameId();
        public override string GetSummaryId() => Property.GetSummaryId();

        public bool IsReadOnly => Property.IsReadonly;
        public bool IsMultiline => Property.IsMultiline;

        public virtual bool IsTextModel => false;
        public virtual bool IsCheckModel => false;
        public virtual bool IsComboModel => false;

        public virtual int GetIndexValue(Root root) => default;
        public virtual bool GetBoolValue(Root root) => default;
        public virtual string GetTextValue(Root root) => default;
        public virtual string[] GetListValue(Root root) => default;

        internal void PostSetIndexValue(Root root, int val) { if (val != GetIndexValue(root)) root.PostSetIndexValue(GetItem(), Property, val); }
        internal void PostSetBoolValue(Root root, bool val) { if (val != GetBoolValue(root)) root.PostSetBoolValue(GetItem(), Property, val); }
        internal void PostSetTextValue(Root root, string val) { if (val != GetTextValue(root)) root.PostSetTextValue(GetItem(), Property, val); }

        public override string GetNameId()
        {
            var name = Property.GetNameId();

            return Property.HasTargetName ? Property.GetTargetName(GetItem()) : name;
        }

        public override string GetModelIdentity() => $"{IdKey}{Environment.NewLine}{Property.IdKey}  ({Property.ItemKey:X3}";
    }
}
