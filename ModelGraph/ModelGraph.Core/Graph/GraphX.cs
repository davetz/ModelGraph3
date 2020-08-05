using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class GraphX : ChildOfStoreOf<GraphXRoot, Graph>
    {
        internal Color Color = new Color();
        internal HashSet<Store> NodeOwners = new HashSet<Store>();
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
        internal GraphX(GraphXRoot owner, bool autoExpand = false)
        {
            Owner = owner;
            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        internal GraphXRoot Owner;
        internal override Item GetOwner() => Owner;
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.GraphX;
        public override string GetNameId() => string.IsNullOrWhiteSpace(Name) ? BlankName : Name;
        public override string GetParentId() => GetKindId();
        public override string GetSummaryId() => Summary;
        public override string GetDescriptionId() => Description;
        #endregion

        #region Properties/Methods  ===========================================
        public float SymbolScale => SymbolSize;
        internal bool TryGetGraph(Item root, out Graph graph)
        {
            if (Count > 0)
            {
                foreach (var item in Items)
                {
                    var g = item as Graph;
                    if (g.SeedItem != root) continue;
                    graph = g;
                    return true;
                }
            }
            graph = null;
            return false;
        } 
        #endregion
    }
}
