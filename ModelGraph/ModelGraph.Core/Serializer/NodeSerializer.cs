using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class NodeSerializer : ISerializer
    {
        static Guid _serializerGuid = new Guid("88223880-DC1E-40DD-BFB2-711372B7BA2D");
        static byte _formatVersion = 1;
        private Root DataChef;
        internal NodeSerializer(Root root)
        {
            DataChef = root;

            CreateProperties(root);
            root.RegisterLinkSerializer((_serializerGuid, this));
        }

        #region CreateProperties  =============================================
        private void CreateProperties(Root root)
        {
            var sto = root.Get<PropertyRoot>();

            root.RegisterReferenceItem(new Property_Node_CenterXY(sto));
            root.RegisterReferenceItem(new Property_Node_SizeWH(sto));
            root.RegisterReferenceItem(new Property_Node_Aspect(sto));
            root.RegisterReferenceItem(new Property_Node_Labeling(sto));
            root.RegisterReferenceItem(new Property_Node_Resizing(sto));
            root.RegisterReferenceItem(new Property_Node_BarWidth(sto));

            root.RegisterReferenceItem(new Property_Edge_Facet1(sto));
            root.RegisterReferenceItem(new Property_Edge_Facet2(sto));

            root.RegisterStaticProperties(typeof(Node), GetProps1(root)); //used by property name lookup
            root.RegisterStaticProperties(typeof(Edge), GetProps2(root)); //used by property name lookup
        }
        private Property[] GetProps1(Root root) => new Property[]
        {
            root.Get<Property_Node_CenterXY>(),
            root.Get<Property_Node_SizeWH>(),
            root.Get<Property_Node_Aspect>(),
            root.Get<Property_Node_Labeling>(),
            root.Get<Property_Node_Resizing>(),
            root.Get<Property_Node_BarWidth>(),
        };
        private Property[] GetProps2(Root root) => new Property[]
        {
            root.Get<Property_Edge_Facet1>(),
            root.Get<Property_Edge_Facet2>(),
        };
        #endregion

        #region HasData  ======================================================
        public bool HasData()
        {

            var root = DataChef;
            var graphXDomain = root.Get<GraphXRoot>();
            var dummyQueryXRef = root.Get<DummyQueryX>();


            var gxList = graphXDomain.Items;
            foreach (var gx in gxList)
            {
                if (gx.Root_QueryX_Parms is null) continue;
                foreach (var e1 in gx.Root_QueryX_Parms)
                {
                    if (e1.Key is null) continue;
                    if (e1.Key.IsDeleted) continue;
                    if (e1.Value is null) continue;
                    foreach (var e2 in e1.Value)
                    {
                        if (e2.Key is null) continue;
                        if (e2.Key.IsDeleted) continue;
                        if (e2.Value is null) continue;
                        if (e2.Value.Count == 0) continue;
                        if (e2.Key == dummyQueryXRef)
                        {
                            foreach (var e3 in e2.Value)
                            {
                                if (!(e3 is Node nd)) continue;
                                if (nd.Item is null) continue;
                                if (nd.Item.IsDeleted) continue;

                                return true;
                            }                                    
                        }
                        else
                        {
                            foreach (var e3 in e2.Value)
                            {
                                if (!(e3 is Edge eg)) continue;
                                if (eg.Node1 is null) continue;
                                if (eg.Node2 is null) continue;
                                if (eg.Node1.Item is null) continue;
                                if (eg.Node2.Item is null) continue;
                                if (eg.Node1.Item.IsDeleted) continue;
                                if (eg.Node2.Item.IsDeleted) continue;

                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        #region ReadData  =====================================================
        public void ReadData(DataReader r, Item[] items)
        {
            var root = DataChef;
            var dummyQueryXRef = root.Get<DummyQueryX>();

            var Item_Node = new Dictionary<Item,Node>(1000);


            var gxCount = r.ReadUInt16();//***********************
            var fv = r.ReadByte();//******************************

            if (fv == 1)
            {
                #region FormatVersion-1  ========================
                for (int i = 0; i < gxCount; i++)
                {
                    var gxIndex = r.ReadInt32();//******************************
                    if (gxIndex < 0 || gxIndex >= items.Length) throw new Exception($"Invalid index {gxIndex}");
                    if (!(items[gxIndex] is GraphX gx)) throw new Exception($"Expected graphDef object, got null {gxIndex}");

                    var riCount = r.ReadUInt16();//******************************

                    var graphParms = new Dictionary<Item, Dictionary<QueryX, List<NodeEdge>>>(riCount);
                    gx.Root_QueryX_Parms = graphParms;

                    for (int j = 0; j < riCount; j++)
                    {
                        Item_Node.Clear();// each graph root item will hav a new set of nodes

                        var rootIndex = r.ReadInt32();//*************************
                        if (rootIndex < 0 || rootIndex >= items.Length) throw new Exception($"Invalid root index {rootIndex}");
                        if (!(items[rootIndex] is Item seed)) throw new Exception($"GraphXParam root item is null {rootIndex}");

                        var qxCount = r.ReadUInt16();//***************************
                        var QueryX_NodeEdgeList = new Dictionary<QueryX, List<NodeEdge>>(qxCount);
                        gx.Root_QueryX_Parms[seed] = QueryX_NodeEdgeList;

                        for (int k = 0; k < qxCount; k++)
                        {
                            var qxIndex = r.ReadInt32();//*********************
                            if (qxIndex < 0 || qxIndex > items.Length) throw new Exception($"Invalid index {qxIndex}");
                            if (!(items[qxIndex] is QueryX qx)) throw new Exception($"Invalid QueryX item index, {qxIndex}");

                            var neCount = r.ReadInt32();//*********************
                            var NodeEdgeList = new List<NodeEdge>(neCount);
                            QueryX_NodeEdgeList[qx] = NodeEdgeList;

                            if (qx == dummyQueryXRef)
                            {
                                #region ReadNodeParams  =======================
                                for (int l = 0; l < neCount; l++)
                                {
                                    var niIndex = r.ReadInt32();//*******************
                                    if (niIndex < 0 || niIndex >= items.Length) throw new Exception($"Invalid node index {niIndex}");
                                    if (!(items[niIndex] is Item item)) throw new Exception($"Invalid node item index, {niIndex}");

                                    var node = new Node() 
                                    { 
                                        Item = item,
                                        X = r.ReadSingle(),
                                        Y = r.ReadSingle(),
                                        DX = r.ReadByte(),
                                        DY = r.ReadByte(),
                                        Aspect = (Aspect)r.ReadByte(),
                                        FlipState = (FlipState)r.ReadByte(),
                                        Labeling = (Labeling)r.ReadByte(),
                                        Sizing = (Sizing)r.ReadByte(),
                                        BarWidth = (BarWidth)r.ReadByte(),
                                    };
                                    Item_Node[item] = node;
                                    NodeEdgeList.Add(node);
                                }
                                #endregion
                            }
                            else
                            {
                                #region ReadEdgeParams  =======================
                                for (int l = 0; l < neCount; l++)
                                {
                                    var itm1Index = r.ReadInt32();
                                    if (itm1Index < 0 || itm1Index >= items.Length) throw new Exception($"Invalid index {itm1Index}");

                                    var itm1 = items[itm1Index];
                                    if (itm1 == null) throw new Exception($"Expected node object, got null {itm1Index}");

                                    var itm2Index = r.ReadInt32();
                                    if (itm2Index < 0 || itm2Index >= items.Length) throw new Exception($"Invalid index {itm2Index}");

                                    var itm2 = items[itm2Index];
                                    if (itm2 == null) throw new Exception($"Expected node object, got null {itm2Index}");

                                    if (!Item_Node.TryGetValue(itm1, out Node node1)) throw new Exception("Could not Finde Item1Node");
                                    if (!Item_Node.TryGetValue(itm2, out Node node2)) throw new Exception("Could not Finde Item2Node");

                                    var edge = new Edge(qx) 
                                    { 
                                        Node1 = node1, 
                                        Node2 = node2
                                    };
                                    NodeEdgeList.Add(edge);

                                    var b = r.ReadByte();

                                    if ((b & B1) != 0) edge.Facet1 = (Facet)r.ReadByte();
                                    if ((b & B2) != 0) edge.Facet2 = (Facet)r.ReadByte();
                                    if ((b & B3) != 0)
                                    {
                                        var pnCount = r.ReadUInt16();
                                        edge.Bends = new (float X, float Y)[pnCount];
                                        for (int n = 0; n < pnCount; n++)
                                        {
                                            edge.Bends[n].X = r.ReadSingle();
                                            edge.Bends[n].Y = r.ReadSingle();
                                        }
                                    }
                                    edge.SP1 = ((sbyte)r.ReadByte(), (sbyte)r.ReadByte());
                                    edge.FP1 = ((sbyte)r.ReadByte(), (sbyte)r.ReadByte());
                                    edge.TP1 = (r.ReadInt16(), r.ReadInt16());

                                    edge.SP2 = ((sbyte)r.ReadByte(), (sbyte)r.ReadByte());
                                    edge.FP2 = ((sbyte)r.ReadByte(), (sbyte)r.ReadByte());
                                    edge.TP2 = (r.ReadInt16(), r.ReadInt16());

                                }
                                #endregion
                            }
                        }
                    }
                }
                #endregion
            }
            else
                throw new Exception($"GraphXParams ReadData, unknown format version: {fv}");
        }
        #endregion

        #region WriteData  ====================================================
        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            var root = DataChef;
            var dummyQueryXRef = root.Get<DummyQueryX>();
            var graphXDomain = root.Get<GraphXRoot>();

            #region RemoveInvalidItems  =======================================
            // hit list of items that no longer exists
            var rtList = new List<Item>();
            var rtqxList = new List<(Item ri, QueryX qx)>();
            var rtqxneList = new List<(Item ri, QueryX qx, NodeEdge ge)>();

            // find items that are referenced in the graph parms, but no longer exist
            foreach (var gx in graphXDomain.Items)//GD
            {
                rtList.Clear();
                rtqxList.Clear();
                rtqxneList.Clear();

                foreach (var e1 in gx.Root_QueryX_Parms)
                {
                    var ri = e1.Key;
                    if (itemIndex.ContainsKey(ri))
                    {
                        foreach (var e2 in e1.Value)
                        {
                            var qx = e2.Key;
                            if (itemIndex.ContainsKey(qx))
                            {
                                if (qx == dummyQueryXRef)
                                {
                                    foreach (var ne in e2.Value)
                                    {
                                        var nd = ne as Node;
                                        if (itemIndex.ContainsKey(nd.Item)) continue;
                                        rtqxneList.Add((ri, qx, ne));
                                    }
                                }
                                else
                                {
                                    foreach (var ne in e2.Value)//GP
                                    {
                                        var eg = ne as Edge;
                                        if (itemIndex.ContainsKey(eg.Node1.Item) && itemIndex.ContainsKey(eg.Node2.Item)) continue;
                                        rtqxneList.Add((ri, qx, ne));
                                    }
                                }
                            }
                            else
                            {
                                rtqxList.Add((ri, qx));
                            }
                        }
                    }
                    else
                    {
                        rtList.Add(ri);
                    }
                }

                // remove graphParams items which no longer exists
                foreach (var ri in rtList)
                {
                    gx.Root_QueryX_Parms.Remove(ri);
                }
                foreach (var (ri, qx) in rtqxList)
                {
                    gx.Root_QueryX_Parms[ri].Remove(qx);
                    if (gx.Root_QueryX_Parms[ri].Count == 0)
                        gx.Root_QueryX_Parms.Remove(ri);
                }
                foreach (var (ri, qx, ne) in rtqxneList)
                {
                    gx.Root_QueryX_Parms[ri][qx].Remove(ne);
                    if (gx.Root_QueryX_Parms[ri][qx].Count == 0)
                        gx.Root_QueryX_Parms[ri].Remove(qx);
                    if (gx.Root_QueryX_Parms[ri].Count == 0)
                        gx.Root_QueryX_Parms.Remove(ri);
                }
            }
            #endregion

            // count number of gx.GraphParams that have data
            var gxCount = 0;
            foreach (var gx in graphXDomain.Items)//GraphX
            {
                if (gx.Root_QueryX_Parms.Count > 0) gxCount++;
            }
            w.WriteUInt16((ushort)gxCount);//**********************************
            w.WriteByte(_formatVersion);//*************************************

            // now write the remaining valid graph params to the storage file
            foreach (var gx in graphXDomain.Items)//GraphX
            {
                if (gx.Root_QueryX_Parms.Count == 0) continue;

                w.WriteInt32(itemIndex[gx]);//*********************************
                w.WriteUInt16((ushort)gx.Root_QueryX_Parms.Count);//******************

                foreach (var e1 in gx.Root_QueryX_Parms)//RootItem
                {
                    w.WriteInt32(itemIndex[e1.Key]);//************************* rootIndex
                    w.WriteUInt16((ushort)e1.Value.Count);//******************* qxCount

                    if (e1.Value.Count > 0)
                    {
                        #region WriteRoots  ===================================
                        var (x0, y0) = GetCenter(root, e1.Value); // used to center the drawing arround point(0,0)

                        foreach (var e2 in e1.Value)//QueryX
                        {
                            w.WriteInt32(itemIndex[e2.Key]);//*****************
                            w.WriteInt32(e2.Value.Count);//********************

                            if (e2.Value.Count > 0)
                            {
                                #region WriteQuerys  ==========================
                                if (e2.Key == dummyQueryXRef)
                                {
                                    #region WriteNodes  =======================
                                    foreach (var en in e2.Value)//NodeEdge
                                    {
                                        var nd = en as Node;
                                        w.WriteInt32(itemIndex[nd.Item]);//****

                                        w.WriteSingle(nd.X - x0);//************
                                        w.WriteSingle(nd.Y - y0);//************
                                        w.WriteByte(nd.DX);//******************
                                        w.WriteByte(nd.DY);//******************
                                        w.WriteByte((byte)nd.Aspect);//********
                                        w.WriteByte((byte)nd.FlipState);//*****
                                        w.WriteByte((byte)nd.Labeling);//******
                                        w.WriteByte((byte)nd.Sizing);//********
                                        w.WriteByte((byte)nd.BarWidth);//******
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region WriteEdges  =======================
                                    foreach (var ne in e2.Value)//NodeEdge
                                    {
                                        var eg = ne as Edge;
                                        w.WriteInt32(itemIndex[eg.Node1.Item]);//************
                                        w.WriteInt32(itemIndex[eg.Node2.Item]);//************

                                        var b = BZ;
                                        if (eg.Facet1 != Facet.None) b |= B1;
                                        if (eg.Facet2 != Facet.None) b |= B2;
                                        if (eg.HasBends) b |= B3;

                                        w.WriteByte(b);//************************************

                                        if ((b & B1) != 0) w.WriteByte((byte)eg.Facet1);//***
                                        if ((b & B2) != 0) w.WriteByte((byte)eg.Facet2);//***
                                        if ((b & B3) != 0)
                                        {
                                            var len = eg.Bends.Length;
                                            w.WriteUInt16((ushort)len);//********************
                                            for (int i = 0; i < len; i++)
                                            {
                                                w.WriteSingle(eg.Bends[i].X - x0);//*********
                                                w.WriteSingle(eg.Bends[i].Y - y0);//*********
                                            }
                                        }
                                        w.WriteByte((byte)eg.SP1.dx);//**********************
                                        w.WriteByte((byte)eg.SP1.dy);//**********************
                                        w.WriteByte((byte)eg.FP1.dx);//**********************
                                        w.WriteByte((byte)eg.FP1.dy);//**********************
                                        w.WriteInt16(eg.TP1.dx);//***************************
                                        w.WriteInt16(eg.TP1.dy);//***************************

                                        w.WriteByte((byte)eg.SP2.dx);//**********************
                                        w.WriteByte((byte)eg.SP2.dy);//**********************
                                        w.WriteByte((byte)eg.FP2.dx);//**********************
                                        w.WriteByte((byte)eg.FP2.dy);//**********************
                                        w.WriteInt16(eg.TP2.dx);//***************************
                                        w.WriteInt16(eg.TP2.dy);//***************************
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                        }
                        #endregion
                    }
                }
            }
        }

        #region GetCenter  ====================================================
        private (float X0, float Y0) GetCenter(Root root, Dictionary<QueryX, List<NodeEdge>> qxParams)
        {
            float x1, y1, x2, y2;
            x1 = y1 = float.MaxValue;
            x2 = y2 = float.MinValue;
            foreach (var e3 in qxParams)
            {
                if (e3.Key == root.Get<DummyQueryX>())
                {
                    foreach (var gp in e3.Value)//GP
                    {
                        var nd = gp as Node;
                        var (x, y) = nd.GetCenter();
                        {
                            if (x < x1) x1 = x;
                            if (y < y1) y1 = y;
                            if (x > x2) x2 = x;
                            if (y > y2) y2 = y;
                        }
                    }
                }
            }
            return (x1 == float.MaxValue) ? (0, 0) : ((x1 + x2) / 2, (y1 + y2) / 2);
        }
        #endregion
        #endregion


        #region ISerializer  ==================================================
        public int GetSerializerItemCount() => 0;
        public void PopulateItemIndex(Dictionary<Item, int> itemIndex) { }
        public void RegisterInternal(Dictionary<int, Item> internalItem) { }
        #endregion


        #region Flags  ========================================================
        // don't read/write missing or default-value propties
        // these flags indicate which properties were non-default
        public const byte BZ = 0;
        public const byte B1 = 0x1;
        public const byte B2 = 0x2;
        public const byte B3 = 0x4;
        public const byte B4 = 0x8;
        public const byte B5 = 0x10;
        public const byte B6 = 0x20;
        public const byte B7 = 0x40;
        public const byte B8 = 0x80;

        public const ushort SZ = 0;
        public const ushort S1 = 0x1;
        public const ushort S2 = 0x2;
        public const ushort S3 = 0x4;
        public const ushort S4 = 0x8;
        public const ushort S5 = 0x10;
        public const ushort S6 = 0x20;
        public const ushort S7 = 0x40;
        public const ushort S8 = 0x80;
        public const ushort S9 = 0x100;
        public const ushort S10 = 0x200;
        public const ushort S11 = 0x400;
        public const ushort S12 = 0x800;
        public const ushort S13 = 0x1000;
        public const ushort S14 = 0x2000;
        public const ushort S15 = 0x4000;
        public const ushort S16 = 0x8000;
        #endregion

    }
}
