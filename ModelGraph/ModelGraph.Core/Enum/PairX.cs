namespace ModelGraph.Core
{
    public class PairX : Item
    {
        internal string DisplayValue;
        internal string ActualValue;
       

        #region Constructors  =================================================
        internal PairX(EnumX owner, bool autoExpand = false)
        {
            Owner = owner;
            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        internal EnumX EnumX => Owner as EnumX;
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.PairX;
        public override string GetNameId(Root root) => string.IsNullOrWhiteSpace(DisplayValue) ? BlankName : DisplayValue;
        #endregion
    }
}
