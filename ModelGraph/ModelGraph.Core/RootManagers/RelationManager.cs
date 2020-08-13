
namespace ModelGraph.Core
{
    public class RelationManager : InternalManager<Root, Relation>, IPrimeRoot, IRelationRoot
    {
        internal RelationManager(Root root)
        {
            Owner = root;
            SetCapacity(30);
            new RelationLink(root, this);
        }

        #region IPrimeRoot  ===================================================
        public void CreateSecondaryHierarchy(Root root)
        {
            root.RegisterReferenceItem(new Relation_ComputeX_QueryX(this));
            root.RegisterReferenceItem(new Relation_EnumX_ColumnX(this));
            root.RegisterReferenceItem(new Relation_GraphX_ColorColumnX(this));
            root.RegisterReferenceItem(new Relation_GraphX_QueryX(this));
            root.RegisterReferenceItem(new Relation_GraphX_SymbolQueryX(this));
            root.RegisterReferenceItem(new Relation_GraphX_SymbolX(this));
            root.RegisterReferenceItem(new Relation_Property_ViewX(this));
            root.RegisterReferenceItem(new Relation_QueryX_Property(this));
            root.RegisterReferenceItem(new Relation_QueryX_QueryX(this));
            root.RegisterReferenceItem(new Relation_QueryX_ViewX(this));
            root.RegisterReferenceItem(new Relation_Relation_QueryX(this));
            root.RegisterReferenceItem(new Relation_Relation_ViewX(this));
            root.RegisterReferenceItem(new Relation_Store_ColumnX(this));
            root.RegisterReferenceItem(new Relation_Store_ComputeX(this));
            root.RegisterReferenceItem(new Relation_Store_NameProperty(this));
            root.RegisterReferenceItem(new Relation_Store_QueryX(this));
            root.RegisterReferenceItem(new Relation_Store_SummaryProperty(this));
            root.RegisterReferenceItem(new Relation_SymbolX_QueryX(this));
            root.RegisterReferenceItem(new Relation_ViewX_Property(this));
            root.RegisterReferenceItem(new Relation_ViewX_QueryX(this));
            root.RegisterReferenceItem(new Relation_ViewX_ViewX(this));

            root.RegisterReferenceItem(new Relation_StoreX_ChildRelation(this));
            root.RegisterReferenceItem(new Relation_StoreX_ParentRelation(this));

            // 2 extra items shown in internal Relations but not serialized
            //=================================================================
            Add(root.Get<Relation_Store_ChildRelation>());
            Add(root.Get<Relation_Store_ParentRelation>());
        }
        public void RegisterRelationalReferences(Root root)
        {
            InitializeLocalReferences(root);
        }
        public void ValidateDomain(Root root) { }
        #endregion

        #region IRelationRoot  ================================================
        public Relation[] GetRelationArray()
        {
            var N = Count - 2;
            var relationArray = new Relation[N];
            for (int i = 0; i < N; i++)
            {
                relationArray[i] = Items[i];
            }
            return relationArray;
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.RelationManager;
        #endregion

        #region RelationMethods  ==============================================
        //========================================== frequently used references
        private Relation_Store_ChildRelation _relation_Store_ChildRelation;
        private Relation_Store_ParentRelation _relation_Store_ParentRelation;

        #region InitializeLocalReferences  ====================================
        private void InitializeLocalReferences(Root root)
        {
            _relation_Store_ChildRelation = root.Get<Relation_Store_ChildRelation>();
            _relation_Store_ParentRelation = root.Get<Relation_Store_ParentRelation>();
        }
        #endregion

        #region GetHeadTail  ==================================================
        public (Store, Store) GetHeadTail(Relation rx)
        {
            Store head, tail;
                _relation_Store_ChildRelation.TryGetParent(rx, out head);
                _relation_Store_ParentRelation.TryGetParent(rx, out tail);

            return (head, tail);
        }

        #endregion

        #endregion
    }
}
