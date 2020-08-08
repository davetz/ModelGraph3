using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class TableXRoot : ExternalRoot<Root, TableX>, ISerializer, IPrimeRoot
    {
        static Guid _serializerGuid = new Guid("93EC136C-6C38-474D-844B-6B8326526CB5");
        static byte _formatVersion = 1;
        internal override IdKey IdKey => IdKey.TableXRoot;

        internal TableXRoot(Root root)
        {
            Owner = root;
            root.RegisterItemSerializer((_serializerGuid, this));
        }

        #region IPrimeRoot  ===================================================
        public void CreateSecondaryHierarchy(Root root)
        {
            root.RegisterStaticProperties(typeof(TableX), GetProps(root)); //used by property name lookup
        }
        public void RegisterRelationalReferences(Root root)
        {
            root.RegisterChildRelation(this, root.Get<Relation_Store_QueryX>());
            root.RegisterChildRelation(this, root.Get<Relation_Store_ColumnX>());
            root.RegisterChildRelation(this, root.Get<Relation_Store_ComputeX>());
            root.RegisterChildRelation(this, root.Get<Relation_Store_NameProperty>());
            root.RegisterChildRelation(this, root.Get<Relation_Store_SummaryProperty>());

            InitializeLocalReferences(root);
        }
        public void ValidateDomain(Root root) { }

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

                    var tx = new TableX(this);
                    items[index] = tx;

                    var b = r.ReadByte();
                    if ((b & B1) != 0) tx.SetState(r.ReadUInt16());
                    if ((b & B2) != 0) tx.Name = Value.ReadString(r);
                    if ((b & B3) != 0) tx.Summary = Value.ReadString(r);
                    if ((b & B4) != 0) tx.Description = Value.ReadString(r);

                    var rxCount = r.ReadInt32();
                    if (rxCount < 0) throw new Exception($"Invalid row count {rxCount}");
                    if (rxCount > 0) tx.SetCapacity(rxCount);

                    for (int j = 0; j < rxCount; j++)
                    {
                        var index2 = r.ReadInt32();
                        if (index2 < 0 || index2 >= items.Length) throw new Exception($"Invalid row index {index2}");

                        items[index2] = new RowX(tx);
                    }
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

                foreach (var tx in Items)
                {
                    w.WriteInt32(itemIndex[tx]);

                    var b = BZ;
                    if (tx.HasState()) b |= B1;
                    if (!string.IsNullOrWhiteSpace(tx.Name)) b |= B2;
                    if (!string.IsNullOrWhiteSpace(tx.Summary)) b |= B3;
                    if (!string.IsNullOrWhiteSpace(tx.Description)) b |= B4;

                    w.WriteByte(b);
                    if ((b & B1) != 0) w.WriteUInt16(tx.GetState());
                    if ((b & B2) != 0) Value.WriteString(w, tx.Name);
                    if ((b & B3) != 0) Value.WriteString(w, tx.Summary);
                    if ((b & B4) != 0) Value.WriteString(w, tx.Description);

                    if (tx.Count > 0)
                    {
                        w.WriteInt32(tx.Count);
                        foreach (var rx in tx.Items)
                        {
                            w.WriteInt32(itemIndex[rx]);
                        }
                    }
                    else
                    {
                        w.WriteInt32(0);
                    }
                }
            }
        }
        #endregion

        #region RowXMethods  ==================================================
        //========================================== frequently used references
        private Relation_Store_NameProperty _relation_Store_NameProperty;
        private Relation_Store_SummaryProperty _relation_Store_SummaryProperty;
        private Relation_Store_ColumnX _relation_Store_ColumnX;

        #region InitializeLocalReferences  ====================================
        private void InitializeLocalReferences(Root root)
        {
            _relation_Store_NameProperty = root.Get<Relation_Store_NameProperty>();
            _relation_Store_SummaryProperty = root.Get<Relation_Store_SummaryProperty>();
            _relation_Store_ColumnX = root.Get<Relation_Store_ColumnX>();
        }
        #endregion

            internal string GetRowXNameId(RowX rx)
        {
            var text = _relation_Store_NameProperty.TryGetChild(rx.Owner, out Property p) ? p.Value.GetString(rx) : string.Empty;
            return string.IsNullOrWhiteSpace(text) ? rx.GetIndexId() : text;
        }

        internal string GetRowXSummaryId(RowX rx) =>  _relation_Store_SummaryProperty.TryGetChild(rx.Owner, out Property p) ? p.Value.GetString(rx) : string.Empty;

        internal IList<ColumnX> GetChoiceProperties(TableX tx)
        {
            var selectList = new List<ColumnX>(5);
            if (_relation_Store_ColumnX.TryGetChildren(tx, out IList<ColumnX> cxList))
            {
                foreach (var cx in cxList)
                {
                    if (cx.IsChoice) selectList.Add(cx);
                }
            }
            return selectList;
        }
        #endregion
    }
}
