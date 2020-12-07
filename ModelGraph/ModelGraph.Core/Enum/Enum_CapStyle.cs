
namespace ModelGraph.Core
{
    public class Enum_CapStyle : EnumZ
    {
        internal override IdKey IdKey => IdKey.StrokeStyleEnum;

        #region Constructor  ==================================================
        internal Enum_CapStyle(EnumRoot owner) : base(owner)
        {
            CreateChildren();
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            new PairZ(this, IdKey.CapStyle_Flat);
            new PairZ(this, IdKey.CapStyle_Square);
            new PairZ(this, IdKey.CapStyle_Round);
            new PairZ(this, IdKey.CapStyle_Triangle);
        }
        #endregion
    }
}
