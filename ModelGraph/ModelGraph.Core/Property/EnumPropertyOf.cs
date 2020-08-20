using System;

namespace ModelGraph.Core
{
    public abstract class EnumPropertyOf<T> : PropertyOf<T, string>, IEnumProperty where T : Item
    {
        internal EnumZ EnumZ { get; }
        protected override Type PropetyModelType => typeof(Model_619_ComboProperty);

        internal EnumPropertyOf(PropertyManager owner, EnumZ enumZ) : base(owner)
        {
            EnumZ = enumZ;
            Value = new StringValue(this);
        }

        internal abstract int GetItemPropertyValue(Item item); //get the item's proproperty enum key value
        internal abstract void SetItemPropertyValue(Item item, int key); //set the item's proproperty enum key value

        public string GetValue(int index) => EnumZ.GetEnumIndexName(index);
        internal override string GetValue(Item item) => EnumZ.GetEnumValueName(GetItemPropertyValue(item));
        internal override void SetValue(Item item, string val) => SetItemPropertyValue(item, EnumZ.GetKey(val));

        internal override int GetIndexValue(Item item) => EnumZ.GetEnumIndex(GetItemPropertyValue(item));
        internal override void SetIndexValue(Item item, int index) => SetItemPropertyValue(item, EnumZ.GetActualValueAt(index));

        internal override string[] GetlListValue(Root root) => EnumZ.GetEnumNames(root);
    }
}
