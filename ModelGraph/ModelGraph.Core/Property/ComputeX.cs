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
        private ComputeXRoot CR => Owner as ComputeXRoot;
        internal ComputeX(ComputeXRoot owner, bool autoExpand = false)
        {
            Owner = owner;

            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        internal ComputeXRoot Owner;
        internal override Item GetOwner() => Owner;
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.ComputeX;
        public override string GetNameId(Root root) => string.IsNullOrWhiteSpace(Name) ? BlankName : Name;
        public override string GetSummaryId(Root root) => Summary;
        public override string GetDescriptionId(Root root) => Description;
        #endregion

        #region Properties  ===================================================
        public override string GetParentId(Root root) => CR.GetParentId(root, this);

        internal void SetCompuType(CompuType ct) => CR.SetComputeTypeProperty(this, ct);

        internal string GetWhereString() => CR.GetWhereString(this);
        internal string GetSelectString() => CR.GetSelectString(this);

        internal void SetWhereString(string val) => CR.SetWhereString(this, val);
        internal void SetSelectString(string val) => CR.SetSelectString(this, val);
        #endregion
    }
}