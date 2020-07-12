using System.Text;

namespace ModelGraph.Core
{/*

 */
    public partial class Root
    {
        #region GetHeadTail  ==================================================
        internal void GetHeadTail(Relation rel, out Store head, out Store tail)
        {
            var relation_Store_ChildRelation = Get<Relation_Store_ChildRelation>();
            var relation_Store_ParentRelation = Get<Relation_Store_ParentRelation>();

            if (rel == null)
            {
                head = null;
                tail = null;
            }
            else if (rel is RelationX_RowX_RowX)
            {
                relation_Store_ChildRelation.TryGetParent(rel, out Store ch); head = ch;
                relation_Store_ParentRelation.TryGetParent(rel, out Store pa); tail = pa;
            }
            else
            {
                relation_Store_ChildRelation.TryGetParent(rel, out head);
                relation_Store_ParentRelation.TryGetParent(rel, out tail);
            }
        }
        #endregion

        #region <Get,Set>RelationName =========================================
        private const string parentNameSuffix = " --> ";
        private const string childNameSuffix = "       (";
        private const string identitySuffix = ")";
        internal string GetRelationName(Relation rel)
        {
            var relation_Store_ChildRelation = Get<Relation_Store_ChildRelation>();
            var relation_Store_ParentRelation = Get<Relation_Store_ParentRelation>();

            var name = (rel is RelationX_RowX_RowX rx) ? rx.Name : "Internal";
            var id = string.IsNullOrWhiteSpace(name) ? string.Empty : name;
            var identity = $"({id})  ";
            var childName = BlankName;
            var parentName = BlankName;
            if (relation_Store_ParentRelation.TryGetParent(rel, out Store childTable)) childName = childTable.Name;
            if (relation_Store_ChildRelation.TryGetParent(rel, out Store parentTable)) parentName = parentTable.Name;
            StringBuilder sb = new StringBuilder(132);
            sb.Append(identity);
            sb.Append(parentName);
            sb.Append(parentNameSuffix);
            sb.Append(childName);
            return sb.ToString();
        }
        internal void SetRelationName(RelationX_RowX_RowX rel, string value)
        {
            var relation_Store_ChildRelation = Get<Relation_Store_ChildRelation>();
            var relation_Store_ParentRelation = Get<Relation_Store_ParentRelation>();

            var childName = BlankName;
            var parentName = BlankName;
            if (relation_Store_ParentRelation.TryGetParent(rel, out Store childTable)) childName = childTable.Name;
            if (relation_Store_ChildRelation.TryGetParent(rel, out Store parentTable)) parentName = parentTable.Name;
            StringBuilder sb = new StringBuilder(value);
            sb.Replace(parentName + parentNameSuffix, "");
            sb.Replace(childName + childNameSuffix, "");
            sb.Replace(identitySuffix, "");
            rel.Name = sb.ToString();
        }
        string GetRelationName(QueryX sd)
        {
            return (Get<Relation_Relation_QueryX>().TryGetParent(sd, out Relation rel) ? GetRelationName(rel) : null);
        }
        #endregion
    }
}
