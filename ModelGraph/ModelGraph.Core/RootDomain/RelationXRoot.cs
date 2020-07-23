using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class RelationXRoot : ExternalRoot<Root, Relation>, ISerializer, IPrimeRoot, IRelationRoot
    {
        static Guid _serializerGuid = new Guid("D950F508-B774-4838-B81A-757EFDC40518");
        static byte _formatVersion = 1;
        internal override IdKey IdKey => IdKey.RelationXRoot;

        internal PropertyOf<Relation, string> NameProperty;
        internal PropertyOf<Relation, string> SummaryProperty;
        internal PropertyOf<Relation, string> PairingProperty;
        internal PropertyOf<Relation, bool> IsRequiredProperty;


        #region Constructor  ==================================================
        internal RelationXRoot(Root root)
        {
            Owner = root;
            root.RegisterItemSerializer((_serializerGuid, this));
            new RelationXLink(root, this);
        }
        #endregion


        #region IPrimeRoot  ===================================================
        public void CreateSecondaryHierarchy(Root root)
        {
            var sto = root.Get<PropertyRoot>();

            root.RegisterReferenceItem(new Property_Relation_Pairing(sto));
            root.RegisterReferenceItem(new Property_Relation_IsRequired(sto));

            root.RegisterStaticProperties(typeof(Relation), GetProps(root)); //used by property name lookup
        }

        public void RegisterRelationalReferences(Root root)
        {
            root.RegisterChildRelation(this, root.Get<Relation_Relation_QueryX>());
            root.RegisterChildRelation(this, root.Get<Relation_Relation_ViewX>());

            InitializeLocalReferences(root);
        }

        private Property[] GetProps(Root root) => new Property[]
        {
            root.Get<Property_Item_Name>(),
            root.Get<Property_Item_Summary>(),
            root.Get<Property_Item_Description>(),

            root.Get<Property_Relation_Pairing>(),
            root.Get<Property_Relation_IsRequired>(),
        };
        #endregion

        #region IRelationStore  ===============================================
        public Relation[] GetRelationArray()
        {
            var relationArray = new Relation[Count];
            for (int i = 0; i < Count; i++)
            {
                relationArray[i] = Items[i];
            }
            return relationArray;
        }
        #endregion

        #region ISerializer  ==================================================
        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            w.WriteInt32(Count);
            w.WriteByte(_formatVersion);

            foreach (var rx in Items)
            {
                w.WriteInt32(itemIndex[rx]);

                var keyCount = rx.KeyCount;
                var valCount = rx.ValueCount;

                var b = BZ;
                if (rx.HasState()) b |= B1;
                if (!string.IsNullOrWhiteSpace(rx.Name)) b |= B2;
                if (!string.IsNullOrWhiteSpace(rx.Summary)) b |= B3;
                if (!string.IsNullOrWhiteSpace(rx.Description)) b |= B4;
                if (rx.Pairing != Pairing.OneToMany) b |= B5;
                if ((keyCount + valCount) > 0) b |= B6;

                w.WriteByte(b);
                if ((b & B1) != 0) w.WriteUInt16(rx.GetState());
                if ((b & B2) != 0) Value.WriteString(w, rx.Name);
                if ((b & B3) != 0) Value.WriteString(w, rx.Summary);
                if ((b & B4) != 0) Value.WriteString(w, rx.Description);
                if ((b & B5) != 0) w.WriteByte((byte)rx.Pairing);
                if ((b & B6) != 0) w.WriteInt32(keyCount);
                if ((b & B6) != 0) w.WriteInt32(valCount);
            }
        }
        public void ReadData(DataReader r, Item[] items)
        {
            var N = r.ReadInt32();
            if (N < 0) throw new Exception($"Invalid count {N}");
            SetCapacity(N);

            var fv = r.ReadByte();
            if (fv == 1)
            {
                for (int i = 0; i < N; i++)
                {
                    var index = r.ReadInt32();
                    if (index < 0 || index >= items.Length) throw new Exception($"RelationXStore ReadData, invalid index {index}");

                    var rx = new RelationX_RowX_RowX(this);
                    items[index] = rx;

                    var b = r.ReadByte();
                    if ((b & B1) != 0) rx.SetState(r.ReadUInt16());
                    if ((b & B2) != 0) rx.Name = Value.ReadString(r);
                    if ((b & B3) != 0) rx.Summary = Value.ReadString(r);
                    if ((b & B4) != 0) rx.Description = Value.ReadString(r);
                    if ((b & B5) != 0) rx.Pairing = (Pairing)r.ReadByte();
                    var keyCount = ((b & B6) != 0) ? r.ReadInt32() : 0;
                    var valCount = ((b & B6) != 0) ? r.ReadInt32() : 0;
                    rx.Initialize(keyCount, valCount);
                }
            }
            else
                throw new Exception($"RelationXStore ReadData, unknown format version: {fv}");
        }
        #endregion

        #region RelationMethods  ==============================================
        //========================================== frequently used references
        private Relation_StoreX_ChildRelation Store_ChildRelation;
        private Relation_StoreX_ParentRelation Store_ParentRelation;

        #region InitializeLocalReferences  ====================================
        private void InitializeLocalReferences(Root root)
        {
            Store_ChildRelation = root.Get<Relation_StoreX_ChildRelation>();
            Store_ParentRelation = root.Get<Relation_StoreX_ParentRelation>();
        }
        #endregion

        #region GetHeadTail  ==================================================
        public (Store, Store) GetHeadTail()
        {
            Store head, tail;
            Store_ChildRelation.TryGetParent(this, out head);
            Store_ParentRelation.TryGetParent(this, out tail);

            return (head, tail);
        }

        #endregion

        #endregion
    }
}
