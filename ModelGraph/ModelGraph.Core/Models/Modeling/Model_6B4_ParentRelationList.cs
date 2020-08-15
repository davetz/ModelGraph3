
using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;

namespace ModelGraph.Core
{
    public class Model_6B4_ParentRelationList : List2ModelOf<RowX, Relation>
    {
        private Relation_StoreX_ParentRelation StoreX_ParentRelation;
        internal Model_6B4_ParentRelationList(LineModel owner, RowX item) : base(owner, item) 
        {
            StoreX_ParentRelation = item.GetRoot().Get<Relation_StoreX_ParentRelation>();
        }
        internal override IdKey IdKey => IdKey.Model_6B4_ParentRelationList;
        public override string GetNameId() => Item.Owner.Owner.Owner.GetNameId(IdKey);
        public override string GetKindId() => string.Empty;
        public override bool CanFilterUsage => true;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => StoreX_ParentRelation.ChildCount(Item.GetOwner());
        protected override IList<Relation> GetChildItems() => StoreX_ParentRelation.TryGetChildren(Item.GetOwner(), out IList<Relation> list) ? list : new Relation[0];
        protected override void CreateChildModel(Relation childItem) => new Model_6A8_ParentRelation(this, Item, childItem);
        #endregion
    }
}
