using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public partial class Root : ISerializer
    {
        Guid _formatGuid = new Guid("3DB85BFF-F448-465C-996D-367E6284E913");
        Guid _serilizerGuid = new Guid("DE976A9D-0C50-4B4E-9B46-74404A64A703");
        static byte _formatVersion = 1;

        #region Serialize/Deserialize  ========================================
        public void Serialize(DataWriter w)
        {
            var serializers = new List<(Guid, ISerializer)>(ItemSerializers);
            serializers.AddRange(LinkSerializers);

            var itemIndex = GetItemIndexDictionary();

            w.WriteGuid(_formatGuid);
            w.WriteInt32(itemIndex.Count);
            foreach (var (g, s) in serializers)
            {
                if (s.HasData())
                {
                    w.WriteGuid(g);
                    s.WriteData(w, itemIndex);
                }
            }
            w.WriteGuid(_formatGuid);
        }
        public void Deserialize(DataReader r)
        {
            var serializers = new List<(Guid, ISerializer)>(ItemSerializers);
            serializers.AddRange(LinkSerializers);

            var guid = r.ReadGuid();
            if (guid != _formatGuid) throw new Exception("Invalid serializer format");

            var count = r.ReadInt32();
            var items = new Item[count];

            while ((guid = r.ReadGuid()) != _formatGuid)
            {
                var found = false;
                foreach (var (g, s) in serializers)
                {
                    if (g != guid) continue;
                    found = true;
                    s.ReadData(r, items);
                    break;
                }
                if (!found) throw new Exception("Unknown serializer guid reference");
            }
        }
        Dictionary<Item, int> GetItemIndexDictionary()
        {
            var maxSize = 4;
            foreach (var itm in Type_InstanceOf.Values)
            {
                if (itm is ISerializer s) maxSize += s.GetSerializerItemCount();
            }
            var itemIndex = new Dictionary<Item, int>(maxSize)
            {
                [Get<DummyItem>()] = 0,
                [Get<DummyQueryX>()] = 0
            };

            foreach (var itm in Type_InstanceOf.Values)
            {
                if (itm is ISerializer s) s.PopulateItemIndex(itemIndex);
            }
            return itemIndex;
        }
        #endregion

        #region ISerializer  ==================================================
        public bool HasData() => true;
        public void ReadData(DataReader r, Item[] items)
        {
            var relation_StoreX_ChildRelation = Get<Relation_StoreX_ChildRelation>();
            var relation_StoreX_ParentRelation = Get<Relation_StoreX_ParentRelation>();
            var property_Item_Name = Get<Property_Item_Name>();
            var property_Item_Summary = Get<Property_Item_Summary>();
            var property_QueryX_Select = Get<Property_QueryX_Select>();
            var property_QueryX_Where = Get<Property_QueryX_Where>();

            var count = r.ReadUInt16();
            if (count > items.Length)
                throw new Exception("Invalid number of guid references");

            var fv = r.ReadByte();

            if (fv == 1)
            {
                for (int i = 0; i < count; i++)
                {
                    var key = r.ReadUInt16();
                    if (IdKey_ReferenceItem.TryGetValue(key, out Item item))
                    {
                        if (item is Relation_Store_ChildRelation)
                            items[i] = relation_StoreX_ChildRelation;
                        else if (item is Relation_Store_ParentRelation)
                            items[i] = relation_StoreX_ParentRelation;
                        else
                            items[i] = item;
                    }
                    else
                    {//==================================Refactor Patch
                        if (key == (ushort)(IdKey.Store_ChildRelation & IdKey.KeyMask))
                            items[i] = relation_StoreX_ChildRelation;
                        else if (key == (ushort)(IdKey.Store_ParentRelation & IdKey.KeyMask))
                            items[i] = relation_StoreX_ParentRelation;
                        if (key == (ushort)(IdKey.EnumNameProperty & IdKey.KeyMask))
                            items[i] = property_Item_Name;
                        else if (key == (ushort)(IdKey.TableNameProperty & IdKey.KeyMask))
                            items[i] = property_Item_Name;
                        else if (key == (ushort)(IdKey.ColumnNameProperty & IdKey.KeyMask))
                            items[i] = property_Item_Name;
                        else if (key == (ushort)(IdKey.RelationNameProperty & IdKey.KeyMask))
                            items[i] = property_Item_Name;
                        else if (key == (ushort)(IdKey.GraphNameProperty & IdKey.KeyMask))
                            items[i] = property_Item_Name;
                        else if (key == (ushort)(IdKey.SymbolXNameProperty & IdKey.KeyMask))
                            items[i] = property_Item_Name;
                        else if (key == (ushort)(IdKey.ComputeXNameProperty & IdKey.KeyMask))
                            items[i] = property_Item_Name;
                        else if (key == (ushort)(IdKey.EnumSummaryProperty & IdKey.KeyMask))
                            items[i] = property_Item_Summary;
                        else if (key == (ushort)(IdKey.TableSummaryProperty & IdKey.KeyMask))
                            items[i] = property_Item_Summary;
                        else if (key == (ushort)(IdKey.ColumnSummaryProperty & IdKey.KeyMask))
                            items[i] = property_Item_Summary;
                        else if (key == (ushort)(IdKey.RelationSummaryProperty & IdKey.KeyMask))
                            items[i] = property_Item_Summary;
                        else if (key == (ushort)(IdKey.GraphSummaryProperty & IdKey.KeyMask))
                            items[i] = property_Item_Summary;
                        else if (key == (ushort)(IdKey.ComputeXSummaryProperty & IdKey.KeyMask))
                            items[i] = property_Item_Summary;
                        else if (key == (ushort)(IdKey.ValueXSelectProperty & IdKey.KeyMask))
                            items[i] = property_QueryX_Select;
                        else if (key == (ushort)(IdKey.ValueXWhereProperty & IdKey.KeyMask))
                            items[i] = property_QueryX_Where;
                        else
                            throw new Exception("Unkown key reference");
                    }
                }
            }
            else
                throw new Exception($"Chef ReadData, unknown format version: {fv}");
        }

        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            var referenceItems = new List<Item>(50);
            var index = 0;
            var items = new List<Item>(itemIndex.Keys);
            foreach (var item in items)
            {
                if (item.IsExternal) continue;

                referenceItems.Add(item);
                itemIndex[item] = index++;
            }

            w.WriteUInt16((ushort)index);
            w.WriteByte(_formatVersion);
            foreach (var item in referenceItems)
            {
                w.WriteUInt16((ushort)item.ItemKey); //referenced internal item
            }

            foreach (var item in items)
            {
                if (item.IsExternal) itemIndex[item] = index++;
            }
        }

        public int GetSerializerItemCount() => 0;
        public void PopulateItemIndex(Dictionary<Item, int> itemIndex) { }
        public void RegisterInternal(Dictionary<int, Item> internalItem) { }
        #endregion
    }
}
