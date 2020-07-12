
namespace ModelGraph.Core
{
    public class Enum_Attach : EnumZ
    {
        internal override IdKey IdKey => IdKey.AttatchEnum;

        #region Constructor  ==================================================
        internal Enum_Attach(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            new PairZ(this, IdKey.Attatch_Normal);
            new PairZ(this, IdKey.Attatch_Radial);
            new PairZ(this, IdKey.Attatch_RightAngle);
            new PairZ(this, IdKey.Attatch_SkewedAngle);
        }
        #endregion
    }
}
