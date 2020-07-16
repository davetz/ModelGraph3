
using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;

namespace ModelGraph.Core
{
    public class Model_6B4_ParentRelationList : ListModelOf<Relation>
    {
        private Relation_StoreX_ParentRelation StoreX_ParentRelation;
        private RowX RX => Item as RowX;
        internal Model_6B4_ParentRelationList(LineModel owner, RowX item) : base(owner, item) 
        {
            StoreX_ParentRelation = item.DataRoot.Get<Relation_StoreX_ParentRelation>();
        }
        internal override IdKey IdKey => IdKey.Model_6B4_ParentRelationList;

        #region RequiredMethods  ==============================================
        protected override int GetTotalCount() => StoreX_ParentRelation.ChildCount(Item.Owner);
        protected override IList<Relation> GetChildItems() => StoreX_ParentRelation.TryGetChildren(Item.Owner, out IList<Relation> list) ? list : new Relation[0];
        protected override void CreateChildModel(Relation childItem)
        {
            new Model_6A8_ParentRelation(this, RX, childItem);
        }
        #endregion
    }
}
