using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public class Selector
    {
        public Graph Graph;   // reference the graphs Node and Edge lists

        public Node PrevNode;
        public Edge PrevEdge;
        public HitLocation PrevLocation;

        public Node HitNode;
        public Edge HitEdge;
        public int HitBend;  // index of bend point (relative to edge.bends)
        public int HitIndex; // index of start of the hit segment (relative to edge.point)
        public Extent HitRegion;
        public Vector2 HitPoint; // the refined hit point location
        public HitLocation HitLocation; // hit location details

        public Edge BendEdge;     // when bending an edge, remember which edge it is
        public int BendIndex;     // when moving an bend point, remember which point it is

        public HashSet<Node> Nodes = new HashSet<Node>();   // interior nodes
        public HashSet<Edge> Edges = new HashSet<Edge>();   // interior edges
        public Dictionary<Edge, (int I1, int I2)> Points = new Dictionary<Edge, (int I1, int I2)>(); // chopped edge interior points

        public Extent Extent = new Extent(); // selector rectangle

        private bool _enableSnapshot = true;

        #region Constructor  ==================================================
        public Selector(Graph graph)
        {
            Graph = graph;
        }
        #endregion

        #region Properties  ===================================================
        public bool IsVoidHit => (HitLocation == HitLocation.Void);
        public bool IsNodeHit => ((HitLocation & HitLocation.Node) != 0);
        public bool IsEdgeHit => ((HitLocation & HitLocation.Edge) != 0);
        public bool IsRegionHit => ((HitLocation & HitLocation.Region) != 0);

        public bool IsChanged => (PrevNode != HitNode) || (PrevEdge != HitEdge) || (PrevLocation != HitLocation);

        public bool IsTopHit => ((HitLocation & HitLocation.Top) != 0);
        public bool IsLeftHit => ((HitLocation & HitLocation.Left) != 0);
        public bool IsRightHit => ((HitLocation & HitLocation.Right) != 0);
        public bool IsBottomHit => ((HitLocation & HitLocation.Bottom) != 0);
        public bool IsCenterHit => ((HitLocation & HitLocation.Center) != 0);
        public bool IsSideHit => ((HitLocation & HitLocation.SideOf) != 0);

        public bool IsEnd1Hit => ((HitLocation & HitLocation.End1) != 0);
        public bool IsEnd2Hit => ((HitLocation & HitLocation.End2) != 0);
        public bool IsBendHit => ((HitLocation & HitLocation.Bend) != 0);
        public ResizerType Resizer => GetResizer();
        ResizerType GetResizer()
        {
            if (IsNodeHit && HitNode.IsGraphNode && HitNode.Sizing == Sizing.Manual)
            {
                if (IsTopHit && (HitNode.Aspect == Aspect.Square || HitNode.Aspect == Aspect.Vertical)) return ResizerType.Top;
                if (IsLeftHit && (HitNode.Aspect == Aspect.Square || HitNode.Aspect == Aspect.Horizontal)) return ResizerType.Left;
                if (IsRightHit && (HitNode.Aspect == Aspect.Square || HitNode.Aspect == Aspect.Horizontal)) return ResizerType.Right;
                if (IsBottomHit && (HitNode.Aspect == Aspect.Square || HitNode.Aspect == Aspect.Vertical)) return ResizerType.Bottom;
            }
            return ResizerType.None;
        }
        #endregion

        #region SelectorRectangle  ============================================
        public void SetPoint1(Vector2 p) => Extent.Point1 = Extent.Point2 = p;
        public void SetPoint2(Vector2 p) => Extent.Point2 = p;
        #endregion

        #region TryAdd  =======================================================
        public void HitTestRegion(bool toggleMode, Vector2 p1, Vector2 p2)
        {
            Extent.Normalize(p1, p2);
            
            if (Extent.HasArea)
            {
                var count = Nodes.Count;
                if (toggleMode)
                {
                    foreach (var node in Graph.Nodes)
                    {
                        if (Extent.Contains(node.Center))
                        {
                            if (Nodes.Contains(node))
                                Nodes.Remove(node);
                            else
                                Nodes.Add(node);
                        }
                    }
                }
                else
                {
                    foreach (var node in Graph.Nodes)
                    {
                        if (Nodes.Contains(node)) continue;
                        if (Extent.Contains(node.Center)) Nodes.Add(node);
                    }
                }
                if (count != Nodes.Count)
                {
                    Edges.Clear();

                    var badBend = new HashSet<Edge>();
                    foreach (var e in Points)
                    {
                        var edge = e.Key;
                        if (Nodes.Contains(edge.Node1) && Nodes.Contains(edge.Node2)) badBend.Add(edge);
                        if (!Nodes.Contains(edge.Node1) && !Nodes.Contains(edge.Node2)) badBend.Add(edge);
                    }
                    foreach (var edge in badBend) { Points.Remove(edge); }

                    foreach (var edge in Graph.Edges)
                    {
                        if (Nodes.Contains(edge.Node1) && Nodes.Contains(edge.Node2))
                        {
                            Edges.Add(edge);
                        }
                        else if (edge.HasBends)
                        {
                            if (Nodes.Contains(edge.Node1) || Nodes.Contains(edge.Node2))
                            {
                                var j = 0;
                                var k = 0;
                                for (int i = edge.Tm1 + 1; i < edge.Tm2; i++)
                                {
                                    if (Extent.Contains(edge.Points[i]))
                                    {
                                        if (j == 0) j = i;
                                    }
                                    else
                                    {
                                        if (k == 0) k = i;
                                        break;
                                    }
                                }
                                if (j > 0 && k > 0) Points.Add(edge, (j, k));                                
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region HitTest  ======================================================
        public void HitTest(Vector2 p)
        {
            PrevNode = HitNode;
            PrevEdge = HitEdge;
            PrevLocation = HitLocation;

            // clear previous results
            HitNode = null;
            HitEdge = null;
            HitBend = -1;
            HitIndex = -1;
            HitPoint = p;
            HitLocation = HitLocation.Void;

            // test prev node
            if (PrevNode != null && PrevNode.HitTest(p))
            {
                var (hit, pnt) = PrevNode.RefinedHit(p);
                HitLocation |= hit;
                HitPoint = pnt;

                HitNode = PrevNode;
                return;  // we're done;
            }

            foreach (var node in Graph.Nodes)
            {
                // eliminate unqualified nodes
                if (!node.HitTest(p)) continue;

                // now refine the hit test results
                // node.RefineHitTest(p, ref HitLocation, ref HitPoint);
                var (hit, pnt) = node.RefinedHit(p);
                HitLocation |= hit;
                HitPoint = pnt;

                HitNode = node;
                return;  // we are done;
            }

            foreach (var edge in Graph.Edges)
            {
                // eliminate unqualified edges
                if (!edge.HitTest(p)) continue;

                // now refine the hit test results
                if (!edge.HitTest(p, ref HitLocation, ref HitBend, ref HitIndex, ref HitPoint)) continue;

                HitEdge = edge;
                return;  // we are done
            }
        }
        #endregion

        #region Clear  ========================================================
        public void Clear()
        {
            Nodes.Clear();
            Edges.Clear();
            Points.Clear();
        }
        #endregion

        #region Resize  =======================================================
        public void Resize(Vector2 delta, ResizerType resizer)
        {
            if (resizer == ResizerType.None) return;
            HitNode.Resize(delta, resizer);
        }
        #endregion

        #region Move  =========================================================
        public void Move(Vector2 delta)
        {
            if (IsNodeHit)
            {
                TakeSnapshot();

                if (Nodes.Contains(HitNode))
                {
                    foreach (var node in Nodes) { node.Move(delta); }
                    foreach (var edge in Edges) { edge.Move(delta); }
                }
                else if (IsNodeHit)
                    HitNode.Move(delta);
                Graph.AdjustGraph(this);
            }
        }
        #endregion

        #region Gravity  ======================================================
        public void GravityInside()
        {
           if (IsNodeHit && IsRegionHit)
           {
                TakeSnapshot();

                var tmLen = Graph.Owner.TerminalLength + 4;
                var nodes = new List<Node>(Nodes);
                if (IsVertical())
                {
                    nodes.Sort((a, b) => a.Y < b.Y ? -1 : a.Y > b.Y ? 1 : 0);

                    var I = nodes.IndexOf(HitNode);
                    for (int i = I, j = I - 1; i > 0; i--, j--)
                    {
                        var n1 = nodes[i];
                        var n2 = nodes[j];
                        var d = n1.DY + n2.DY;
                        if (n1.IsGraphSymbol) d += tmLen;
                        if (n2.IsGraphSymbol) d += tmLen;
                        n2.Y = n1.Y - d;
                        n2.X = n1.X;
                    }
                    for (int i = I, j = I + 1; j < nodes.Count; i++, j++)
                    {
                        var n1 = nodes[i];
                        var n2 = nodes[j];
                        var d = n1.DY + n2.DY;
                        if (n1.IsGraphSymbol) d += tmLen;
                        if (n2.IsGraphSymbol) d += tmLen;
                        n2.Y = n1.Y + d;
                        n2.X = n1.X;
                    }
                }
                else
                {
                    nodes.Sort((a, b) => a.X < b.X ? -1 : a.X > b.X ? 1 : 0);

                    var I = nodes.IndexOf(HitNode);
                    for (int i = I, j = I - 1; i > 0; i--, j--)
                    {
                        var n1 = nodes[i];
                        var n2 = nodes[j];
                        var d = n1.DX + n2.DX;
                        if (n1.IsGraphSymbol) d += tmLen;
                        if (n2.IsGraphSymbol) d += tmLen;
                        n2.X = n1.X - d;
                        n2.Y = n1.Y;
                    }
                    for (int i = I, j = I + 1; j < nodes.Count; i++, j++)
                    {
                        var n1 = nodes[i];
                        var n2 = nodes[j];
                        var d = n1.DX + n2.DX;
                        if (n1.IsGraphSymbol) d += tmLen;
                        if (n2.IsGraphSymbol) d += tmLen;
                        n2.X = n1.X + d;
                        n2.Y = n1.Y;
                    }
                }
            }
        }
        public void GravityDisperse()
        {
        }

        #region IsForward  ====================================================
        bool IsForward(Node n1, Node n2, (int x, int y) d)
        {
            if (d.y == 0)
            {
                var dx = n2.X - n1.X;
                return dx > 0;
            }
            else
            {
                var dy = n2.Y - n1.Y;
                return dy > 0;
            }
        }
        #endregion

        #region IsVertical  ===================================================
        private bool IsVertical()
        {
            var e = new Extent();
            foreach (var node in Nodes)
            {
                e.Expand(node.Center);
            }
            return e.DY > e.DX;
        }
        #endregion
        #endregion

        #region Rotate  =======================================================
        public void RotateLeft45()
        {
            Rotate(XYTuple.RotateLeft45Matrix(HitPoint));
        }
        public void RotateRight45()
        {
            Rotate(XYTuple.RotateRight45Matrix(HitPoint));
        }
        public void RotateLeft90()
        {
            Rotate(XYTuple.RotateLeft90Matrix(HitPoint));
        }
        public void RotateRight90()
        {
            Rotate(XYTuple.RotateRight90Matrix(HitPoint));
        }
        public void Rotate(Matrix3x2 mx)
        {
            TakeSnapshot();
            if (IsRegionHit)
            {
                foreach (var node in Nodes) { node.Center = XYTuple.Transform(mx, node.Center); }
                foreach (var edge in Edges)
                {
                    if (edge.HasBends)
                    {
                        for (int i = 0; i < edge.Bends.Length; i++)
                        {
                            edge.Bends[i] = XYTuple.Transform(mx, edge.Bends[i]);
                        }
                    }
                }
            }
            UpdateModels();
        }
        #endregion

        #region Align  ========================================================
        public void AlignVert()
        {
            var x = HitPoint.X;
            TakeSnapshot();
            foreach (var node in Nodes)
            {
                node.AlignVert(x);
            }
            UpdateModels();
        }
        public void AlignHorz()
        {
            var y = HitPoint.Y;
            TakeSnapshot();
            foreach (var node in Nodes)
            {
                node.AlignHorz(y);
            }
            UpdateModels();
        }
        #endregion

        #region Flip  =========================================================
        public void FlipHorz()
        {
            var x = HitPoint.X;
            TakeSnapshot();
            foreach (var node in Nodes)
            {
                node.FlipHorz(x);
            }
            UpdateModels();
        }
        public void FlipVert()
        {
            var y = HitPoint.Y;
            TakeSnapshot();
            foreach (var node in Nodes)
            {
                node.FlipVert(y);
            }
            UpdateModels();
        }
        #endregion

        #region UpdateModels  =================================================
        private void UpdateModels()
        {
            var root = Graph.Owner.Owner.Owner;
            foreach (var pm in root.Items)
            {
                if (pm.LeadModel is GraphModel gm && gm.Graph == Graph) gm.RefreshDrawData();
            }
        }

        #endregion

        #region Snapshot  =====================================================
        public void EnableSnapshot() => _enableSnapshot = true;

        public void TakeSnapshot()
        {
            if (_enableSnapshot)
            {
                _enableSnapshot = false;
                Graph.TakeSnapshot(this);
            }
        }
        #endregion
    }
}
