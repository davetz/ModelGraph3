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
        public override string GetNameId(Root root) => root.Get<Relation_Store_NameProperty>().TryGetChild(Owner, out Property p) ? p.Value.GetString(this) : GetIndexId();
        public override string GetSummaryId(Root root) => root.Get<Relation_Store_SummaryProperty>().TryGetChild(Owner, out Property p) ? p.Value.GetString(this) : GetNameId(root);
        #endregion
    }
}
