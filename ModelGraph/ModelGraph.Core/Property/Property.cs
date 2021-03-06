﻿
using System;

namespace ModelGraph.Core
{
    public abstract class Property : Item
    {
        private Value _value = Value.ValuesUnknown;
        internal Value Value { get => _value; set { _value = value; _value.SetOwner(this); } }
        internal override State State { get; set; } //Properties uses the State bits

        protected virtual Type PropetyModelType => typeof(Model_617_TextProperty);

        internal virtual bool HasTargetName => false;
        internal virtual string GetTargetName(Item itm) => default;

        internal virtual void TriggerItemRefresh() { }

        internal virtual bool IsReadonly => false;
        internal virtual bool IsMultiline => false;

        internal virtual string[] GetListValue(Root root) => new string[0];
        internal virtual int GetIndexValue(Item item) => 0;
        internal virtual void SetIndexValue(Item item, int val) { }

        internal virtual void CreatePropertyModel(ItemModel owner, Item item)
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
