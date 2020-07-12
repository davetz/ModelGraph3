using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class NamePropertyModel_675 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal NamePropertyModel_675(NamePropertyRelationModel_673 owner, Property item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.NamePropertyModel_675;
        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);

        public override void GetMenuCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => RemoveNameProperty(root)));
        }
        private void RemoveNameProperty(Root root)
        {
            var pm = Owner as LineModel;
            var tx = pm.Item;
            pm.ChildDelta++;
            ItemUnLinked.Record(root, root.Get<Relation_Store_NameProperty>(), tx, Item);
        }
    }
}
