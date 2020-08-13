﻿
namespace ModelGraph.Core
{
    public class Enum_LineStyle : EnumZ
    {
        internal override IdKey IdKey => IdKey.LineStyleEnum;

        #region Constructor  ==================================================
        internal Enum_LineStyle(EnumManager owner) : base(owner)
        {
            CreateChildren();
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            new PairZ(this, IdKey.LineStyle_PointToPoint);
            new PairZ(this, IdKey.LineStyle_SimpleSpline);
            new PairZ(this, IdKey.LineStyle_DoubleSpline);
        }
        #endregion
    }
}
