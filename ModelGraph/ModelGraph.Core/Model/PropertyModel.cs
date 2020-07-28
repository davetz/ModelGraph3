using System;

namespace ModelGraph.Core
{
    public abstract class PropertyModel : LineModel
    {
        internal Property Property;
        internal Item Item;
        internal override Item GetItem() => Item;

        internal override bool IsItemUsed => !(Property is ColumnX cx) || (cx.Value.IsSpecific(Item));

        internal PropertyModel(LineModel owner, Item item, Property prop)
        {
            Item = item;
            Owner = owner;
            Property = prop;

            Depth = (byte)(owner.Depth + 1);

            if (prop.IsReference)
                owner.AddPropertyModel(this);
            else
                Owner.Add(this);
        }
        internal override string GetFilterSortId(Root root) => Property.GetNameId(root);
        public override string GetSummaryId(Root root) => Property.GetSummaryId(root);

        public bool IsReadOnly => Property.IsReadonly;
        public bool IsMultiline => Property.IsMultiline;

        public virtual bool IsTextModel => false;
        public virtual bool IsCheckModel => false;
        public virtual bool IsComboModel => false;


        public virtual int GetIndexValue(Root root) => default;
        public virtual bool GetBoolValue(Root root) => default;
        public virtual string GetTextValue(Root root) => default;
        public virtual string[] GetlListValue(Root root) => default;

        public void PostSetIndexValue(Root root, int val) { if (val != GetIndexValue(root)) root.PostSetIndexValue(GetItem(), Property, val); }
        public void PostSetBoolValue(Root root, bool val) { if (val != GetBoolValue(root)) root.PostSetBoolValue(GetItem(), Property, val); }
        public void PostSetTextValue(Root root, string val) { if (val != GetTextValue(root)) root.PostSetTextValue(GetItem(), Property, val); }

        public override (string, string) GetKindNameId(Root root)
        {
            var name = Property.GetNameId(root);

            return (null, Property.HasParentName ? Property.GetParentName(root, GetItem()) : name);
        }

        public override string GetModelIdentity() => $"{IdKey}{Environment.NewLine}{Property.IdKey}  ({Property.ItemKey:X3}";
    }
}
