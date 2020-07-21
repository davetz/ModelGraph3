﻿using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class ViewXRoot : ExternalRoot<ViewX>, ISerializer, IPrimeRoot
    {
        static Guid _serializerGuid = new Guid("396EC955-832E-4BEA-9E5C-C2A203ADAD23");
        static byte _formatVersion = 1;
        internal override IdKey IdKey => IdKey.ViewXRoot;

        internal ViewXRoot(Root root)
        {
            Owner = root;
            root.RegisterItemSerializer((_serializerGuid, this));
        }

        #region IPrimeRoot  ===================================================
        public void CreateSecondaryHierarchy(Root root)
        {
            root.RegisterStaticProperties(typeof(ViewX), GetProps(root)); //used by property name lookup
        }

        public void RegisterRelationalReferences(Root root)
        {
            root.RegisterChildRelation(this, root.Get<Relation_ViewX_Property>());
            root.RegisterChildRelation(this, root.Get<Relation_ViewX_QueryX>());
            root.RegisterChildRelation(this, root.Get<Relation_ViewX_ViewX>());

            root.RegisterParentRelation(this, root.Get<Relation_ViewX_ViewX>());
            root.RegisterParentRelation(this, root.Get<Relation_QueryX_ViewX>());
            root.RegisterParentRelation(this, root.Get<Relation_Property_ViewX>());
        }

        private Property[] GetProps(Root root) => new Property[]
        {
            root.Get<Property_Item_Name>(),
            root.Get<Property_Item_Summary>(),
            root.Get<Property_Item_Description>(),
        };
        #endregion

        #region ISerializer  ==================================================
        public void ReadData(DataReader r, Item[] items)
        {
            var N = r.ReadInt32();
            if (N < 1) throw new Exception($"Invalid count {N}");

            var fv = r.ReadByte();
            if (fv == 1)
            {
                for (int i = 0; i < N; i++)
                {
                    var index = r.ReadInt32();
                    if (index < 0 || index >= items.Length) throw new Exception($"Invalid index {index}");

                    var vx = new ViewX(this);
                    items[index] = vx;

                    var b = r.ReadByte();
                    if ((b & B1) != 0) vx.Name = Value.ReadString(r);
                    if ((b & B2) != 0) vx.Summary = Value.ReadString(r);
                    if ((b & B3) != 0) vx.Description = Value.ReadString(r);
                }
            }
            else
                throw new Exception($"ViewXDomain ReadData, unknown format version: {fv}");
        }

        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            if (Count > 0)
            {
                w.WriteInt32(Count);
                w.WriteByte(_formatVersion);

                foreach (var view in Items)
                {
                    w.WriteInt32(itemIndex[view]);

                    var b = BZ;
                    if (!string.IsNullOrWhiteSpace(view.Name)) b |= B1;
                    if (!string.IsNullOrWhiteSpace(view.Summary)) b |= B2;
                    if (!string.IsNullOrWhiteSpace(view.Description)) b |= B3;

                    w.WriteByte(b);
                    if ((b & B1) != 0) Value.WriteString(w, view.Name);
                    if ((b & B2) != 0) Value.WriteString(w, view.Summary);
                    if ((b & B3) != 0) Value.WriteString(w, view.Description);
                }
            }
        }
        #endregion

        #region ViewXMethods  =================================================

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


        #endregion
    }
}
