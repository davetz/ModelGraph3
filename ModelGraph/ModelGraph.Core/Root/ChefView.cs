using System.Collections.Generic;

namespace ModelGraph.Core
{
    public partial class Root
    {
        private bool TryGetQueryItems(QueryX query, out List<Item> items, Item key = null)
        {
            items = null;
            if (query != null)
            {
                if (key == null)
                {
                    if (Get<Relation_Store_QueryX>().TryGetParent(query, out Store sto))
                    {
                        items = sto.GetItems();
                    }
                }
                else
                {
                    if (Get<Relation_Relation_QueryX>().TryGetParent(query, out Relation rel))
                    {
                        if (query.IsReversed)
                            rel.TryGetParents(key, out items);
                        else
                            rel.TryGetChildren(key, out items);
                    }
                }
                if (query.HasWhere) items = ApplyFilter(query, items);
            }
            return (items != null && items.Count > 0);
        }

        private (int L1, IList<Property> PropertyList, int L2, IList<QueryX> QueryList, int L3, IList<ViewX> ViewList) GetQueryXChildren(QueryX qx)
        {
            int L1 = Get<Relation_QueryX_Property>().TryGetChildren(qx, out IList<Property> ls1) ? ls1.Count : 0;
            int L2 = Get<Relation_QueryX_QueryX>().TryGetChildren(qx, out IList<QueryX> ls2) ? ls2.Count : 0;
            int L3 = Get<Relation_QueryX_ViewX>().TryGetChildren(qx, out IList<ViewX> ls3) ? ls3.Count : 0;

            return (L1, ls1, L2, ls2, L3, ls3);
        }

    }
}
