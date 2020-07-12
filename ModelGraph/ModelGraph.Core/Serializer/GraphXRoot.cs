using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class GraphXRoot : ExternalRoot<GraphX>, ISerializer, IPrimeRoot
    {
        static Guid _serializerGuid = new Guid("48C7FA8C-88F1-4203-8E54-3255C1F8C528");
        static byte _formatVersion = 1;
        internal override IdKey IdKey => IdKey.GraphXRoot;

        internal GraphXRoot(Root root) 
        {
            Owner = root;
            root.RegisterItemSerializer((_serializerGuid, this));
            new NodeSerializer(root);
        }

        #region IPrimeRoot  ===================================================
        public void CreateSecondaryHierarchy(Root root)
        {
            var sto = root.Get<PropertyRoot>();

            root.RegisterReferenceItem(new Property_GraphX_TerminalLength(sto));
            root.RegisterReferenceItem(new Property_GraphX_TerminalSpacing(sto));
            root.RegisterReferenceItem(new Property_GraphX_TerminalStretch(sto));
            root.RegisterReferenceItem(new Property_GraphX_SymbolSize(sto));

            root.RegisterStaticProperties(typeof(GraphX), GetProps(root)); //used by property name lookup
        }
        public void RegisterRelationalReferences(Root root)
        {
            root.RegisterChildRelation(this, root.Get<Relation_GraphX_ColorColumnX>());
            root.RegisterChildRelation(this, root.Get<Relation_GraphX_QueryX>());
            root.RegisterChildRelation(this, root.Get<Relation_GraphX_SymbolQueryX>());
            root.RegisterChildRelation(this, root.Get<Relation_GraphX_SymbolX>());
        }

        private Property[] GetProps(Root root) => new Property[]
        {
            root.Get<Property_Item_Name>(),
            root.Get<Property_Item_Summary>(),
            root.Get<Property_Item_Description>(),

            root.Get<Property_GraphX_TerminalLength>(),
            root.Get<Property_GraphX_TerminalSpacing>(),
            root.Get<Property_GraphX_TerminalStretch>(),
            root.Get<Property_GraphX_SymbolSize>(),
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

                    var gx = new GraphX(this);
                    items[index] = gx;

                    var b = r.ReadByte();
                    if ((b & B1) != 0) gx.SetState(r.ReadUInt16());
                    if ((b & B2) != 0) gx.Name = Value.ReadString(r);
                    if ((b & B3) != 0) gx.Summary = Value.ReadString(r);
                    if ((b & B4) != 0) gx.Description = Value.ReadString(r);

                    gx.MinNodeSize = r.ReadByte();
                    gx.ThinBusSize = r.ReadByte();
                    gx.WideBusSize = r.ReadByte();
                    gx.ExtraBusSize = r.ReadByte();

                    gx.SurfaceSkew = r.ReadByte();
                    gx.TerminalSkew = r.ReadByte();
                    gx.TerminalLength = r.ReadByte();
                    gx.TerminalSpacing = r.ReadByte();
                    gx.SymbolSize = r.ReadByte();
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

                foreach (var gx in Items)
                {
                    w.WriteInt32(itemIndex[gx]);

                    var b = BZ;
                    if (gx.HasState()) b |= B1;
                    if (!string.IsNullOrWhiteSpace(gx.Name)) b |= B2;
                    if (!string.IsNullOrWhiteSpace(gx.Summary)) b |= B3;
                    if (!string.IsNullOrWhiteSpace(gx.Description)) b |= B4;

                    w.WriteByte(b);
                    if ((b & B1) != 0) w.WriteUInt16(gx.GetState());
                    if ((b & B2) != 0) Value.WriteString(w, gx.Name);
                    if ((b & B3) != 0) Value.WriteString(w, gx.Summary);
                    if ((b & B4) != 0) Value.WriteString(w, gx.Description);

                    w.WriteByte(gx.MinNodeSize);
                    w.WriteByte(gx.ThinBusSize);
                    w.WriteByte(gx.WideBusSize);
                    w.WriteByte(gx.ExtraBusSize);

                    w.WriteByte(gx.SurfaceSkew);
                    w.WriteByte(gx.TerminalSkew);
                    w.WriteByte(gx.TerminalLength);
                    w.WriteByte(gx.TerminalSpacing);
                    w.WriteByte(gx.SymbolSize);
                }
            }
        }
        #endregion
    }
}
