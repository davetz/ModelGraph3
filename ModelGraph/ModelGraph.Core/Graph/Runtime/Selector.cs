using System;
using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class Selector
    {
        internal Graph Graph;   // reference the graphs Node and Edge lists

        internal Node HitNode;
        internal Node RefNode;  //saved node reference
        internal Node PrevNode;

        internal Edge HitEdge;
        internal Edge RefEdge;  //saved edge reference

        internal int HitBend;  // index of bend point (relative to edge.bends)
        internal int HitIndex; // index of start of the hit segment (relative to edge.point)

        internal Vector2 HitPoint;  //refined hit point location
        internal Vector2 RefPoint;  //saved point reference

        internal HitLocation HitLocation;  //hit location details
        internal HitLocation RefLocation;  //saved hit reference

        internal Edge BendEdge;     // when bending an edge, remember which edge it is
        internal int BendIndex;     // when moving an bend point, remember which point it is

        internal HashSet<Node> Nodes = new HashSet<Node>();   // interior nodes
        internal HashSet<Edge> Edges = new HashSet<Edge>();   // interior edges

        internal Dictionary<Edge, (int I1, int I2)> Points = new Dictionary<Edge, (int I1, int I2)>(); // chopped edge interior points

        internal Extent Extent = new Extent(); // selector rectangle
        private bool _enableSnapshot = true;

        #region Constructor  ==================================================
        internal Selector(Graph graph)
        {
            Graph = graph;
        }
        #endregion

        #region Properties  ===================================================
        internal bool IsVoidHit => (HitLocation == HitLocation.Void);
        internal bool IsNodeHit => ((HitLocation & HitLocation.Node) != 0);
        internal bool IsEdgeHit => ((HitLocation & HitLocation.Edge) != 0);

        internal bool IsChanged => (RefNode != HitNode) || (RefEdge != HitEdge) || (RefLocation != HitLocation);

        internal bool IsTopHit => ((HitLocation & HitLocation.Top) != 0);
        internal bool IsLeftHit => ((HitLocation & HitLocation.Left) != 0);
        internal bool IsRightHit => ((HitLocation & HitLocation.Right) != 0);
        internal bool IsBottomHit => ((HitLocation & HitLocation.Bottom) != 0);
        internal bool IsCenterHit => ((HitLocation & HitLocation.Center) != 0);
        internal bool IsSideHit => ((HitLocation & HitLocation.SideOf) != 0);

        internal bool IsEnd1Hit => ((HitLocation & HitLocation.End1) != 0);
        internal bool IsEnd2Hit => ((HitLocation & HitLocation.End2) != 0);
        internal bool IsBendHit => ((HitLocation & HitLocation.Bend) != 0);
        internal ResizerType Resizer => GetResizer();
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

        #region HitTestPoint  =================================================
        internal void HitTestPoint(Vector2 p)
        {
            PrevNode = HitNode;

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
            if (Graph.HitTestMap.HitTest(p, out List<IHitTestable> targets))
            {
                foreach (var item in targets)
                {
                    if (item is Node n && n.HitTest(p))
                    {
                        var (hit, pnt) = n.RefinedHit(p);
                        HitLocation |= hit;
                        HitPoint = pnt;

                        HitNode = n;
                        return;  // we are done;
                    }
                    else if (item is Edge e && e.HitTest(p) && e.HitTest(p, ref HitLocation, ref HitBend, ref HitIndex, ref HitPoint))
                    {
                        HitEdge = e;
                        return;  // we are done
                    }
                }
            }
        }
        #endregion

        #region HitTestRegion  ================================================
        internal void HitTestRegion(bool toggleMode, Vector2 p1, Vector2 p2)
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

        #region SaveHitReference  =============================================
        internal void SaveHitReference()
        {
            RefNode = HitNode;
            RefEdge = HitEdge;
            RefPoint = HitPoint;
            RefLocation = HitLocation;
            EnableSnapshot();
        }
        #endregion

        #region Clear  ========================================================
            internal void Clear()
        {
            Nodes.Clear();
            Edges.Clear();
            Points.Clear();
            RefNode = null;
            RefEdge = null;
        }
        #endregion

        #region Resize  =======================================================
        internal void Resize(Vector2 delta, ResizerType resizer)
        {
            if (resizer == ResizerType.None) return;
            HitNode.Resize(delta, resizer);
        }
        #endregion

        #region Move  =========================================================
        internal void Move(Vector2 delta)
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
        internal void GravityInside()
        {
           if (IsNodeHit)
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
        internal void GravityDisperse()
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
        static readonly float Radians45Degree = (float)(Math.PI / 4);
        static readonly float Radians90Degree = (float)(Math.PI / 2);

        internal void RotateLeft45() =>  Rotate(Matrix3x2.CreateRotation(-Radians45Degree, HitPoint));
        internal void RotateRight45() => Rotate(Matrix3x2.CreateRotation(Radians45Degree, HitPoint));
        internal void RotateLeft90() => Rotate(Matrix3x2.CreateRotation(-Radians90Degree, HitPoint));
        internal void RotateRight90() => Rotate(Matrix3x2.CreateRotation(Radians90Degree, HitPoint));
        internal void Rotate(Matrix3x2 mx)
        {
            if (Nodes.Count < 2) return; // this is nonsense

            TakeSnapshot();
            foreach (var node in Nodes) { node.Center = Vector2.Transform(node.Center, mx); }
            foreach (var edge in Edges)
            {
                if (edge.HasBends)
                {
                    for (int i = 0; i < edge.Bends.Length; i++)
                    {
                        edge.Bends[i] = Vector2.Transform(edge.Bends[i], mx);
                    }
                }
            }
            UpdateModels();
        }
        #endregion

        #region Align  ========================================================
        internal void AlignVert()
        {
            if (RefNode is null || Nodes.Count < 2) return; // this is nonsense

            var x = RefNode.X;
            TakeSnapshot();
            foreach (var node in Nodes)
            {
                node.AlignVert(x);
            }
            UpdateModels();
        }
        internal void AlignHorz()
        {
            if (RefNode is null || Nodes.Count < 2) return; // this is nonsense

            var y = RefNode.Y;
            TakeSnapshot();
            foreach (var node in Nodes)
            {
                node.AlignHorz(y);
            }
            UpdateModels();
        }
        #endregion

        #region Flip  =========================================================
        internal void FlipHorz()
        {
            if (RefNode is null || Nodes.Count < 2) return; // this is nonsense

            var x = RefNode.X;
            TakeSnapshot();
            foreach (var node in Nodes)
            {
                node.FlipHorz(x);
            }
            UpdateModels();
        }
        internal void FlipVert()
        {
            if (RefNode is null || Nodes.Count < 2) return; // this is nonsense

            var y = RefNode.Y;
            TakeSnapshot();
            foreach (var node in Nodes)
            {
                node.FlipVert(y);
            }
            UpdateModels();
        }
        #endregion

        #region UpdateModels  =================================================
        internal void UpdateModels()
        {
            var root = Graph.Owner.Owner.Owner;
            Graph.AdjustGraph(this);
            foreach (var pm in root.Items)
            {
                if (pm.LeadModel is GraphModel gm && gm.Graph == Graph) gm.Refresh();
            }
            EnableSnapshot();
        }

        #endregion

        #region Snapshot  =====================================================
        internal void EnableSnapshot() => _enableSnapshot = true;

        internal void TakeSnapshot()
        {
            if (_enableSnapshot)
            {
                Graph.ModelDelta++;
                _enableSnapshot = false;
                Graph.TakeSnapshot(this);
            }
        }
        #endregion
    }
}
