using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class SymbolXRoot : ExternalRoot<SymbolX>, ISerializer, IPrimeRoot
    {
        static Guid _serializerGuid = new Guid("D3956312-BEC7-4988-8228-DCA95CF23781");
        static byte _formatVersion = 1;
        internal override IdKey IdKey => IdKey.SymbolXRoot;

        internal SymbolXRoot(Root root)
        {
            Owner = root;
            root.RegisterItemSerializer((_serializerGuid, this));
        }

        #region IPrimeRoot  ===================================================
        public void CreateSecondaryHierarchy(Root root)
        {
            var sto = root.Get<PropertyRoot>();

            root.RegisterReferenceItem(new Property_SymbolX_Attatch(sto));

            root.RegisterStaticProperties(typeof(SymbolX), GetProps(root)); //used by property name lookup
        }
        public void RegisterRelationalReferences(Root root)
        {
            root.RegisterChildRelation(this, root.Get<Relation_SymbolX_QueryX>());

            root.RegisterParentRelation(this, root.Get<Relation_GraphX_SymbolX>());
        }

        private Property[] GetProps(Root root) => new Property[]
        {
            root.Get<Property_Item_Name>(),
            root.Get<Property_Item_Summary>(),
            root.Get<Property_Item_Description>(),

            root.Get<Property_SymbolX_Attatch>(),
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

                    var sx = new SymbolX(this);
                    items[index] = sx;

                    var b = r.ReadUInt16();
                    if ((b & S1) != 0) sx.SetState(r.ReadUInt16());
                    if ((b & S2) != 0) sx.Name = Value.ReadString(r);
                    if ((b & S3) != 0) sx.Summary = Value.ReadString(r);
                    if ((b & S4) != 0) sx.Description = Value.ReadString(r);
                    if ((b & S5) != 0) sx.Data = Value.ReadBytes(r);
                    if ((b & S6) != 0) sx.Attach = (Attach)r.ReadByte();
                    if ((b & S7) != 0) sx.AutoFlip = (AutoFlip)r.ReadByte();
                    var cnt = r.ReadByte();
                    sx.TargetContacts.Clear();
                    for (int j = 0; j < cnt; j++)
                    {
                        var tg = (Target)r.ReadUInt16();
                        var ti = (TargetIndex)r.ReadByte();
                        var cn = (Contact)r.ReadByte();
                        var dx = (sbyte)r.ReadByte();
                        var dy = (sbyte)r.ReadByte();
                        var sz = r.ReadByte();
                        sx.TargetContacts.Add((tg, ti, cn, (dx, dy), sz));
                    }
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

                foreach (var sx in Items)
                {
                    w.WriteInt32(itemIndex[sx]);

                    var b = SZ;
                    if (sx.HasState()) b |= S1;
                    if (!string.IsNullOrWhiteSpace(sx.Name)) b |= S2;
                    if (!string.IsNullOrWhiteSpace(sx.Summary)) b |= S3;
                    if (!string.IsNullOrWhiteSpace(sx.Description)) b |= S4;
                    if (sx.Data != null && sx.Data.Length > 12) b |= S5;
                    if (sx.Attach != Attach.Normal) b |= S6;
                    if (sx.AutoFlip != AutoFlip.None) b |= S7;

                    w.WriteUInt16(b);
                    if ((b & S1) != 0) w.WriteUInt16(sx.GetState());
                    if ((b & S2) != 0) Value.WriteString(w, sx.Name);
                    if ((b & S3) != 0) Value.WriteString(w, sx.Summary);
                    if ((b & S4) != 0) Value.WriteString(w, sx.Description);
                    if ((b & S5) != 0) Value.WriteBytes(w, sx.Data);
                    if ((b & S6) != 0) w.WriteByte((byte)sx.Attach);
                    if ((b & S7) != 0) w.WriteByte((byte)sx.AutoFlip);
                    var cnt = (byte)sx.TargetContacts.Count;
                    w.WriteByte(cnt);
                    foreach (var e in sx.TargetContacts)
                    {
                        w.WriteUInt16((ushort)e.trg);   //Target
                        w.WriteByte((byte)e.tix);       //TargetIndex
                        w.WriteByte((byte)e.con);       //Contact
                        w.WriteByte((byte)e.pnt.dx);    //sbyte dx
                        w.WriteByte((byte)e.pnt.dy);    //sbyte dy
                        w.WriteByte(e.siz);             //byte size
                    }
                }
            }
        }
        #endregion
    }
}
