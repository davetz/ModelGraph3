
namespace ModelGraph.Core
{
    public class Enum_Connect : EnumZ
    {
        internal override IdKey IdKey => IdKey.ConnectEnum;

        #region Constructor  ==================================================
        internal Enum_Connect(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            new PairZ(this, IdKey.Connect_Any);
            new PairZ(this, IdKey.Connect_East);
            new PairZ(this, IdKey.Connect_West);
            new PairZ(this, IdKey.Connect_North);
            new PairZ(this, IdKey.Connect_South);
            new PairZ(this, IdKey.Connect_East_West);
            new PairZ(this, IdKey.Connect_North_South);
            new PairZ(this, IdKey.Connect_North_East);
            new PairZ(this, IdKey.Connect_North_West);
            new PairZ(this, IdKey.Connect_North_East_West);
            new PairZ(this, IdKey.Connect_North_South_East);
            new PairZ(this, IdKey.Connect_North_South_West);
            new PairZ(this, IdKey.Connect_South_East);
            new PairZ(this, IdKey.Connect_South_West);
            new PairZ(this, IdKey.Connect_South_East_West);
        }
        #endregion
    }
}
