
namespace ModelGraph.Core
{
    public class Enum_Attach : EnumZ
    {
        internal override IdKey IdKey => IdKey.AttatchEnum;

        #region Constructor  ==================================================
        internal Enum_Attach(EnumRoot owner) : base(owner)
        {
            CreateChildren();
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
