﻿
namespace ModelGraph.Core
{
    public class Enum_Aspect : EnumZ
    {
        internal override IdKey IdKey => IdKey.AspectEnum;

        #region Constructor  ==================================================
        internal Enum_Aspect(EnumRoot owner) : base(owner)
        {
            CreateChildren();
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            new PairZ(this, IdKey.Aspect_Point);
            new PairZ(this, IdKey.Aspect_Square);
            new PairZ(this, IdKey.Aspect_Vertical);
            new PairZ(this, IdKey.Aspect_Horizontal);
        }
        #endregion
    }
}
