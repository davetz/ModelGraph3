
namespace ModelGraph.Core
{
    public class Enum_StrokeStyle : EnumZ
    {
        internal override IdKey IdKey => IdKey.StrokeStyleEnum;

        #region Constructor  ==================================================
        internal Enum_StrokeStyle(EnumManager owner) : base(owner)
        {
            CreateChildren();
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            new PairZ(this, IdKey.StrokeStyle_Solid);
            new PairZ(this, IdKey.StrokeStyle_Dashed);
            new PairZ(this, IdKey.StrokeStyle_Dotted);
            new PairZ(this, IdKey.StrokeStyle_Filled);
        }
        #endregion
    }
}
