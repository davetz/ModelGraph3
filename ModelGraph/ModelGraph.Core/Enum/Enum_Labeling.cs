
namespace ModelGraph.Core
{
    public class Enum_Labeling : EnumZ
    {
        internal override IdKey IdKey => IdKey.LabelingEnum;

        #region Constructor  ==================================================
        internal Enum_Labeling(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            new PairZ(this, IdKey.Labeling_None);
            new PairZ(this, IdKey.Labeling_Top);
            new PairZ(this, IdKey.Labeling_Left);
            new PairZ(this, IdKey.Labeling_Right);
            new PairZ(this, IdKey.Labeling_Bottom);
            new PairZ(this, IdKey.Labeling_Center);
            new PairZ(this, IdKey.Labeling_TopLeft);
            new PairZ(this, IdKey.Labeling_TopRight);
            new PairZ(this, IdKey.Labeling_BottomLeft);
            new PairZ(this, IdKey.Labeling_BottomRight);
            new PairZ(this, IdKey.Labeling_TopLeftSide);
            new PairZ(this, IdKey.Labeling_TopRightSide);
            new PairZ(this, IdKey.Labeling_TopLeftCorner);
            new PairZ(this, IdKey.Labeling_TopRightCorner);
            new PairZ(this, IdKey.Labeling_BottomLeftSide);
            new PairZ(this, IdKey.Labeling_BottomRightSide);
            new PairZ(this, IdKey.Labeling_BottomLeftCorner);
            new PairZ(this, IdKey.Labeling_BottomRightCorner);
        }
        #endregion
    }
}
