
namespace ModelGraph.Core
{
    public class Enum_Resizing : EnumZ
    {
        internal override IdKey IdKey => IdKey.ResizingEnum;

        #region Constructor  ==================================================
        internal Enum_Resizing(EnumManager owner) : base(owner)
        {
            CreateChildren();
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            new PairZ(this, IdKey.Resizing_Auto);
            new PairZ(this, IdKey.Resizing_Fixed);
            new PairZ(this, IdKey.Resizing_Manual);
        }
        #endregion
    }
}
