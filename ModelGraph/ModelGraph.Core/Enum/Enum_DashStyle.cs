﻿
namespace ModelGraph.Core
{
    public class Enum_DashStyle : EnumZ
    {
        internal override IdKey IdKey => IdKey.DashStyleEnum;

        #region Constructor  ==================================================
        internal Enum_DashStyle(EnumManager owner) : base(owner)
        {
            CreateChildren();
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            new PairZ(this, IdKey.DashStyle_Solid);
            new PairZ(this, IdKey.DashStyle_Dashed);
            new PairZ(this, IdKey.DashStyle_Dotted);
            new PairZ(this, IdKey.DashStyle_DashDot);
            new PairZ(this, IdKey.DashStyle_DashDotDot);
        }
        #endregion
    }
}
