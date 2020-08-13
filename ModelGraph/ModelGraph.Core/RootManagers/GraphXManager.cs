using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class GraphXManager : ExternalManager<Root, GraphX>, ISerializer, IPrimeRoot
    {
        static Guid _serializerGuid = new Guid("48C7FA8C-88F1-4203-8E54-3255C1F8C528");
        static byte _formatVersion = 1;
        internal override IdKey IdKey => IdKey.GraphXManager;

        internal GraphXManager(Root root) 
        {
            Owner = root;
            root.RegisterItemSerializer((_serializerGuid, this));
            new NodeSerializer(root);
        }

        #region IPrimeRoot  ===================================================
        public void CreateSecondaryHierarchy(Root root)
        {
            var sto = root.Get<PropertyManager>();

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

            InitializeLocalReferences(root);
        }
        public void ValidateDomain(Root root) { }

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

        #region GraphXMethods  ================================================
        //========================================== frequently used references
        private QueryXManager _queryXRoot;
        private DummyItem _dummyItem;
        private DummyQueryX _dummyQueryX;

        private Relation_QueryX_QueryX _relation_QueryX_QueryX;
        private Relation_Store_QueryX _relation_Store_QueryX;
        private Relation_GraphX_QueryX _relation_GraphX_QueryX;
        private Relation_GraphX_SymbolX _relation_GraphX_SymbolX;
        private Relation_GraphX_SymbolQueryX _relation_GraphX_SymbolQueryX;
        private Relation_Store_ColumnX _relation_Store_ColumnX;
        private Relation_SymbolX_QueryX _relation_SymbolX_QueryX;
        private Relation_Relation_QueryX _relation_Relation_QueryX;
        private Relation_GraphX_ColorColumnX _relation_GraphX_ColorColumnX;

        #region InitializeLocalReferences  ====================================
        private void InitializeLocalReferences(Root root)
        {
            _queryXRoot = root.Get<QueryXManager>();
            _dummyItem = root.Get<DummyItem>();
            _dummyQueryX = root.Get<DummyQueryX>();

            _relation_Store_QueryX = root.Get<Relation_Store_QueryX>();
            _relation_QueryX_QueryX = root.Get<Relation_QueryX_QueryX>();
            _relation_GraphX_QueryX = root.Get<Relation_GraphX_QueryX>();
            _relation_GraphX_SymbolX = root.Get<Relation_GraphX_SymbolX>();
            _relation_GraphX_SymbolQueryX = root.Get<Relation_GraphX_SymbolQueryX>();
            _relation_Store_ColumnX = root.Get<Relation_Store_ColumnX>();
            _relation_SymbolX_QueryX = root.Get<Relation_SymbolX_QueryX>();
            _relation_Relation_QueryX = root.Get<Relation_Relation_QueryX>();
            _relation_GraphX_ColorColumnX = root.Get<Relation_GraphX_ColorColumnX>();
        }
        #endregion

        #region ValidateGraphParms  ===========================================
        // Ensure all edges and nodes have parameters.
        bool ValidateGraphParms(Graph g)
        {
            var GraphX_SymbolX = _relation_GraphX_SymbolX;

            var gx = g.Owner;
            var rt = (g.SeedItem == null) ? _dummyItem : g.SeedItem;
            var anyChange = false;

            #region Build validPathPairs dictionary  ==========================
            var validPathPairs = new Dictionary<QueryX, List<(Item, Item)>>();
            List<(Item, Item)> validItemPair;

            foreach (var (q1, q2) in g.PathQuerys)
            {
                var qx = q1.QueryX;
                if (!validPathPairs.TryGetValue(qx, out validItemPair))
                {
                    validItemPair = new List<(Item, Item)>(g.PathQuerys.Count);
                    validPathPairs.Add(qx, validItemPair);
                }

                g.NodeItems.Add(q1.Item);

                for (int i = 0; i < q2.ItemCount; i++)
                {
                    g.NodeItems.Add(q2.Items[i]);

                    validItemPair.Add((q1.Item, q2.Items[i]));
                }
            }
            #endregion

            if (g.NodeItems.Count > 0)
            {
                if (!gx.Root_QueryX_Parms.TryGetValue(rt, out Dictionary<QueryX, List<NodeEdge>> qxParams))
                {
                    qxParams = new Dictionary<QueryX, List<NodeEdge>>();
                    gx.Root_QueryX_Parms[rt] = qxParams;
                }

                #region Remove invalid QxParams  ==============================

                var invalidQx = new HashSet<QueryX>();
                var invalidQxParam = new Dictionary<QueryX, List<NodeEdge>>();

                foreach (var e1 in qxParams)
                {
                    List<NodeEdge> invalidParams = null;
                    if (e1.Key == _dummyQueryX)
                    {

                        foreach (var pm in e1.Value)
                        {
                            var nd = pm as Node;
                            if (!g.NodeItems.Contains(nd.Item))
                            {
                                if (invalidParams == null)
                                {
                                    invalidParams = new List<NodeEdge>();
                                    invalidQxParam.Add(e1.Key, invalidParams);
                                }
                                invalidParams.Add(pm);
                            }
                        }
                    }
                    else
                    {
                        foreach (var pm in e1.Value)
                        {
                            var eg = pm as Edge;
                            //var id = $"({GetIdentity(eg.Node1.Item, IdentityStyle.Double)})  -->  ({GetIdentity(eg.Node2.Item, IdentityStyle.Double)})";

                            if (!g.NodeItems.Contains(eg.Node1.Item) || !g.NodeItems.Contains(eg.Node2.Item))
                            {
                                if (invalidParams == null)
                                {
                                    invalidParams = new List<NodeEdge>();
                                    invalidQxParam.Add(e1.Key, invalidParams);
                                }
                                invalidParams.Add(pm);
                            }
                            else
                            {
                                validItemPair = null;
                                if (validPathPairs.TryGetValue(eg.QueryX, out validItemPair))
                                {
                                    bool found = false;
                                    foreach (var tp in validItemPair)
                                    {
                                        if (tp.Item1 != eg.Node1.Item || tp.Item2 != eg.Node2.Item) continue;
                                        found = true;
                                        break;
                                    }
                                    if (!found)
                                    {
                                        if (invalidParams == null)
                                        {
                                            invalidParams = new List<NodeEdge>();
                                            invalidQxParam.Add(e1.Key, invalidParams);
                                        }
                                        invalidParams.Add(pm);
                                    }
                                }
                                else
                                {
                                    invalidQx.Add(e1.Key);
                                }
                            }
                        }
                    }
                }

                // remove the invalid graphic params 
                foreach (var qx in invalidQx) { qxParams.Remove(qx); }
                foreach (var e1 in invalidQxParam)
                {
                    foreach (var pm in e1.Value)
                    {
                        if (qxParams.ContainsKey(e1.Key)) qxParams[e1.Key].Remove(pm);
                    }
                    if (qxParams[e1.Key].Count == 0) qxParams.Remove(e1.Key);
                }
                #endregion

                #region Add new QxParams  =====================================

                if (!qxParams.TryGetValue(_dummyQueryX, out List<NodeEdge> parmList))
                {
                    // there weren't any existing node parms,
                    // so create all new ones
                    anyChange = true;
                    parmList = new List<NodeEdge>(g.NodeItems.Count);
                    qxParams.Add(_dummyQueryX, parmList);
                    foreach (var item in g.NodeItems)
                    {
                        Node node1 = new Node
                        {
                            Item = item,
                            Owner = g
                        };
                        var node = node1;
                        parmList.Add(node);
                        g.Nodes.Add(node);
                        g.Item_Node.Add(item, node);
                    }
                }
                else
                {
                    // validate the existing nodes
                    foreach (var pm in parmList)
                    {
                        var node = pm as Node;
                        node.Owner = g;
                        g.Nodes.Add(node);
                        g.Item_Node.Add(node.Item, node);
                    }
                    // add new nodes that where missing
                    foreach (var item in g.NodeItems)
                    {
                        if (!g.Item_Node.ContainsKey(item))
                        {
                            anyChange = true;
                            var node = new Node
                            {
                                Item = item,
                                Owner = g
                            };
                            parmList.Add(node);
                            g.Nodes.Add(node);
                            g.Item_Node.Add(item, node);
                        }
                    }
                }

                foreach (var e1 in validPathPairs)
                {
                    // skip over the nodes, they are already done
                    if (e1.Key == _dummyQueryX) continue;

                    if (!qxParams.TryGetValue(e1.Key, out List<NodeEdge> paramList))
                    {
                        // there weren't any existing edge parms,
                        // so create all new ones
                        paramList = new List<NodeEdge>(e1.Value.Count);
                        qxParams.Add(e1.Key, paramList);
                        anyChange = true;

                        foreach (var pair in e1.Value)
                        {
                            var eg = new Edge(e1.Key)
                            {
                                Owner = g,
                            };
                            g.Edges.Add(eg);
                            paramList.Add(eg);
                            eg.Node1 = g.Item_Node[pair.Item1];
                            eg.Node2 = g.Item_Node[pair.Item2];
                        }
                    }
                    else
                    {
                        // validate the existing edges
                        List<Item> items;
                        var item_items = new Dictionary<Item, List<Item>>();
                        foreach (var pm in paramList)
                        {
                            var eg = pm as Edge;
                            eg.Owner = g;
                            g.Edges.Add(eg);

                            if (!item_items.TryGetValue(eg.Node1.Item, out items))
                            {
                                items = new List<Item>(4);
                                item_items.Add(eg.Node1.Item, items);
                            }
                            items.Add(eg.Node2.Item);
                        }
                        // add new edges that where missing
                        foreach (var pair in e1.Value)
                        {
                            if (item_items.TryGetValue(pair.Item1, out items) && items.Contains(pair.Item2)) continue;

                            anyChange = true;
                            var eg = new Edge(e1.Key)
                            {
                                Owner = g,
                            };
                            g.Edges.Add(eg);
                            paramList.Add(eg);
                            eg.Node1 = g.Item_Node[pair.Item1];
                            eg.Node2 = g.Item_Node[pair.Item2];
                        }
                    }
                }

                #endregion

                #region populate g.Node_Edges  ================================

                foreach (var edge in g.Edges)
                {
                    if (!g.Node_Edges.TryGetValue(edge.Node1, out List<Edge> edgeList))
                    {
                        edgeList = new List<Edge>(2);
                        g.Node_Edges.Add(edge.Node1, edgeList);
                    }
                    edgeList.Add(edge);

                    if (!g.Node_Edges.TryGetValue(edge.Node2, out edgeList))
                    {
                        edgeList = new List<Edge>(2);
                        g.Node_Edges.Add(edge.Node2, edgeList);
                    }
                    edgeList.Add(edge);
                }
                #endregion
            }
            else
            {
                gx.Root_QueryX_Parms.Remove(rt);
            }

            #region OpenPathIndex  ============================================
            var N = g.OpenQuerys.Count;
            for (int i = 0; i < N;)
            {
                var item = g.OpenQuerys[i].Item1.Item;
                var j = i + 1;
                for (; j < N; j++) { if (g.OpenQuerys[j].Item1.Item != item) break; }
                if (g.Item_Node.TryGetValue(item, out Node node)) { node.OpenPathIndex = i; }
                i = j;
            }
            #endregion

            #region AssignSymbolIndex  ========================================

            if (GraphX_SymbolX.TryGetChildren(gx, out IList<SymbolX> symbols))
            {
                g.Symbols = symbols.ToArray();

                var storeNonSymbols = new HashSet<Store>();
                var storeSymbolXQueryX = new Dictionary<Store, (List<SymbolX>, List<QueryX>)>();
                foreach (var node in g.Nodes)
                {
                    var sto = node.Item.Store;
                    if (storeNonSymbols.Contains(sto)) continue;
                    if (storeSymbolXQueryX.ContainsKey(sto)) continue;

                    var sxqx = GetSymbolXQueryX(gx, sto);
                    if (sxqx.symbols == null)
                        storeNonSymbols.Add(sto);
                    else
                        storeSymbolXQueryX.Add(sto, sxqx);
                }

                foreach (var node in g.Nodes)
                {
                    node.Symbol = 0;
                    var row = node.Item;
                    var sto = row.Store;
                    if (storeNonSymbols.Contains(sto)) continue;

                    (var sxList, var qxList) = storeSymbolXQueryX[sto];

                    int i, j;
                    for (j = 0; j < sxList.Count; j++)
                    {
                        var filter = qxList[j].Where;
                        if (filter == null || filter.Matches(row)) break;
                    }
                    if (j == sxList.Count) continue;

                    var sx = sxList[j];
                    for (i = 0; i < symbols.Count; i++)
                    {
                        if (symbols[i] == sx) break;
                    }
                    if (i == symbols.Count) continue;

                    node.Symbol = (byte)(i + 2);
                    node.Aspect = Aspect.Square;
                    if (node.FlipState < FlipState.LeftRotate)
                    {
                        node.DX = (byte)(sx.Width * gx.SymbolScale / 2);
                        node.DY = (byte)(sx.Height * gx.SymbolScale / 2);
                    }
                    else
                    {
                        node.DY = (byte)(sx.Width * gx.SymbolScale / 2);
                        node.DX = (byte)(sx.Height * gx.SymbolScale / 2);
                    }
                }
            }
            #endregion

            return anyChange;
        }
        (List<SymbolX> symbols, List<QueryX> querys) GetSymbolXQueryX(GraphX gx, Store nodeOwner)
        {
            if (_relation_GraphX_SymbolQueryX.TryGetChildren(gx, out IList<QueryX> sqxList))
            {
                var sxList = new List<SymbolX>(sqxList.Count);
                var qxList = new List<QueryX>(sqxList.Count);

                foreach (var qx in sqxList)
                {
                    if (_relation_Store_QueryX.TryGetParent(qx, out Store store) && store == nodeOwner)
                    {
                        if (_relation_SymbolX_QueryX.TryGetParent(qx, out SymbolX sx))
                        {
                            sxList.Add(sx);
                            qxList.Add(qx);
                        }
                    }
                }
                if (sxList.Count > 0) return (sxList, qxList);
            }
            return (null, null);
        }

        #endregion

        #region CreateGraph  ==================================================
        private bool CreateGraph(GraphX gx, out Graph graph, Item seed = null)
        {
            if (!gx.TryGetGraph(seed, out graph))
                graph = new Graph(gx, seed);

            RefreshGraph(graph);

            return true;
        }
        #endregion

        #region RefreshGraph  =================================================
        internal void RefreshAllGraphs()
        {
            foreach (var gx in Items)
            {
                if (gx.Count > 0)
                {
                    foreach (var g in gx.Items) { RefreshGraph(g); }
                }
            }
        }

        internal bool RefreshGraphX(QueryX qx)
        {
            var qr = qx;
            while (_relation_QueryX_QueryX.TryGetParent(qr, out qx)) { qr = qx; }

            if (_relation_GraphX_QueryX.TryGetParent(qr, out GraphX gx))
                RefreshGraphX(gx);

            return true;
        }


        internal void RebuildGraphX_ARGBList_NodeOwners(GraphX gx)
        {
            gx.Color.Reset();
            gx.NodeOwners.Clear();

            if (_relation_GraphX_ColorColumnX.TryGetChild(gx, out ColumnX cx) && _relation_Store_ColumnX.TryGetParent(cx, out Store tx) && tx.Count > 0)
            {
                var items = tx.GetItems();
                foreach (var item in items)
                {
                    gx.Color.BuildARGBList(cx.Value.GetString(item));
                }
            }

            if (_relation_GraphX_QueryX.TryGetChildren(gx, out IList<QueryX> qxList))
            {
                var workQueue = new Queue<QueryX>(qxList);
                while (workQueue.Count > 0)
                {
                    var qx = workQueue.Dequeue();
                    if (qx.PathParm != null)
                    {
                        gx.Color.BuildARGBList(qx.PathParm.LineColor);
                    }
                    if (_relation_QueryX_QueryX.TryGetChildren(qx, out IList<QueryX> qcList))
                    {
                        foreach (var qc in qcList)
                        {
                            workQueue.Enqueue(qc);
                        }
                    }
                    if (qx.QueryKind == QueryType.Path && qx.IsHead)
                    {
                        var (head, _) = qx.Owner.GetHeadTail(qx);
                        gx.NodeOwners.Add(head);

                        var qt = qx;
                        while (_relation_QueryX_QueryX.TryGetChild(qt, out QueryX qn)) { qt = qn; }

                        var (_, tail) = qt.Owner.GetHeadTail(qt);
                        gx.NodeOwners.Add(tail);
                    }
                }
            }
        }


        private void RefreshGraphX(GraphX gx)
        {
            RebuildGraphX_ARGBList_NodeOwners(gx);

            foreach (var g in gx.Items)
            {
                RefreshGraph(g);
            }
        }

        private void RefreshGraph(Graph g)
        {
            var gx = g.Owner;
            var rt = g.SeedItem;

            g.Reset();
            _queryXRoot.TryGetForest(g, rt, gx.NodeOwners);
            var anyChange = ValidateGraphParms(g);

            TryCreateQueryPaths(g);
            while (TryPathReduction(g)) { }

            AssignNodeColor(g);
            AssignEdgeColor(g);

            if (anyChange) g.CheckLayout();
            g.AdjustGraph();
        }
        #endregion

        #region AssignNodeColor  ==============================================
        private void AssignNodeColor(Graph g)
        {
            var group_Color = new Dictionary<Item, byte>();
            var item_group = new Dictionary<Item, Item>();

            if (_relation_GraphX_ColorColumnX.TryGetChild(g.Owner, out ColumnX cx) && _relation_Store_ColumnX.TryGetParent(cx, out Store tx) && tx.Count > 0)
            {
                var items = tx.GetItems();
                foreach (var gp in items)
                {
                    group_Color[gp] = g.Owner.Color.ColorIndex(cx.Value.GetString(gp));
                }

                foreach (var (q1, q2) in g.GroupQuerys)
                {
                    item_group[q1.Item] = q2.Items[0];
                }

                foreach (var nd in g.Nodes)
                {
                    nd.Color = (item_group.TryGetValue(nd.Item, out Item gp) && group_Color.TryGetValue(gp, out byte index)) ? index : (byte)0;
                }
            }
            else
            {
                foreach (var node in g.Nodes) { node.Color = 0; }
            }
        }
        #endregion

        #region AssignEdgeColor  ==============================================
        private void AssignEdgeColor(Graph g)
        {
            foreach (var eg in g.Edges)
            {
                eg.LineColor = (eg.QueryX.PathParm != null) ? g.Owner.Color.ColorIndex(eg.QueryX.PathParm.LineColor) : (byte)0;
            }
        }
        #endregion

        #region CreateQueryPaths  =============================================
        internal bool TryCreateQueryPaths(Graph g)
        {
            if (g.PathQuerys.Count == 0) return false;
            var L = new Level(g);
            foreach (var (q1, q2) in g.PathQuerys)
            {
                L.Add(new QueryPath(g, q1, q2));
            }
            return (L.Count > 0);
        }
        #endregion

        #region TryPathReduction  =============================================
        internal bool TryPathReduction(Graph G)
        {
            if (!G.TryGetTopLevel(out Level L)) return false;

            var inputPaths = L.Paths.ToArray(); // copy of this levels path list
            var N = inputPaths.Length;
            if (N < 2) return false;

            var nodeIndex = new Dictionary<Item, List<int>>(N * 2); // index into inputPaths 

            var nodeList1 = new List<Item>(N); // list of nodes with 1 connection
            var nodeList2 = new List<Item>(N); // list of nodew with 2 connections

            bool found, found2, found3 = false;

            #region build the nodeIndex dictionary  ===========================
            for (int i = 0; i < N; i++)
            {
                var path = inputPaths[i];


                var head = path.Head;
                if (!nodeIndex.TryGetValue(head, out List<int> indexList)) nodeIndex.Add(head, (indexList = new List<int>(4)));
                indexList.Add(i);

                var tail = path.Tail;
                if (!nodeIndex.TryGetValue(tail, out indexList)) nodeIndex.Add(tail, (indexList = new List<int>(4)));
                indexList.Add(i);
            }
            #endregion

            #region lists of nodes that have exactly 1 or exactly 2 connections
            foreach (var e in nodeIndex)
            {
                if (e.Value.Count == 1)
                {
                    nodeList1.Add(e.Key);

                    var path = inputPaths[e.Value[0]];
                    if (path.Head == e.Key) path.Reverse();
                    path.IsRadial = true;
                }
                else if (e.Value.Count == 2)
                {
                    nodeList2.Add(e.Key);
                }
            }
            #endregion

            #region seed and grow the flaredPath lists  =======================
            var flaredPaths = new List<List<Path>>(nodeList1.Count);

            foreach (var node in nodeList1)
            {
                var i = nodeIndex[node][0];
                var path = inputPaths[i];
                if (path == null) continue;

                List<Path> pathList = null;
                foreach (var k in nodeIndex[path.Head])
                {
                    var tp = inputPaths[k];
                    if (tp == null) continue;
                    if (tp == path) continue;
                    if (!tp.IsRadial) continue;

                    if (pathList == null)
                    {
                        pathList = new List<Path>(4)
                        {
                            path
                        };
                        flaredPaths.Add(pathList);
                        inputPaths[i] = null;
                        found3 = true;
                    }
                    pathList.Add(tp);
                    inputPaths[k] = null;
                }
            }
            #endregion

            #region seed and grow the radialPath lists  =======================
            var radialPaths = new List<List<Path>>(nodeList1.Count);

            foreach (var tail in nodeList1)
            {
                List<Path> pathList = null;

                var i = nodeIndex[tail][0];
                var path = inputPaths[i];
                if (path != null)
                {
                    var head = path.Head;

                    found = true;
                    while (found)
                    {
                        found = false;

                        if (nodeIndex[head].Count == 2)
                        {
                            var j = nodeIndex[head][0];
                            var k = nodeIndex[head][1];
                            var path1 = inputPaths[j];
                            var path2 = inputPaths[k];
                            var path3 = (path1 == null || path1 == path) ? path2 : ((path2 == null || path2 == path) ? path1 : null);

                            if (path3 != null)
                            {
                                if (pathList == null)
                                {
                                    pathList = new List<Path>(8);
                                    radialPaths.Add(pathList);
                                    pathList.Add(path);
                                    inputPaths[i] = null;
                                    found3 = true;
                                }
                                found = true;
                                path = path3;
                                if (path.Tail != head) path.Reverse();
                                pathList.Add(path);
                                inputPaths[j] = null;
                                inputPaths[k] = null;
                                head = path.Head;
                            }
                        }
                    }
                }
            }
            #endregion

            #region seed and grow the innerPath lists  ========================
            // innerPaths flow from nodes with 3 or more connections
            found = true;
            var innerPaths = new List<List<Path>>(nodeList2.Count);
            while (found)
            {
                found = false;
                List<Path> pathList = null;

                #region look for a new seed  ==================================
                foreach (var node in nodeList2)
                {
                    var i = nodeIndex[node][0];
                    var j = nodeIndex[node][1];
                    var path1 = inputPaths[i];
                    var path2 = inputPaths[j];
                    if (path1 == null || path2 == null) continue;

                    var h1 = path1.Head;
                    var t1 = path1.Tail;
                    var h2 = path2.Head;
                    var t2 = path2.Tail;
                    if (t1 == h2 && h1 != t2)
                    {
                        if (nodeIndex[h1].Count > 2)
                        {
                            pathList = new List<Path>(4);
                            innerPaths.Add(pathList);
                            inputPaths[i] = null;
                            inputPaths[j] = null;

                            pathList.Add(path1);
                            pathList.Add(path2);
                            found = found3 = true;
                            break;
                        }
                        else if (nodeIndex[t2].Count > 2)
                        {
                            pathList = new List<Path>(4);
                            innerPaths.Add(pathList);
                            inputPaths[i] = null;
                            inputPaths[j] = null;

                            path2.Reverse();
                            pathList.Add(path2);
                            path1.Reverse();
                            pathList.Add(path1);
                            found = found3 = true;
                            break;
                        }
                    }
                    else if (t1 == t2 && h1 != h2)
                    {
                        if (nodeIndex[h1].Count > 2)
                        {
                            pathList = new List<Path>(4);
                            innerPaths.Add(pathList);
                            inputPaths[i] = null;
                            inputPaths[j] = null;

                            pathList.Add(path1);
                            path2.Reverse();
                            pathList.Add(path2);
                            found = found3 = true;
                            break;
                        }
                        else if (nodeIndex[h2].Count > 2)
                        {
                            pathList = new List<Path>(4);
                            innerPaths.Add(pathList);
                            inputPaths[i] = null;
                            inputPaths[j] = null;

                            pathList.Add(path2);
                            path1.Reverse();
                            pathList.Add(path1);
                            found = found3 = true;
                            break;
                        }
                    }
                    else if (h1 == h2 && t1 != t2)
                    {
                        if (nodeIndex[t1].Count > 2)
                        {
                            pathList = new List<Path>(4);
                            innerPaths.Add(pathList);
                            inputPaths[i] = null;
                            inputPaths[j] = null;

                            path1.Reverse();
                            pathList.Add(path1);
                            pathList.Add(path2);
                            found = found3 = true;
                            break;
                        }
                        else if (nodeIndex[t2].Count > 2)
                        {
                            pathList = new List<Path>(4);
                            innerPaths.Add(pathList);
                            inputPaths[i] = null;
                            inputPaths[j] = null;

                            path2.Reverse();
                            pathList.Add(path2);
                            pathList.Add(path1);
                            found = found3 = true;
                            break;
                        }
                    }
                    else if (h1 == t2 && t1 != h2)
                    {
                        if (nodeIndex[t1].Count > 2)
                        {
                            pathList = new List<Path>(4);
                            innerPaths.Add(pathList);
                            inputPaths[i] = null;
                            inputPaths[j] = null;

                            path1.Reverse();
                            pathList.Add(path1);
                            pathList.Add(path2);
                            found = found3 = true;
                            break;
                        }
                        else if (nodeIndex[h2].Count > 2)
                        {
                            pathList = new List<Path>(4);
                            innerPaths.Add(pathList);
                            inputPaths[i] = null;
                            inputPaths[j] = null;

                            pathList.Add(path2);
                            pathList.Add(path1);
                            found = found3 = true;
                            break;
                        }
                    }
                }
                #endregion

                if (found)
                {
                    found = false;
                    #region grow the innerPath  ===============================
                    found2 = true;
                    while (found2)
                    {
                        found2 = false;
                        var tail = pathList[pathList.Count - 1].Tail;
                        if (nodeIndex[tail].Count > 2) break;

                        foreach (var node in nodeList2)
                        {
                            var i = nodeIndex[node][0];
                            var j = nodeIndex[node][1];
                            var path1 = inputPaths[i];
                            var path2 = inputPaths[j];
                            if (path1 == null)
                            {
                                if (path2 == null) continue;
                                if (path2.Head == tail)
                                {
                                    pathList.Add(path2);
                                    inputPaths[j] = null;
                                    found = found2 = true;
                                    break;
                                }
                                else if (path2.Tail == tail)
                                {
                                    path2.Reverse();
                                    pathList.Add(path2);
                                    inputPaths[j] = null;
                                    found = found2 = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (path2 != null) continue;
                                if (path1.Head == tail)
                                {
                                    pathList.Add(path1);
                                    inputPaths[i] = null;
                                    found = found2 = true;
                                    break;
                                }
                                else if (path1.Tail == tail)
                                {
                                    path1.Reverse();
                                    pathList.Add(path1);
                                    inputPaths[i] = null;
                                    found = found2 = true;
                                    break;
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region find all parallel paths  ==================================
            var parallelPaths = new List<List<Path>>(8);

            foreach (var e in nodeIndex)
            {
                List<Path> pathList = null;
                foreach (var i in e.Value)
                {
                    var path1 = inputPaths[i];
                    if (path1 == null) continue;
                    var h1 = path1.Head;
                    var t1 = path1.Tail;
                    if (e.Key == h1)
                    {
                        foreach (var j in nodeIndex[t1])
                        {
                            var path2 = inputPaths[j];
                            if (path2 == null || path2 == path1) continue;
                            var h2 = path2.Head;
                            var t2 = path2.Tail;
                            if (h1 == h2)
                            {
                                if (t1 != t2) continue;
                                if (pathList == null)
                                {
                                    pathList = new List<Path>(4) { path1 };
                                    inputPaths[i] = null;
                                    parallelPaths.Add(pathList);
                                    found3 = true;
                                }
                                pathList.Add(path2);
                                inputPaths[j] = null;
                            }
                            else if (h1 == t2)
                            {
                                if (t1 != h2) continue;
                                if (pathList == null)
                                {
                                    pathList = new List<Path>(4)
                                    {
                                        path1
                                    };
                                    inputPaths[i] = null;
                                    parallelPaths.Add(pathList);
                                    found3 = true;
                                }
                                path2.Reverse();
                                pathList.Add(path2);
                                inputPaths[j] = null;
                            }
                            else if (t1 == h2)
                            {
                                if (h1 != t2) continue;
                                if (pathList == null)
                                {
                                    pathList = new List<Path>(4)
                                    {
                                        path1
                                    };
                                    inputPaths[i] = null;
                                    parallelPaths.Add(pathList);
                                    found3 = true;
                                }
                                path2.Reverse();
                                pathList.Add(path2);
                                inputPaths[j] = null;
                            }
                            else if (t1 == t2)
                            {
                                if (h1 != h2) continue;
                                if (pathList == null)
                                {
                                    pathList = new List<Path>(4)
                                    {
                                        path1
                                    };
                                    inputPaths[i] = null;
                                    parallelPaths.Add(pathList);
                                    found3 = true;
                                }
                                pathList.Add(path2);
                                inputPaths[j] = null;
                            }
                        }
                    }
                    else
                    {
                        foreach (var j in nodeIndex[h1])
                        {
                            var path2 = inputPaths[j];
                            if (path2 == null || path2 == path1) continue;
                            var h2 = path2.Head;
                            var t2 = path2.Tail;
                            if (h1 == h2)
                            {
                                if (t1 != t2) continue;
                                if (pathList == null)
                                {
                                    pathList = new List<Path>(4)
                                    {
                                        path1
                                    };
                                    inputPaths[i] = null;
                                    parallelPaths.Add(pathList);
                                    found3 = true;
                                }
                                pathList.Add(path2);
                                inputPaths[j] = null;
                            }
                            else if (h1 == t2)
                            {
                                if (t1 != h2) continue;
                                if (pathList == null)
                                {
                                    pathList = new List<Path>(4)
                                    {
                                        path1
                                    };
                                    inputPaths[i] = null;
                                    parallelPaths.Add(pathList);
                                    found3 = true;
                                }
                                path2.Reverse();
                                pathList.Add(path2);
                                inputPaths[j] = null;
                            }
                            else if (t1 == h2)
                            {
                                if (h1 != t2) continue;
                                if (pathList == null)
                                {
                                    pathList = new List<Path>(4)
                                    {
                                        path1
                                    };
                                    inputPaths[i] = null;
                                    parallelPaths.Add(pathList);
                                    found3 = true;
                                }
                                path2.Reverse();
                                pathList.Add(path2);
                                inputPaths[j] = null;
                            }
                            else if (t1 == t2)
                            {
                                if (h1 != h2) continue;
                                if (pathList == null)
                                {
                                    pathList = new List<Path>(4)
                                    {
                                        path1
                                    };
                                    inputPaths[i] = null;
                                    parallelPaths.Add(pathList);
                                    found3 = true;
                                }
                                pathList.Add(path2);
                                inputPaths[j] = null;
                            }
                        }
                    }
                }
            }
            #endregion

            if (!found3) return false;

            #region create and populate next path level  ======================
            L = new Level(G);

            foreach (var path in inputPaths)
            {
                if (path == null) continue;
                L.Add(path);
            }

            foreach (var list in radialPaths)
            {
                list.Reverse();
                if (list.Count > 1)
                    L.Add(new SeriesPath(G, list.ToArray(), true));
                else if (list.Count == 1 && list[0] != null)
                    L.Add(list[0]);

            }

            foreach (var list in innerPaths)
            {
                if (list.Count > 1)
                    L.Add(new SeriesPath(G, list.ToArray()));
                else if (list.Count == 1 && list[0] != null)
                    L.Add(list[0]);
            }

            foreach (var list in parallelPaths)
            {
                if (list.Count < 2) continue;
                L.Add(new ParallelPath(G, list.ToArray(), list[0].IsRadial));
            }

            foreach (var list in flaredPaths)
            {
                if (list.Count < 2) continue;
                L.Add(new FlaredPath(G, list.ToArray()));
            }
            #endregion

            return true;
        }
        #endregion

        #endregion
    }
}
