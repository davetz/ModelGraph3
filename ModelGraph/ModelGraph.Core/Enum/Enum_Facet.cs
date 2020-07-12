
namespace ModelGraph.Core
{
    public class Enum_Facet : EnumZ
    {
        internal override IdKey IdKey => IdKey.FacetEnum;

        #region Constructor  ==================================================
        internal Enum_Facet(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            new PairZ(this, IdKey.Facet_None);
            new PairZ(this, IdKey.Facet_Nubby);
            new PairZ(this, IdKey.Facet_Diamond);
            new PairZ(this, IdKey.Facet_InArrow);
            new PairZ(this, IdKey.Facet_Force_None);
            new PairZ(this, IdKey.Facet_Force_Nubby);
            new PairZ(this, IdKey.Facet_Force_Diamond);
            new PairZ(this, IdKey.Facet_Force_InArrow);
        }
        #endregion
    }
}
