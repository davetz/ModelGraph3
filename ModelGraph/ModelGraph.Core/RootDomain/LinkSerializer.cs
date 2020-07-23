using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    internal abstract class LinkSerializer 
    {
        static byte _formatVersion = 1;
        protected IRelationRoot _relationStore;

        internal LinkSerializer(IRelationRoot relStore)
        {
            _relationStore = relStore;
        }

        public bool HasData()
        {
            var rlationArray = _relationStore.GetRelationArray();
            foreach (var rel in rlationArray)
            {
                if (rel.HasLinks) return true;
            }
            return false;
        }

        #region ListSizeCode  =================================================
        // Endode the size range info { (n < 256), (n < 65536), (n > 65535) }
        // for the four lists that occur in a Relation object.

        const byte BZ = 0x00;
        //-------------------------List1
        const byte B01 = 0x01;  // n < 256   -- the list size is stored in a byte
        const byte B02 = 0x02;  // n < 65536 -- the list size is stored in a ushort
        const byte B03 = 0x03;  // n > 65535 -- the list size is stored in an int
        //-------------------------List2
        const byte B04 = 0x04;
        const byte B08 = 0x08;
        const byte B0C = 0x0C;
        //-------------------------List3
        const byte B10 = 0x10;
        const byte B20 = 0x20;
        const byte B30 = 0x30;
        //-------------------------List4
        const byte B40 = 0x40;
        const byte B80 = 0x80;
        const byte BC0 = 0xC0;

        byte GetCompositeCode((int, int)[] list) //=== OneToOne Relation
        {
            var len = list.Length;
            if (len < 256) return B10;
            if (len < 65356) return B20;
            return B30;
        }
        byte GetCompositeCode((int, int[])[] children) //=== OneToMany Relation
        {
            var code = BZ;
            var len1 = 0;
            var len3 = children.Length;
            foreach (var (ix1, ix2List) in children)
            {
                var len = ix2List.Length;
                if (len > len1) len1 = len;
            }
            if (len1 < 256) code |= B01;
            else if (len1 < 65356) code |= B02;
            else code |= B03;

            if (len3 < 256) code |= B10;
            else if (len3 < 65536) code |= B20;
            else code |= B30;

            return code;
        }
        byte GetCompositeCode((int, int[])[] parents, (int, int[])[] children) //=== ManyToMany Relation
        {
            var code = BZ;
            var len1 = 0;
            var len2 = 0;
            var len3 = children.Length;
            var len4 = parents.Length;
            foreach (var (ix1, ix2List) in children)
            {
                var len = ix2List.Length;
                if (len > len1) len1 = len;
            }
            if (len1 < 256) code |= B01;
            else if (len1 < 65356) code |= B02;
            else code |= B03;

            foreach (var (ix1, ix2List) in parents)
            {
                var len = ix2List.Length;
                if (len > len2) len2 = len;
            }
            if (len2 < 256) code |= B04;
            else if (len2 < 65356) code |= B08;
            else code |= B0C;

            if (len3 < 256) code |= B10;
            else if (len3 < 65536) code |= B20;
            else code |= B30;

            if (len4 < 256) code |= B40;
            else if (len4 < 65536) code |= B80;
            else code |= BC0;

            return code;
        }

        //=====================================================================
        // Extract the list size information from the composite sizing code
        // and express it in a regular format for each of relation pairing type         
        byte SizeCodeOneToOne(byte code)
        {
            if (code == B10) return B10;// n < 256   -- the list size is stored in a byte
            if (code == B20) return B20;// n < 65536 -- the list size is stored in a ushort
            if (code == B30) return B40;// n > 65535 -- the list size is stored in an int
            throw new Exception($"LinkSerializer ReadData, invalid list size code");
        }
        byte SizeCodeOneToMany(byte code)
        {
            var code1 = BZ;
            if ((code & B03) == B01) 
                code1 |= B01;// n < 256   -- the sub list size is stored in a byte
            else if ((code & B03) == B02) 
                code1 |= B02;// n < 256   -- the sub list size is stored in a byte
            else if ((code & B03) == B03) 
                code1 |= B04;// n < 256   -- the sub list size is stored in a byte
            else
                throw new Exception($"LinkSerializer ReadData, invalid list size code");

            if ((code & B30) == B10)
                code1 |= B10;// n < 256   -- the list size is stored in a byte
            else if ((code & B30) == B20)
                code1 |= B20;// n < 256   -- the list size is stored in a byte
            else if ((code & B30) == B30)
                code1 |= B40;// n < 256   -- the list size is stored in a byte
            else
                throw new Exception($"LinkSerializer ReadData, invalid list size code");

            return code1;
        }
        (byte, byte) SizeCodeManyToMany(byte code)
        {
            var code1 = BZ;
            var code2 = BZ;

            if ((code & B03) == B01)
                code1 |= B01;
            else if ((code & B03) == B02)
                code1 |= B02;
            else if ((code & B03) == B03)
                code1 |= B04;
            else
                throw new Exception($"LinkSerializer ReadData, invalid list size code");

            if ((code & B0C) == B04)
                code2 |= B01;
            else if ((code & B0C) == B08)
                code2 |= B02;
            else if ((code & B0C) == B0C)
                code2 |= B04;
            else
                throw new Exception($"LinkSerializer ReadData, invalid list size code");

            if ((code & B30) == B10)
                code1 |= B10;
            else if ((code & B30) == B20)
                code1 |= B20;
            else if ((code & B30) == B30)
                code1 |= B40;
            else
                throw new Exception($"LinkSerializer ReadData, invalid list size code");

            if ((code & BC0) == B40)
                code2 |= B10;
            else if ((code & BC0) == B80)
                code2 |= B20;
            else if ((code & BC0) == BC0)
                code2 |= B40;
            else
                throw new Exception($"LinkSerializer ReadData, invalid list size code");

            return (code2, code1);
        }
        #endregion

        #region WriteData  ====================================================
        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            var rlationArray = _relationStore.GetRelationArray();
            var N = 0;
            foreach (var rel in rlationArray) { if (rel.HasLinks) N++; } //count number of serialized relations 

            w.WriteInt32(N);                //number of serialized relations 
            w.WriteByte(_formatVersion);    //format version

            foreach (var rel in rlationArray)  //foreach relation entry
            {
                if (rel.HasLinks)
                {
                    w.WriteInt32(itemIndex[rel]);    //relation index
                    w.WriteByte((byte)rel.Pairing);  //pairing cross check, it should match the relation.pairing on reading

                    switch (rel.Pairing)
                    {
                        case Pairing.OneToOne:

                            var list1 = rel.GetChildren1Items(itemIndex);
                            var len = list1.Length;

                            var code1 = GetCompositeCode(list1);
                            w.WriteByte(code1);//===================write composite sizing code;

                            var listSize1 = SizeCodeOneToOne(code1);

                            if ((listSize1 & B10) != 0)
                            {//============================list length < 256
                                w.WriteByte((byte)len);
                            }
                            else if ((listSize1 & B20) != 0)
                            {//============================list length < 65536
                                w.WriteUInt16((ushort)len);
                            }
                            else
                            {//============================list length > 65535
                                w.WriteInt32(len);
                            }

                            foreach (var (ix1, ix2) in list1)
                            {
                                w.WriteInt32(ix1);      //parent item
                                w.WriteInt32(ix2);      //child item
                            }
                            break;

                        case Pairing.OneToMany:

                            var list2 = rel.GetChildren2Items(itemIndex);

                            var code2 = GetCompositeCode(list2);
                            w.WriteByte(code2);//===================write composite sizing code;

                            var listSize2 = SizeCodeOneToMany(code2);

                            WriteList(w, list2, listSize2);   //write the compound list
                            break;

                        case Pairing.ManyToMany:

                            var list3 = rel.GetChildren2Items(itemIndex);
                            var list4 = rel.GetParents2Items(itemIndex);

                            var code3 = GetCompositeCode(list4, list3);
                            w.WriteByte(code3);//===================write composite sizing code;

                            var (listSize4, listSize3) = SizeCodeManyToMany(code3);

                            WriteList(w, list3, listSize3);   //write the compound list
                            WriteList(w, list4, listSize4);  //write the compound list
                            break;
                    }
                }
            }
        }
        #endregion

        #region ReadData  =====================================================
        public void ReadData(DataReader r, Item[] items)
        {
            var N = r.ReadInt32();      //number of  serialized relations 
            var fv = r.ReadByte();      //format version

            for (int i = 0; i < N; i++)
            {
                if (fv == 1)
                {
                    var irel = r.ReadInt32();//=============== read relation index
                    if (irel < 0 || irel > items.Length)
                        throw new Exception($"LinkSerializer ReadData, invalid relation index: {irel}");

                    if (!(items[irel] is Relation rel))
                        throw new Exception($"LinkSerializer ReadData, null relation for index: {irel}");

                    var pairing = (Pairing)r.ReadByte();//==== read pairing type
                    if (pairing > rel.Pairing)
                        throw new Exception($"LinkSerializer ReadData, invalid relation pairing type");

                    var sizeCode = r.ReadByte();//============ read list size code        

                    switch (pairing)
                    {
                        case Pairing.OneToOne:
                            {
                                int len;
                                var listSize = SizeCodeOneToOne(sizeCode);

                                if ((listSize & B10) != 0)
                                {//============================list length < 256
                                    len = r.ReadByte();
                                }
                                else if ((listSize & B20) != 0)
                                {//============================list length < 65536
                                    len = r.ReadUInt16();
                                }
                                else
                                {//============================list length > 65535
                                    len = r.ReadInt32();
                                }

                                var list1 = new (int, int)[len];
                                var list2 = new (int, int)[len];

                                for (int j = 0; j < len; j++)
                                {
                                    var ix1 = r.ReadInt32();
                                    var ix2 = r.ReadInt32();

                                    list1[j] = (ix1, ix2);
                                    list2[j] = (ix2, ix1);
                                }
                                rel.SetChildren1(list1, items);
                                rel.SetParents1(list2, items);
                            }
                            break;

                        case Pairing.OneToMany:
                            {
                                var listSize = SizeCodeOneToMany(sizeCode);

                                var (len, list1) = ReadList(r, listSize);
                                var list2 = new List<(int, int)>(len);
                                foreach (var (ix1, ix2List) in list1)
                                {
                                    foreach (var ix2 in ix2List) { list2.Add((ix2, ix1)); }
                                }
                                if (rel.Pairing == Pairing.ManyToMany) // allow some resiliance for legacy data files
                                {
                                    var list3 = new (int, int[])[list2.Count];
                                    for (int j = 0; j < list2.Count; j++)
                                    {
                                        var (ix1, ix2) = list2[j];
                                        list3[j] = (ix1, new int[1] { ix2 });
                                    }
                                    rel.SetChildren2(list1, items);
                                    rel.SetParents2(list3, items);

                                }
                                else
                                {
                                    rel.SetChildren2(list1, items);
                                    rel.SetParents1(list2.ToArray(), items);
                                }
                            }
                            break;

                        case Pairing.ManyToMany:
                            {
                                var (parentsSize, childrenSize) = SizeCodeManyToMany(sizeCode);

                                var (_, list1) = ReadList(r, childrenSize);
                                var (_, list2) = ReadList(r, parentsSize);

                                rel.SetChildren2(list1, items);
                                rel.SetParents2(list2, items);
                            }
                            break;
                        default:
                            throw new Exception("LinkSerializer ReadData, invalid pairing");
                    }
                }
                else
                    throw new Exception("LinkSerializer ReadData, invalid format version");
            }
        }
        #endregion

        #region WriteList - write the compond list  ===========================
        void WriteList(DataWriter w, (int, int[])[] list, byte listSize)
        {
            var len = list.Length;

            if ((listSize & B10) != 0)
            {//============================list length < 256
                w.WriteByte((byte)len);
            }
            else if ((listSize & B20) != 0)
            {//============================list length < 65536
                w.WriteUInt16((ushort)len);
            }
            else
            {//============================list length > 65535
                w.WriteInt32(len);
            }

            foreach (var ent in list) { WriteOneToMany(w, ent, listSize); }
        }
        #endregion

        #region ReadList - read the compond list  =============================
        (int, (int, int[])[]) ReadList(DataReader r, byte listSize)
        {
            int len;
            if ((listSize & B10) != 0)
            {//============================list length < 256
                len = r.ReadByte();
            }
            else if ((listSize & B20) != 0)
            {//============================list length < 65536
                len = r.ReadUInt16();
            }
            else
            {//============================list length > 65535
                len = r.ReadInt32();
            }

            var list = new (int, int[])[len];
            for (int i = 0; i < len; i++) { list[i] = ReadOneToMany(r, listSize); }

            return (len, list);
        }
        #endregion

        #region WriteOneToMany  ===============================================
        void WriteOneToMany(DataWriter w, (int, int[]) pcList, byte listSize)
        {
            var (ix1, ix2List) = pcList;
            w.WriteInt32(ix1);

            var len = ix2List.Length;
            if ((listSize & B01) != 0)
            {//============================list length < 256
                w.WriteByte((byte)len);
            }
            else if ((listSize & B02) != 0)
            {//============================list length < 65536
                w.WriteUInt16((ushort)len);
            }
            else
            {//============================list length > 65535
                w.WriteInt32(len);
            }

            foreach (var ix2 in ix2List) { w.WriteInt32(ix2); }
        }
        #endregion

        #region ReadOneToMany  ================================================
        (int, int[]) ReadOneToMany(DataReader r, byte listSize)
        {
            int len;
            var ix1 = r.ReadInt32();

            if ((listSize & B01) != 0)
            {//============================list length < 256
                len = r.ReadByte();
            }
            else if ((listSize & B02) != 0)
            {//============================list length < 65536
                len = r.ReadUInt16();
            }
            else 
            {//============================list length > 65535
                len = r.ReadInt32();
            }
            var ix2List = new int[len];
            for (int i = 0; i < len; i++) { ix2List[i] = r.ReadInt32(); }
            return (ix1, ix2List);
        }
        #endregion
    }
}
