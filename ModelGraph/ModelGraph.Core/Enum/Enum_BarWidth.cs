
namespace ModelGraph.Core
{
    public class Enum_BarWidth : EnumZ
    {
        internal override IdKey IdKey => IdKey.BarWidthEnum;

        #region Constructor  ==================================================
        internal Enum_BarWidth(EnumRoot owner) : base(owner)
        {
            CreateChildren();
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            new PairZ(this, IdKey.BarWidth_Thin);
            new PairZ(this, IdKey.BarWidth_Wide);
            new PairZ(this, IdKey.BarWidth_ExtraWide);
        }
        #endregion
    }
}
