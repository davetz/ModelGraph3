﻿
namespace ModelGraph.Core
{
    public class Enum_Pairing : EnumZ
    {
        internal override IdKey IdKey => IdKey.PairingEnum;

        #region Constructor  ==================================================
        internal Enum_Pairing(EnumRoot owner) : base(owner)
        {
            CreateChildren();
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            new PairZ(this, IdKey.Pairing_OneToOne);
            new PairZ(this, IdKey.Pairing_OneToMany);
            new PairZ(this, IdKey.Pairing_ManyToMany);
        }
        #endregion
    }
}
