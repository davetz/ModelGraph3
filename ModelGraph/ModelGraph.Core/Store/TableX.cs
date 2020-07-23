namespace ModelGraph.Core
{
    public class TableX : ChildOfStoreOf<TableXRoot, RowX>
    {
        internal override string Name { get => _name; set => _name = value; }
        private string _name;
        internal override string Summary { get => _summary; set => _summary = value; }
        private string _summary;
        internal override string Description { get => _description; set => _description = value; }
        private string _description;

        #region Constructors  =================================================
        internal TableX(TableXRoot owner, bool autoExpand = false)
        {
            Owner = owner;
            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.TableX;
        public override string GetNameId(Root root) => string.IsNullOrWhiteSpace(Name) ? BlankName : Name;
        public override string GetSummaryId(Root root) => Summary;
        public override string GetDescriptionId(Root root) => Description;
        #endregion
    }
}
