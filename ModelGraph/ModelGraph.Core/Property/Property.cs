
using System;

namespace ModelGraph.Core
{
    public abstract class Property : Item
    {
        internal Value Value = Root.ValuesUnknown;
        protected virtual Type PropetyModelType => typeof(Model_617_TextProperty);

        internal virtual bool HasParentName => false;
        internal virtual string GetParentName(Root root, Item itm) => default;

        internal virtual bool IsReadonly => false;
        internal virtual bool IsMultiline => false;

        internal virtual string[] GetlListValue(Root root) => new string[0];
        internal virtual int GetIndexValue(Item item) => 0;
        internal virtual void SetIndexValue(Item item, int val) { }

        internal void CreatePropertyModel(LineModel owner, Item item)
        {
            if (PropetyModelType == typeof(Model_619_ComboProperty))
                new Model_619_ComboProperty(owner, item, this);
            else if (PropetyModelType == typeof(Model_618_CheckProperty))
                new Model_618_CheckProperty(owner, item, this);
            else
                new Model_617_TextProperty(owner, item, this);
        }
    }
}
