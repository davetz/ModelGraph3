

namespace ModelGraph.Core
{
    public class Enum_Contact : EnumZ
    {
        internal override IdKey IdKey => IdKey.ContactEnum;

        #region Constructor  ==================================================
        internal Enum_Contact(EnumManager owner) : base(owner)
        {
            CreateChildren();
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            new PairZ(this, IdKey.Contact_Any);
            new PairZ(this, IdKey.Contact_One);
            new PairZ(this, IdKey.Contact_None);
        }
        #endregion
    }
}
