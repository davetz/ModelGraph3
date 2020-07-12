namespace ModelGraph.Core
{
    public class ItemUpdated : ItemChange
    {
        internal Item Item;
        internal Property Property;
        internal string OldValue;
        internal string NewValue;
        internal override IdKey IdKey =>  IdKey.ItemUpdated;

        #region Constructor  ==================================================
        private ItemUpdated(ChangeSet owner, Item item, Property property, string oldValue, string newValue, string name)
        {
            Owner = owner;
            _name = name;

            Item = item;
            Property = property;
            OldValue = oldValue;
            NewValue = newValue;

            owner.Add(this);
            UpdateDelta();
        }
        #endregion

        #region Record  =====================================================
        internal static bool IsNotRequired(Item itm, Property prop, string b)
        {
            var a = prop.Value.GetString(itm);

            return N(a) ? N(b) : (N(b) ? false : E(a, b));

            bool N(string v) => string.IsNullOrWhiteSpace(v); //is NULL or blank
            bool E(string p, string q) => (string.Compare(p, q) == 0); //are EQUAL
        }

        internal static void Record(Root root, Item itm, Property prop, string newValue)
        {
            itm.ModelDelta++;
            if (prop.IsCovert)
            {
                prop.Value.SetString(itm, newValue);
            }
            else
            {
                var oldValue = prop.Value.GetString(itm);
                var name = $"{itm.GetChangeLogId(root)}    {prop.GetNameId(root)}:  old<{oldValue}>  new<{newValue}>";
                if (prop.Value.SetString(itm, newValue))
                {
                    new ItemUpdated(root.Get<ChangeRoot>().ChangeSet, itm, prop, oldValue, newValue, name);
                }
            }
        }
        #endregion

        #region Undo/Redo  ====================================================
        internal override void Undo()
        {
            if (IsValid(Item) && CanUndo && Property.Value.SetString(Item, OldValue))
            {
                Item.ModelDelta++;
                IsUndone = true;
            }
        }

        internal override void Redo()
        {
            if (IsValid(Item) && CanRedo && Property.Value.SetString(Item, NewValue))
            {
                Item.ModelDelta++;
                IsUndone = false;
            }
        }
        #endregion
    }
}
