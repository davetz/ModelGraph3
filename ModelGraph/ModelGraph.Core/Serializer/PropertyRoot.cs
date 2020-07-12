
namespace ModelGraph.Core
{
    public class PropertyRoot : InternalRoot<Property>, IPrimeRoot
    {
        internal PropertyRoot(Root root)
        {
            Owner = root;
            SetCapacity(100);
        }

        #region IPrimeRoot  ===================================================
        public void CreateSecondaryHierarchy(Root root)
        {
            root.RegisterReferenceItem(new Property_Item_Name(this));
            root.RegisterReferenceItem(new Property_Item_Summary(this));
            root.RegisterReferenceItem(new Property_Item_Description(this));
        }

        public void RegisterRelationalReferences(Root root)
        {
            root.RegisterParentRelation(this, root.Get<Relation_ViewX_Property>());
            root.RegisterParentRelation(this, root.Get<Relation_Store_NameProperty>());
            root.RegisterParentRelation(this, root.Get<Relation_Store_SummaryProperty>());
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.PropertyRoot;
        public override string GetParentId(Root root) => GetKindId(root);
        #endregion

    }
}
