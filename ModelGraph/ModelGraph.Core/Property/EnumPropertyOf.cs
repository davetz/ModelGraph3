namespace ModelGraph.Core
{
    public abstract class EnumPropertyOf<T> : PropertyOf<T, string>, IEnumProperty where T : Item
    {
        internal EnumZ EnumZ { get; }

        internal EnumPropertyOf(PropertyRoot owner, EnumZ enumZ)
        {
            Owner = owner;
            EnumZ = enumZ;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal abstract int GetItemPropertyValue(Item item); //get the item's proproperty enum key value
        internal abstract void SetItemPropertyValue(Item item, int key); //set the item's proproperty enum key value

        public string GetValue(int index) => EnumZ.GetEnumIndexName(DataRoot, index);
        internal override string GetValue(Item item) => EnumZ.GetEnumValueName(DataRoot, GetItemPropertyValue(item));
        internal override void SetValue(Item item, string val) => SetItemPropertyValue(item, EnumZ.GetKey(DataRoot, val));

        internal override int GetIndexValue(Item item) => EnumZ.GetEnumIndex(GetItemPropertyValue(item));
        internal override void SetIndexValue(Item item, int index) => SetItemPropertyValue(item, EnumZ.GetActualValueAt(index));

        internal override string[] GetlListValue(Root root) => EnumZ.GetEnumNames(root);
    }
}
