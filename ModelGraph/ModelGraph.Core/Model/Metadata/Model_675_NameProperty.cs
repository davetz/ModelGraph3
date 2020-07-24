using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_675_NameProperty : ItemModelOf<Property>
    {
        internal Model_675_NameProperty(Model_673_NamePropertyRelation owner, Property item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.Model_675_NameProperty;
        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);

        public override void GetMenuCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => RemoveNameProperty(root)));
        }
        private void RemoveNameProperty(Root root)
        {
            var pm = Owner as LineModel;
            var tx = pm.GetItem();
            pm.ChildDelta++;
            ItemUnLinked.Record(root, root.Get<Relation_Store_NameProperty>(), tx, Item);
        }
    }
}
