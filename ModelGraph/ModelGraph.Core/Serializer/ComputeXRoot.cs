using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class ComputeXRoot : ExternalRoot<ComputeX>, ISerializer, IPrimeRoot
    {
        static Guid _serializerGuid = new Guid("35522B27-A925-4CE0-8D65-EDEF451097F2");
        static byte _formatVersion = 1;
        internal override IdKey IdKey => IdKey.ComputeXRoot;

        internal ComputeXRoot(Root root)
        {
            Owner = root;
            root.RegisterItemSerializer((_serializerGuid, this));
        }

        #region IPrimeRoot  ===================================================
        public void CreateSecondaryHierarchy(Root root)
        {
            var sto = root.Get<PropertyRoot>();

            root.RegisterReferenceItem(new Property_ComputeX_CompuType(sto));
            root.RegisterReferenceItem(new Property_ComputeX_Where(sto));
            root.RegisterReferenceItem(new Property_ComputeX_Select(sto));
            root.RegisterReferenceItem(new Property_ComputeX_Separator(sto));
            root.RegisterReferenceItem(new Property_ComputeX_ValueType(sto));

            root.RegisterStaticProperties(typeof(ComputeX), GetProps(root)); //used by property name lookup
        }

        public void RegisterRelationalReferences(Root root)
        {
            root.RegisterChildRelation(this, root.Get<Relation_ComputeX_QueryX>());

            root.RegisterParentRelation(this, root.Get<Relation_Store_ComputeX>());
            root.RegisterParentRelation(this, root.Get<Relation_Store_NameProperty>());
            root.RegisterParentRelation(this, root.Get<Relation_Store_SummaryProperty>());
            root.RegisterParentRelation(this, root.Get<Relation_QueryX_Property>());
        }


        private Property[] GetProps(Root root) => new Property[]
        {
            root.Get<Property_Item_Name>(),
            root.Get<Property_Item_Summary>(),
            root.Get<Property_Item_Description>(),

            root.Get<Property_ComputeX_CompuType>(),
            root.Get<Property_ComputeX_Where>(),
            root.Get<Property_ComputeX_Select>(),
            root.Get<Property_ComputeX_Separator>(),
            root.Get<Property_ColumnX_ValueType>(),
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

                    var cx = new ComputeX(this);
                    items[index] = cx;

                    var b = r.ReadByte();
                    if ((b & B1) != 0) cx.SetState(r.ReadUInt16());
                    if ((b & B2) != 0) cx.Name = Value.ReadString(r);
                    if ((b & B3) != 0) cx.Summary = Value.ReadString(r);
                    if ((b & B4) != 0) cx.Description = Value.ReadString(r);
                    if ((b & B5) != 0) cx.Separator = Value.ReadString(r);
                    if ((b & B6) != 0) cx.CompuType = (CompuType)r.ReadByte();
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

                foreach (var cx in Items)
                {
                    w.WriteInt32(itemIndex[cx]);

                    var b = BZ;
                    if (cx.HasState()) b |= B1;
                    if (!string.IsNullOrWhiteSpace(cx.Name)) b |= B2;
                    if (!string.IsNullOrWhiteSpace(cx.Summary)) b |= B3;
                    if (!string.IsNullOrWhiteSpace(cx.Description)) b |= B4;
                    if (cx.Separator != ComputeX.DefaultSeparator) b |= B5;
                    if (cx.CompuType != CompuType.RowValue) b |= B6;

                    w.WriteByte(b);
                    if ((b & B1) != 0) w.WriteUInt16(cx.GetState());
                    if ((b & B2) != 0) Value.WriteString(w, cx.Name);
                    if ((b & B3) != 0) Value.WriteString(w, cx.Summary);
                    if ((b & B4) != 0) Value.WriteString(w, cx.Description);
                    if ((b & B5) != 0) Value.WriteString(w, (cx.Separator ?? string.Empty));
                    if ((b & B6) != 0) w.WriteByte((byte)cx.CompuType);
                }
            }
        }
        #endregion
    }
}
