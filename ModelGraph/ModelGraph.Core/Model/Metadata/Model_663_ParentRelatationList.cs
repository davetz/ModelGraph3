
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_663_ParentRelatationList : ListModelOf<Relation>
    {//============================================== In the MetaDataRoot hierarchy  ==============
        private Relation_StoreX_ParentRelation StoreX_ParentRelation;
        private TableX TX => Item as TableX;
        internal Model_663_ParentRelatationList(Model_654_Table owner, TableX item) : base(owner, item) 
        {
            StoreX_ParentRelation = item.DataRoot.Get<Relation_StoreX_ParentRelation>();
        }
        internal override IdKey IdKey => IdKey.Model_663_ParentRelatationList;
        internal override string GetFilterSortId(Root root) => GetNameId(root);

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => StoreX_ParentRelation.ChildCount(Item);
        protected override IList<Relation> GetChildItems() => StoreX_ParentRelation.TryGetChildren(Item, out IList<Relation> list) ? list : new Relation[0];
        protected override void CreateChildModel(Relation childItem)
        {
            new Model_672_ParentRelation(this, childItem);
        }
        #endregion

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => AddNewParentRelation(root)));
        }
        private void AddNewParentRelation(Root root)
        {
            var rx = new RelationX_RowX_RowX(root.Get<RelationXRoot>(), true);

            // the data root implements undo/redo functionality
            ItemCreated.Record(root, rx);
            ItemLinked.Record(root, StoreX_ParentRelation, TX, rx);
            ChildDelta -= 2;
        }
    }
}
