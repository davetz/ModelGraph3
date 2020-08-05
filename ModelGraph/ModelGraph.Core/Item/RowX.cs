namespace ModelGraph.Core
{
    public class RowX : ChildOf<TableX>
    {
        #region Constructors  =================================================
        internal RowX(TableX owner, bool autoExpand = false)
        {
            Owner = owner;
            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.RowX;
        public override string GetNameId() => Owner.Owner.GetRowXNameId(this);
        public override string GetSummaryId() => Owner.Owner.GetRowXSummaryId(this);
        #endregion
    }
}
