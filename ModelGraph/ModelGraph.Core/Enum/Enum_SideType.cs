
namespace ModelGraph.Core
{
    public class Enum_SideType : EnumZ
    {
        internal override IdKey IdKey => IdKey.SideEnum;

        #region Constructor  ==================================================
        internal Enum_SideType(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            new PairZ(this, IdKey.Side_Any);
            new PairZ(this, IdKey.Side_East);
            new PairZ(this, IdKey.Side_West);
            new PairZ(this, IdKey.Side_North);
            new PairZ(this, IdKey.Side_South);
        }
        #endregion
    }
}
