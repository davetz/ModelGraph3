
using System;

namespace ModelGraph.Core
{
    public abstract class Property : Item
    {
        internal Value Value = Root.ValuesUnknown;
        protected virtual Type PropetyModelType => typeof(PropertyTextModel_617);

        internal virtual bool HasParentName => false;
        internal virtual string GetParentName(Root root, Item itm) => default;

        internal virtual bool IsReadonly => false;
        internal virtual bool IsMultiline => false;

        internal virtual string[] GetlListValue(Root root) => new string[0];
        internal virtual int GetIndexValue(Item item) => 0;
        internal virtual void SetIndexValue(Item item, int val) { }

        internal void CreatePropertyModel(LineModel owner, Item item)
        {
            if (PropetyModelType == typeof(PropertyComboModel_619))
                new PropertyComboModel_619(owner, item, this);
            else if (PropetyModelType == typeof(PropertyCheckModel_618))
                new PropertyCheckModel_618(owner, item, this);
            else
                new PropertyTextModel_617(owner, item, this);
        }
    }
}
