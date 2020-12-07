﻿
namespace ModelGraph.Core
{
    public class Enum_CompuType : EnumZ
    {
        internal override IdKey IdKey => IdKey.CompuTypeEnum;

        #region Constructor  ==================================================
        internal Enum_CompuType(EnumRoot owner) : base(owner)
        {
            CreateChildren();
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            new PairZ(this, IdKey.CompuType_RowValue);
            new PairZ(this, IdKey.CompuType_RelatedValue);
            new PairZ(this, IdKey.CompuType_CompositeString);
            new PairZ(this, IdKey.CompuType_CompositeReversed);
        }
        #endregion
    }
}
