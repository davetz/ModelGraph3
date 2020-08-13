namespace ModelGraph.Core
{
    public class ViewX : Item
    {
        internal override string Name { get => _name; set => _name = value; }
        private string _name;
        internal override string Summary { get => _summary; set => _summary = value; }
        private string _summary;
        internal override string Description { get => _description; set => _description = value; }
        private string _description;

        #region Constructor  ======================================================
        internal ViewX(ViewXManager owner, bool autoExpandRight = false)
        {
            Owner = owner;

            if (autoExpandRight) AutoExpandRight = true;
            owner.Add(this);
        }
        internal ViewXManager Owner;
        internal override Item GetOwner() => Owner;
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.ViewX;
        public override string GetNameId() => string.IsNullOrWhiteSpace(Name) ? BlankName : Name;
        public override string GetSummaryId() => Summary;
        public override string GetDescriptionId() => Description;
        #endregion
    }
}
