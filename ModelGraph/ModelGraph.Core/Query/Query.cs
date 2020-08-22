using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Query : ChildOf<QueryX>
    {
        internal Item Item;
        internal Item[] Items;

        internal Query Parent;
        internal Query[][] Children;
        internal override State State { get; set; }

        internal override IdKey IdKey => IdKey.Query;

        #region Constructor  ==================================================
        internal Query(QueryX qx, Query parent, Item item, Item[] items)
        {
            base.Owner = qx;

            Item = item;
            Items = items;
            Parent = parent;

            SetState(qx.GetState()); // copy the queryX state flags
        }
        #endregion

        #region Properties/Methods  ===========================================
        internal Query GetHeadQuery() { var q = this; while (!q.IsHead && q.Parent != null) { q = q.Parent; } return q; }

        internal Store GetStore() => Owner.Owner.GetStore(Owner);

        internal bool IsEmpty => (Items == null || Items.Length == 0);
        internal Item GetItem(int index) { return Items[index]; }

        /// <summary>
        /// Try to populate a list of stepValues (they will be in reverse order)
        /// </summary>
        internal bool TryGetValues(List<(Item, WhereSelect)> values)
        {
            values.Clear();
            var q = this;
            while (q != null) // don't run off the end
            {
                var select = q.Owner.Select;
                if (select != null && q.Items != null)
                {
                    foreach (var item in q.Items)
                    {
                        if (item == null) continue;
                        values.Add((item, select));
                    }
                }
                if (q.IsHead) return true;
                q = q.Parent;
            }
            return false; // we've failed
        }

        internal int ItemCount => (Items == null) ? 0 : Items.Length;

        internal bool TryGetItems(out Item[] items)
        {
            if (IsEmpty)
            {
                items = null;
                return false;
            }
            items = Items;
            return true;
        }
        internal bool TryGetQuerys(Item item, out Query[] querys)
        {
            querys = null;
            if (IsEmpty || Children == null) return false;

            for (int i = 0; i < Items.Length; i++)
            {
                if (item != Items[i]) continue;
                querys = Children[i];
                break;
            }

            return (querys != null);
        }
        internal int QueryCount(Item item)
        {
            if (!IsEmpty && Children != null)
            {
                for (int i = 0; i < Items.Length; i++)
                {
                    if (item != Items[i]) continue;
                    return (Children[i] != null) ? Children[i].Length : 0;
                }
            }
            return 0;
        }
        #endregion

        #region ModelHelpers  =================================================
        internal IdKey GetModelIdKey(bool isLink)
        {
            if (Owner.QueryKind == QueryType.Path)
            {
                if (isLink)
                {
                    if (IsHead)
                        return IdKey.Model_6C2_PathHead;
                    else
                        return IdKey.Model_6C3_PathLink;
                }
                else
                {
                    if (IsTail)
                        return IdKey.Model_6D3_PathTail;
                    else
                        return IdKey.Model_6D2_PathStep;
                }
            }
            if (Owner.QueryKind == QueryType.Group)
            {
                if (isLink)
                {
                    if (IsHead)
                        return IdKey.Model_6C4_GroupHead;
                    else
                        return IdKey.Model_6C5_GroupLink;
                }
                else
                {
                    if (IsTail)
                        return IdKey.Model_6D5_GroupTail;
                    else
                        return IdKey.Model_6D4_GroupStep;
                }
            }
            if (Owner.QueryKind == QueryType.Egress)
            {
                if (isLink)
                {
                    if (IsHead)
                        return IdKey.Model_6C6_EgressHead;
                    else
                        return IdKey.Model_6C7_EgressLink;
                }
                else
                {
                    if (IsTail)
                        return IdKey.Model_6D7_EgressTail;
                    else
                        return IdKey.Model_6D6_EgressStep;
                }
            }
            return IdKey.Model_6C1_QueryLink;
        }
        #endregion
    }
}
