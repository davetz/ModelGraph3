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
        internal EnumX Owner;
        internal override Item GetOwner() => Owner;
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.PairX;
        public override string GetNameId() => string.IsNullOrWhiteSpace(DisplayValue) ? BlankName : DisplayValue;
        #endregion
    }
}
