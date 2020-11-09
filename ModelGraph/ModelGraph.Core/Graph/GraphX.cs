using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class GraphX : ChildOfStoreOf<GraphXManager, Graph>
    {
        internal int SymbolCount => Symbols is null ? 0 : Symbols.Count;
        internal Color Color = new Color();
        internal IList<SymbolX> Symbols = null;
        internal Dictionary<Store, List<(QueryX, byte)>> NodeStore_QuerySymbol = new Dictionary<Store, List<(QueryX, byte)>>(); //used to build node query symbol index
        internal Dictionary<Store, Property> NodeStore_Color = new Dictionary<Store, Property>();
        internal Dictionary<Store, List<Property>> NodeStore_ToolTip = new Dictionary<Store, List<Property>>();

        internal Dictionary<Item, Dictionary<QueryX, List<NodeEdge>>> Root_QueryX_Parms = new Dictionary<Item, Dictionary<QueryX, List<NodeEdge>>>(10);
        internal override string Name { get => _name; set => _name = value; }
        private string _name;
        internal override string Summary { get => _summary; set => _summary = value; }
        private string _summary;
        internal override string Description { get => _description; set => _description = value; }
        private string _description;

        internal byte SymbolSize = 48;
        internal byte ThinBusSize = 6;
        internal byte WideBusSize = 10;
        internal byte ExtraBusSize = 20;
        internal byte MinNodeSize = 4;
        internal byte TerminalLength = 10;
        internal byte TerminalSpacing = 8;
        internal byte SurfaceSkew = 4;
        internal byte TerminalSkew = 2;

        public List<(byte A, byte R, byte G, byte B)> ARGBList => Color.ARGBList;

        #region Constructors  =================================================
        internal GraphX(GraphXManager owner, bool autoExpand = false)
        {
            Owner = owner;
            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        internal override Item GetOwner() => Owner;
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.GraphX;
        public override string GetNameId() => string.IsNullOrWhiteSpace(Name) ? BlankName : Name;
        public override string GetSummaryId() => Summary;
        public override string GetDescriptionId() => Description;
        #endregion

        #region Properties/Methods  ===========================================
        internal Graph CreateGraph(Item seed = null)
        {
            foreach (var gt in Items) 
            { 
                if (gt.SeedItem == seed) return RefreshedGraph(gt); 
            }
            return RefreshedGraph(new Graph(this, seed));
                        
            Graph RefreshedGraph(Graph g)
            {
                Owner.RefreshGraph(g);
                return g;
            }
        }

        public float SymbolScale => SymbolSize;
        internal bool TryGetGraph(Item root, out Graph graph)
        {
            graph = null;
            return false;
        }
        internal void Refresh() { }
        #endregion
    }
}
