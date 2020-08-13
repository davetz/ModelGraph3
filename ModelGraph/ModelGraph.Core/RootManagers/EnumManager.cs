namespace ModelGraph.Core
{
    public class EnumManager : ChildOfStoreOf<Root, EnumZ>
    {
        #region Constructors  =================================================
        internal EnumManager(Root root)
        {
            Owner = root;
            SetCapacity(20);
            root.RegisterPrivateItem(new Enum_Aspect(this));
            root.RegisterPrivateItem(new Enum_Attach(this));
            root.RegisterPrivateItem(new Enum_BarWidth(this));
            root.RegisterPrivateItem(new Enum_CompuType(this));
            root.RegisterPrivateItem(new Enum_Connect(this));
            root.RegisterPrivateItem(new Enum_Contact(this));
            root.RegisterPrivateItem(new Enum_DashStyle(this));
            root.RegisterPrivateItem(new Enum_Facet(this));
            root.RegisterPrivateItem(new Enum_Labeling(this));
            root.RegisterPrivateItem(new Enum_LineStyle(this));
            root.RegisterPrivateItem(new Enum_Pairing(this));
            root.RegisterPrivateItem(new Enum_Resizing(this));
            root.RegisterPrivateItem(new Enum_SideType(this));
            root.RegisterPrivateItem(new Enum_ValueType(this));
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.EnumManager;
        #endregion
    }
}
