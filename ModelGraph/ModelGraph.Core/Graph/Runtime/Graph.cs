using System.Collections.Generic;

namespace ModelGraph.Core
{
    public partial class Graph : ChildOf<GraphX>
    {
        internal Item SeedItem;    // seed of query forest 
        internal Query[] Forest;   // roots of the query forest

        internal List<Node> Nodes = new List<Node>();
        internal List<Edge> Edges = new List<Edge>();
        internal List<Path> Paths = new List<Path>();
        internal List<Level> Levels = new List<Level>();
        internal List<(Query, Query)> PathQuerys = new List<(Query, Query)>();  // completed path query sequences
        internal List<(Query, Query)> OpenQuerys = new List<(Query, Query)>();  // incomplete path query sequences
        internal List<(Query, Query)> GroupQuerys = new List<(Query, Query)>(); // completed group query sequences
        internal List<(Query, Query)> SegueQuerys = new List<(Query, Query)>(); // completed segue query sequences

        internal HashSet<Item> NodeItems = new HashSet<Item>(); // hash of Node.Item for all nodes
        internal Dictionary<Node, List<Edge>> Node_Edges = new Dictionary<Node, List<Edge>>(); // list of edges for each node
        internal Dictionary<Item, Node> Item_Node = new Dictionary<Item, Node>();              // look up item -> node

        internal List<(byte A, byte R, byte G, byte B)> ARGBList => Owner.ARGBList;
        internal readonly Selector Selector;

        internal Extent Extent;  // current x,y extent of this graph

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.Graph;
        public override string GetNameId() => $"{Owner.GetNameId()}  {GetSeedItemName()}";
        private string GetSeedItemName() => (SeedItem is null) ? string.Empty : SeedItem.GetNameId();
        #endregion

        #region Constructor  ==================================================
        internal Graph(GraphX owner, Item seedItem = null)
        {
            Owner = owner;
            SeedItem = seedItem;
            Selector = new Selector(this);
            owner.Add(this);
        }
        public GraphX GraphX => Owner;
        #endregion

        #region Properties/Methods  ===========================================
        internal (int count, Edge[] edges) ConnectedEdges(Node n) => Node_Edges.TryGetValue(n, out List<Edge> list) ? (list.Count, list.ToArray()) : (0, null);

        public void Add(Path path) { Paths.Add(path); }
        public void Add(Level level) { Levels.Add(level); }

        public int QueryCount => (Forest == null) ? 0 : Forest.Length;
        public int OpenPathCount(int index)
        {
            if (index < 0 || index > OpenQuerys.Count) return 0;
            var head = OpenQuerys[index].Item1.Item;
            var count = 1;
            for (var i = index + 1; i < OpenQuerys.Count; i++)
            {
                if (OpenQuerys[index].Item1.Item != head) return count;
                count += 1;
            }
            return count;
        }
        public int Count => Levels.Count;
        private int Last => Count - 1;

        public bool TryGetTopLevel(out Level lvl) { lvl = null; if (Count == 0) return false; lvl = Levels[Last] as Level; return true; }

        public int NodeCount { get { return (Nodes == null) ? 0 : Nodes.Count; } }
        public int EdgeCount { get { return (Edges == null) ? 0 : Edges.Count; } }

        internal Attach GetAttach(Node node)
        {
            var n = node.Symbol - 2;
            if (n < 0 || n > Owner.Symbols.Count) return Attach.Normal;

            return Owner.Symbols[n].Attach;
        }

        public void Reset()
        {
            Forest = null;
    
            Nodes.Clear();
            Edges.Clear();
            Paths.Clear();
            Levels.Clear();
            NodeItems.Clear();
            PathQuerys.Clear();
            OpenQuerys.Clear();
            GroupQuerys.Clear();
            SegueQuerys.Clear();

            NodeItems.Clear();
            Item_Node.Clear();
            Node_Edges.Clear();
        }
        #endregion

        #region Helpers  ======================================================
        internal Extent ResetExtent()
        {
            Extent = new Extent();
            Extent = Extent.SetExtent(Nodes, 16);
            return Extent;
        }
        #endregion
    }
}