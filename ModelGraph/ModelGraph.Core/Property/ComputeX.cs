namespace ModelGraph.Core
{
    public class ComputeX : Property
    {
        internal const string DefaultSeparator = " : ";
        internal override string Name { get => _name; set => _name = value; }
        private string _name;
        internal override string Summary { get => _summary; set => _summary = value; }
        private string _summary;
        internal override string Description { get => _description; set => _description = value; }
        private string _description;

        internal string Separator = DefaultSeparator;

        internal override State State { get; set; }

        internal CompuType CompuType; // type of computation

        #region Constructors  =================================================
        internal ComputeX(StoreOf<ComputeX> owner, bool autoExpand = false)
        {
            Owner = owner;

            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.ComputeX;
        public override string GetNameId(Root root) => string.IsNullOrWhiteSpace(Name) ? BlankName : Name;
        public override string GetParentId(Root root) => root.Get<Relation_Store_ComputeX>().TryGetParent(this, out Store p) ? p.GetNameId(root) : GetKindId(root);
        public override string GetSummaryId(Root root) => Summary;
        public override string GetDescriptionId(Root root) => Description;
        #endregion
    }
}