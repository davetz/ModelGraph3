using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class QueryXRoot : ExternalRoot<QueryX>, ISerializer, IPrimeRoot
    {
        static Guid _serializerGuid = new Guid("33B9B8A4-9332-4902-A3C1-37C5F971B6FF");
        static byte _formatVersion = 1;
        internal override IdKey IdKey => IdKey.QueryXRoot;

        internal QueryXRoot(Root root)
        {
            Owner = root;
            root.RegisterItemSerializer((_serializerGuid, this));
        }

        #region IPrimeRoot  ===================================================
        public void CreateSecondaryHierarchy(Root root)
        {
            var sto = root.Get<PropertyRoot>();

            root.RegisterReferenceItem(new Property_QueryX_Where(sto));
            root.RegisterReferenceItem(new Property_QueryX_Select(sto));
            root.RegisterReferenceItem(new Property_QueryX_Facet1(sto));
            root.RegisterReferenceItem(new Property_QueryX_Facet2(sto));
            root.RegisterReferenceItem(new Property_QueryX_Connect1(sto));
            root.RegisterReferenceItem(new Property_QueryX_Connect2(sto));
            root.RegisterReferenceItem(new Property_QueryX_LineStyle(sto));
            root.RegisterReferenceItem(new Property_QueryX_DashStyle(sto));
            root.RegisterReferenceItem(new Property_QueryX_LineColor(sto));
            root.RegisterReferenceItem(new Property_QueryX_Relation(sto));
            root.RegisterReferenceItem(new Property_QueryX_IsReversed(sto));
            root.RegisterReferenceItem(new Property_QueryX_IsBreakPoint(sto));
            root.RegisterReferenceItem(new Property_QueryX_ExclusiveKey(sto));
            root.RegisterReferenceItem(new Property_QueryX_ValueType(sto));

            root.RegisterStaticProperties(typeof(QueryX), GetProps(root)); //used by property name lookup
        }

        public void RegisterRelationalReferences(Root root)
        {
            root.RegisterChildRelation(this, root.Get<Relation_QueryX_ViewX>());
            root.RegisterChildRelation(this, root.Get<Relation_QueryX_QueryX>());
            root.RegisterChildRelation(this, root.Get<Relation_QueryX_Property>());

            root.RegisterParentRelation(this, root.Get<Relation_ViewX_QueryX>());
            root.RegisterParentRelation(this, root.Get<Relation_QueryX_QueryX>());
            root.RegisterParentRelation(this, root.Get<Relation_Store_QueryX>());
            root.RegisterParentRelation(this, root.Get<Relation_GraphX_QueryX>());
            root.RegisterParentRelation(this, root.Get<Relation_SymbolX_QueryX>());
            root.RegisterParentRelation(this, root.Get<Relation_ComputeX_QueryX>());
            root.RegisterParentRelation(this, root.Get<Relation_Relation_QueryX>());
            root.RegisterParentRelation(this, root.Get<Relation_GraphX_SymbolQueryX>());
        }


        private Property[] GetProps(Root root) => new Property[]
        {
            root.Get<Property_QueryX_Where>(),
            root.Get<Property_QueryX_Select>(),
            root.Get<Property_QueryX_Facet1>(),
            root.Get<Property_QueryX_Facet2>(),
            root.Get<Property_QueryX_Connect1>(),
            root.Get<Property_QueryX_Connect2>(),
            root.Get<Property_QueryX_LineStyle>(),
            root.Get<Property_QueryX_DashStyle>(),
            root.Get<Property_QueryX_LineColor>(),
            root.Get<Property_QueryX_Relation>(),
            root.Get<Property_QueryX_IsReversed>(),
            root.Get<Property_QueryX_IsBreakPoint>(),
            root.Get<Property_QueryX_ExclusiveKey>(),
            root.Get<Property_QueryX_ValueType>(),
        };
        #endregion

        #region ISerializer  ==================================================
        public void ReadData(DataReader r, Item[] items)
        {
            var N = r.ReadInt32();
            if (N < 1) throw new Exception($"Invalid count {N}");
            SetCapacity(N);

            var fv = r.ReadByte();
            if (fv == 1)
            {
                for (int i = 0; i < N; i++)
                {
                    var index = r.ReadInt32();
                    if (index < 0 || index >= items.Length) throw new Exception($"Invalid index {index}");

                    var qx = new QueryX(this);
                    items[index] = qx;

                    var b = r.ReadUInt16();
                    if ((b & S1) != 0) qx.SetState(r.ReadUInt16());
                    if ((b & S2) != 0) qx.WhereString = Value.ReadString(r);
                    if ((b & S3) != 0) qx.SelectString = Value.ReadString(r);
                    if ((b & S4) != 0) qx.ExclusiveKey = r.ReadByte();

                    if (qx.QueryKind == QueryType.Path && qx.IsHead) qx.PathParm = new PathParm();

                    if ((b & S5) != 0) qx.PathParm.Facet1 = (Facet)r.ReadByte();
                    if ((b & S6) != 0) qx.PathParm.Target1 = (Target)r.ReadUInt16();

                    if ((b & S7) != 0) qx.PathParm.Facet2 = (Facet)r.ReadByte();
                    if ((b & S8) != 0) qx.PathParm.Target2 = (Target)r.ReadUInt16();

                    if ((b & S9) != 0) qx.PathParm.DashStyle = (DashStyle)r.ReadByte();
                    if ((b & S10) != 0) qx.PathParm.LineStyle = (LineStyle)r.ReadByte();
                    if ((b & S11) != 0) qx.PathParm.LineColor = Value.ReadString(r);
                }
            }
            else
                throw new Exception($"ColumnXDomain ReadData, unknown format version: {fv}");
        }

        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            if (Count > 0)
            {
                w.WriteInt32(Count);
                w.WriteByte(_formatVersion);

                foreach (var qx in Items)
                {
                    w.WriteInt32(itemIndex[qx]);

                    var b = SZ;
                    if (qx.HasState()) b |= S1;
                    if (!string.IsNullOrWhiteSpace(qx.WhereString)) b |= S2;
                    if (!string.IsNullOrWhiteSpace(qx.SelectString)) b |= S3;
                    if (qx.IsExclusive) b |= S4;
                    if (qx.QueryKind == QueryType.Path && qx.IsHead == true && qx.PathParm != null)
                    {
                        if (qx.PathParm.Facet1 != Facet.None) b |= S5;
                        if (qx.PathParm.Target1 != Target.Any) b |= S6;

                        if (qx.PathParm.Facet2 != Facet.None) b |= S7;
                        if (qx.PathParm.Target2 != Target.Any) b |= S8;

                        if (qx.PathParm.DashStyle != DashStyle.Solid) b |= S9;
                        if (qx.PathParm.LineStyle != LineStyle.PointToPoint) b |= S10;
                        if (!string.IsNullOrWhiteSpace(qx.PathParm.LineColor)) b |= S11;
                    }

                    w.WriteUInt16(b);
                    if ((b & S1) != 0) w.WriteUInt16(qx.GetState());
                    if ((b & S2) != 0) Value.WriteString(w, qx.WhereString);
                    if ((b & S3) != 0) Value.WriteString(w, qx.SelectString);
                    if ((b & S4) != 0) w.WriteByte(qx.ExclusiveKey);

                    if ((b & S5) != 0) w.WriteByte((byte)qx.PathParm.Facet1);
                    if ((b & S6) != 0) w.WriteUInt16((ushort)qx.PathParm.Target1);

                    if ((b & S7) != 0) w.WriteByte((byte)qx.PathParm.Facet2);
                    if ((b & S8) != 0) w.WriteUInt16((ushort)qx.PathParm.Target2);

                    if ((b & S9) != 0) w.WriteByte((byte)qx.PathParm.DashStyle);
                    if ((b & S10) != 0) w.WriteByte((byte)qx.PathParm.LineStyle);
                    if ((b & S11) != 0) Value.WriteString(w, qx.PathParm.LineColor);
                }
            }
        }
        #endregion
    }
}
