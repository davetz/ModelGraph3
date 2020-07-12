using System;

namespace ModelGraph.Core
{
    public class QueryX : Item
    {
        internal WhereSelect Where;
        internal WhereSelect Select;
        internal PathParm PathParm;
        internal byte ExclusiveKey;
        internal override State State { get; set; }
        private PFlag _flags;

        #region Constructor  ==================================================
        internal QueryX() { }
        internal QueryX(QueryXRoot owner, QueryType kind, bool isRoot = false, bool isHead = false)
        {
            Owner = owner;
            QueryKind = kind;
            IsRoot = isRoot;
            IsHead = isHead;
            IsTail = true;
            AutoExpandRight = true;

            if (QueryKind == QueryType.Path && IsHead) PathParm = new PathParm();

            owner.Add(this);
        }
        internal QueryX(QueryXRoot owner)
        {
            Owner = owner;

            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => GetIdKey();

        public override string GetNameId(Root root)
        {
            string name;
            if (IsRoot)
            {
                if (root.Get<Relation_Store_QueryX>().TryGetParent(this, out Store st))
                    name = st.GetDoubleNameId(root);
                else
                    name = InvalidItem;
            }
            else
            {
                if (root.Get<Relation_Relation_QueryX>().TryGetParent(this, out Relation re))
                {
                    var (head, tail) = re.GetHeadTail(root);
                    name = $"{head.GetNameId(root)} --> {tail.GetNameId(root)}";
                }
                else
                    name = InvalidItem;
            }

            if (HasSelect || HasWhere || IsRoot || IsHead || IsTail)
            {
                name = $"{name}      [";
                if (HasWhere) name = $"{name}{root.GetNameId(IdKey.QueryWhere)}( {WhereString} )";
                if (HasSelect) name = $"{name} {root.GetNameId(IdKey.QuerySelect)}( {SelectString} )";

                if (IsRoot || IsHead || IsTail)
                {
                    name = $"{name} ";
                    if (IsRoot) name = $"{name}R";
                    if (IsHead) name = $"{name}H";
                    if (IsTail) name = $"{name}T";
                }
                name = $"{name}]";
            }
            return name;
        }

        #region GetIdKey  =====================================================
        private IdKey GetIdKey()
        {
            var idKe = IdKey.QueryIsCorrupt;
            switch (QueryKind)
            {
                case QueryType.View:
                    if (IsHead)
                        idKe = IdKey.QueryViewHead;
                    else if (IsRoot)
                        idKe = IdKey.QueryViewRoot;
                    else
                        idKe = IdKey.QueryViewLink;
                    break;
                case QueryType.Path:
                    if (IsHead)
                        idKe = IdKey.QueryPathHead;
                    else
                        idKe = IdKey.QueryPathLink;
                    break;
                case QueryType.Group:
                    if (IsHead)
                        idKe = IdKey.QueryGroupHead;
                    else
                        idKe = IdKey.QueryGroupLink;
                    break;
                case QueryType.Egress:
                    if (IsHead)
                        idKe = IdKey.QuerySegueHead;
                    else
                        idKe = IdKey.QuerySegueLink;
                    break;
                case QueryType.Graph:
                    if (IsRoot)
                        idKe = IdKey.QueryGraphRoot;
                    else
                        idKe = IdKey.QueryGraphLink;
                    break;
                case QueryType.Value:
                    if (IsHead)
                        idKe = IdKey.QueryValueHead;
                    else if (IsRoot)
                        idKe = IdKey.QueryValueRoot;
                    else
                        idKe = IdKey.QueryValueLink;
                    break;
                case QueryType.Symbol:
                    idKe = IdKey.QueryNodeSymbol;
                    break;
            }
            return idKe;
        }
        #endregion
        #endregion

        #region Validation  ===================================================
        [Flags]
        private enum PFlag : byte //private validation state
        {
            Reset = 0,
            Completed = 0x1,
            InvalidRef = 0x2,
            CircularRef = 0x4,
            UnresolvedRef = 0x8,
            InvalidSyntax = 0x10,
            ErrorMask = InvalidRef | CircularRef | InvalidSyntax,
        } 
        private bool GetFlag(PFlag flag) => (_flags & flag) != 0;
        private void SetFlag(bool val, PFlag flag) { if (val) _flags |= flag; else _flags &= ~flag; }

        internal bool IsResolved => HasCompleted || HasError; 
        internal bool HasError => (_flags & PFlag.ErrorMask) != 0;

        internal bool HasCompleted { get { return GetFlag(PFlag.Completed); } set { SetFlag(value, PFlag.Completed); } }
        internal bool HasInvalidRef { get { return GetFlag(PFlag.InvalidRef); } set { SetFlag(value, PFlag.InvalidRef); } }
        internal bool HasCircularRef { get { return GetFlag(PFlag.CircularRef); } set { SetFlag(value, PFlag.CircularRef); } }
        internal bool HasUnresolvedRef { get { return GetFlag(PFlag.UnresolvedRef); } set { SetFlag(value, PFlag.UnresolvedRef); } }
        internal bool HasInvalidSyntax { get { return GetFlag(PFlag.InvalidSyntax); } set { SetFlag(value, PFlag.InvalidSyntax); } }
        internal void Validate(Store store, bool firstPass = false)
        {
            if (firstPass)
            {
                _flags = PFlag.Reset;
                if (Where != null && !Where.TryValidate(store)) HasInvalidSyntax = true;
                if (Select != null && !Select.TryValidate(store)) HasInvalidSyntax = true;
                return;
            }
            else
            {

            }
        }
        #endregion

        #region Property/Method  ==============================================
        internal bool IsExclusive => ExclusiveKey != 0;

        internal bool HasWhere => Where != null;
        internal bool HasSelect => Select != null;
        internal bool HasValidSelect => Select != null && Select.IsValid;

        internal bool AnyChange => (HasWhere && Where.AnyChange) || (HasSelect && Select.AnyChange);

        internal string WhereString { get { return Where?.InputString; } set { SetWhereString(value); } }
        internal string SelectString { get { return Select?.InputString; } set { SetSelectString(value); } }
        private void SetWhereString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                Where = null;
            else
                Where = new WhereSelect(value);
        }
        private void SetSelectString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                Select = null;
            else
                Select = new WhereSelect(value);
        }
        #endregion
    }
}
