using System.Collections.Generic;
using System.Text;

namespace ModelGraph.Core
{
    public class ComputeX : Property
    {
        internal static string DefaultSeparator = " : ";

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
        internal ComputeX(ComputeXManager owner, bool autoExpand = false)
        {
            Owner = owner;

            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        internal ComputeXManager Owner;
        internal override Item GetOwner() => Owner;
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.ComputeX;
        public override string GetNameId() => string.IsNullOrWhiteSpace(Name) ? BlankName : Name;
        public override string GetSummaryId() => Summary;
        public override string GetDescriptionId() => Description;
        public override Item GetParent() => Owner.TryGetParent(this, out Store p) ? p : GetOwner();
        #endregion

        #region Properties  ===================================================
        internal void SetCompuType(CompuType ct) => Owner.SetComputeTypeProperty(this, ct);

        internal string GetWhereString() => Owner.GetWhereString(this);
        internal string GetSelectString() => Owner.GetSelectString(this);

        internal void SetWhereString(string val) => Owner.SetWhereString(this, val);
        internal void SetSelectString(string val) => Owner.SetSelectString(this, val);
        #endregion
    }
}