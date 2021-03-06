﻿using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class ColumnXRoot : ExternalRoot<Root, ColumnX>, ISerializer, IPrimeRoot
    {
        static Guid _serializerGuid = new Guid("3E7097FE-22D5-43B2-964A-9DB843F6D55B");
        static byte _formatVersion = 1;
        internal override IdKey IdKey => IdKey.ColumnXRoot;

        internal ColumnXRoot(Root root) 
        { 
            Owner = root;
            root.RegisterItemSerializer((_serializerGuid, this));
        }

        #region IPrimeRoot  ===================================================
        public void CreateSecondaryHierarchy(Root root)
        {
            var sto = root.Get<PropertyRoot>();

            root.RegisterReferenceItem(new Property_ColumnX_ValueType(sto));
            root.RegisterReferenceItem(new Property_ColumnX_IsChoice(sto));

            root.RegisterInternalProperties(typeof(ColumnX), GetProps(root)); //used by property name lookup
        }

        public void RegisterRelationalReferences(Root root)
        {
            root.RegisterParentRelation(this, root.Get<Relation_Store_ColumnX>());
            root.RegisterParentRelation(this, root.Get<Relation_EnumX_ColumnX>());
            root.RegisterParentRelation(this, root.Get<Relation_GraphX_ColorProperty>());
            root.RegisterParentRelation(this, root.Get<Relation_Store_NameProperty>());
            root.RegisterParentRelation(this, root.Get<Relation_Store_SummaryProperty>());
            root.RegisterParentRelation(this, root.Get<Relation_QueryX_Property>());

            InitializeLocalReferences(root);
        }
        public void ValidateDomain(Root root) { }

        private Property[] GetProps(Root root) => new Property[]
        {
            root.Get<Property_Item_Name>(),
            root.Get<Property_Item_Summary>(),
            root.Get<Property_Item_Description>(),

            root.Get<Property_ColumnX_ValueType>(),
            root.Get<Property_ColumnX_IsChoice>(),
        };
        #endregion

        #region ISerializer  ==================================================
        #region ReadData  =====================================================
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

                    var cx = new ColumnX(this);
                    items[index] = cx;

                    var b = r.ReadByte();
                    if ((b & B1) != 0) cx.SetState(r.ReadUInt16());
                    if ((b & B2) != 0) cx.Name = Value.ReadString(r);
                    if ((b & B3) != 0) cx.Summary = Value.ReadString(r);
                    if ((b & B4) != 0) cx.Description = Value.ReadString(r);

                    ValueDictionary.ReadData(r, cx, items);
                }
            }
            else
                throw new Exception($"ColumnXDomain ReadData, unknown format version: {fv}");
        }
        #endregion

        #region WriteData  ====================================================
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

                    w.WriteByte(b);
                    if ((b & B1) != 0) w.WriteUInt16(cx.GetState());
                    if ((b & B2) != 0) Value.WriteString(w, cx.Name);
                    if ((b & B3) != 0) Value.WriteString(w, cx.Summary);
                    if ((b & B4) != 0) Value.WriteString(w, cx.Description);

                    cx.Value.WriteData(w, itemIndex);
                }
            }
        }
        #endregion
        #endregion

        #region ColumnXMethods  ===============================================
        //========================================== frequently used references
        private Relation_Store_ColumnX _relation_Store_ColumnX;
        private Relation_EnumX_ColumnX _relation_EumX_ColumnX;

        #region InitializeLocalReferences  ====================================
        private void InitializeLocalReferences(Root root)
        {
            _relation_Store_ColumnX = root.Get<Relation_Store_ColumnX>();
            _relation_EumX_ColumnX = root.Get<Relation_EnumX_ColumnX>();
        }
        #endregion

        internal bool TryGetParent(ColumnX cx, out Store p) => _relation_Store_ColumnX.TryGetParent(cx, out p);

        internal string[] GetEnumXlListValue(ColumnX cx) => (cx.HasEnumXRef && _relation_EumX_ColumnX.TryGetParent(cx, out EnumX ex)) ? ex.GetlListValue() : new string[0];
        internal int GetEnumXlIndexValue(ColumnX cx, Item item) => (cx.HasEnumXRef && _relation_EumX_ColumnX.TryGetParent(cx, out EnumX ex)) ? ex.GetIndexValue(cx, item) : 0;
        internal string GetActualValueAt(ColumnX cx, int index) => (cx.HasEnumXRef && _relation_EumX_ColumnX.TryGetParent(cx, out EnumX ex)) ? ex.GetActualValueAt(index) : InvalidItem;

        internal void SetEnumXIndexValue(ColumnX cx, Item item, int index)
        {
            if (cx.HasEnumXRef && _relation_EumX_ColumnX.TryGetParent(cx, out EnumX ex))
                ex.SetIndexValue(cx, item, index);
        }
        #endregion
    }
}
