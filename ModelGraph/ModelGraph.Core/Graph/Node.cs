using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public class Node : NodeEdge, IHitTestable
    {
        internal Item Item;
        internal int OpenPathIndex = -1;

        internal float X;
        internal float Y;
        internal byte DX;
        internal byte DY;
        internal byte Color;
        internal byte Symbol;
        internal Labeling Labeling;
        internal Sizing Sizing;
        internal BarWidth BarWidth;
        internal FlipState FlipState;
        internal Aspect Aspect;

        #region Ìdentity  =====================================================
        internal override IdKey IdKey => IdKey.Node;
        public override string GetNameId() => Item.GetNameId();
        public override string GetSummaryId() => Item.GetSummaryId();
        #endregion

        #region Snapshot  =====================================================
        internal (float X, float Y, byte DX, byte DY, byte Color, byte Symbol, Labeling Labeling, Sizing Resizing, BarWidth BarWidth, FlipState FlipRotate, Aspect Orientation)
            Snapshot
        {
            get { return (X, Y, DX, DY, Color, Symbol, Labeling, Sizing, BarWidth, FlipState, Aspect); }
            set
            {
                X = value.X;
                Y = value.Y;
                DX = value.DX;
                DY = value.DY;
                Color = value.Color;
                Symbol = value.Symbol;
                Labeling = value.Labeling;
                Sizing = value.Resizing;
                BarWidth = value.BarWidth;
                FlipState = value.FlipRotate;
                Aspect = value.Orientation;
            }
        }
        #endregion

        #region Constructor  ==================================================
        internal Node()
        {
            DX = DY = (byte)GraphDefault.MinNodeSize;
        }
        internal Graph Owner;
        internal override Item GetOwner() => Owner;
        #endregion

        #region Booleans  =====================================================
        public bool IsGraphPoint => Aspect == Aspect.Point;

        public bool IsGraphNode => Symbol == 0;
        public bool IsGraphEgress => Symbol == 1;
        public bool IsGraphSymbol => Symbol > 1;


        public bool IsAutoResizing => Sizing == Sizing.Auto;

        public bool IsMasked { get { return (IsGraphNode && Aspect != Aspect.Square && Sizing == Sizing.Manual); } }

        public bool IsNodePoint => IsGraphNode && Aspect == Aspect.Point; 
        public bool IsAutoSizing { get { return (Sizing == Sizing.Auto && Aspect != Aspect.Point); } }
        public bool IsFixedSizing { get { return (Sizing == Sizing.Fixed && Aspect != Aspect.Point); } }
        public bool IsManualSizing { get { return (IsGraphNode && Sizing == Sizing.Manual && Aspect != Aspect.Point); } }
        #endregion

        #region Center, Extent, Radius  =======================================
        public Vector2 Center { get { return new Vector2(X, Y); } set { X = value.X; Y = value.Y; } }
        public Vector2 Radius => new Vector2(DX, DY);
        public (float X1, float Y1, float X2, float Y2) Extent => (X - DX, Y - DY, X + DX, Y + DY);
        #endregion

        #region IHitTestable  =================================================
        public void GetHitSegments(HashSet<(int,int)> hitSegments, uint mask, ushort size, byte margin)
        {
            var x = (int)X;
            var y = (int)Y;
            int mx = DX + margin;
            int my = DY + margin;
            var x1 = (int)((x - mx) & mask);
            var x2 = (int)((x + mx) & mask);
            var y1 = (int)((y - my) & mask);
            var y2 = (int)((y + my) & mask);
            hitSegments.Add((x1, y1));
            hitSegments.Add((x1, y2));
            hitSegments.Add((x2, y1));
            hitSegments.Add((x2, y2));
        }
        #endregion

        #region Tryinitialize  ================================================
        static byte _min = (byte)GraphDefault.MinNodeSize;
        static byte _max = byte.MaxValue;

        internal bool TryInitialize(int cp)
        {
            if (DX == 0 || DY == 0) { DX = DY = _min; }
            if (X == 0 || Y == 0) { X = Y = cp; return true; }

            return false;
        }
        #endregion

        #region SetSize, GetValues  ===========================================
        internal void SetSize(float x, float y)
        {
            DX = (byte)((x < _min) ? _min : ((x > _max) ? _max : x));
            DY = (byte)((y < _min) ? _min : ((y > _max) ? _max : y));
        }
        internal (float x, float y, float w, float h) Values()
        {
            return IsGraphPoint ? (X, Y, 1, 1) : (X, Y, DX, DY);
        }
        internal int[] CenterXY
        {
            get { return new int[] { (int)X, (int)Y }; }
            set { if (value != null && value.Length == 2) { X = value[0]; Y = value[1]; UpdateModels(); } }
        }

        internal int[] SizeWH
        {
            get { return new int[] { DX, DY }; }
            set { if (value != null && value.Length == 2) { DX = ValidSize(value[0]); DY = ValidSize(value[1]); UpdateModels(); } }
        }
        private byte ValidSize(int val)
        {
            if (val < _min) return _min;
            if (val > _max) return _max;
            return (byte)val;
        }
        #endregion

        #region Flip, Align  ==================================================
        internal void AlignTop(float y) { Y = y + DY; }

        internal void AlignLeft(float x) { X = x + DX; }

        internal void AlignRight(float x) { X = x - DX; }

        internal void AlignBottom(float y) { Y = y - DY; }

        internal void AlignVert(float x) { X = x; }

        internal void AlignHorz(float y) { Y = y; }

        internal void FlipVert(float y) { Y = y + (y - Y); }

        internal void FlipHorz(float x) { X = x + (x - X); }
        #endregion

        #region Move, Resize  =================================================
        internal void Resize(Vector2 delta, ResizerType resizer)
        {
            var x1 = X - DX;
            var x2 = X + DX;
            var y1 = Y - DY;
            var y2 = Y + DY;
          
            switch (resizer)
            {
                case ResizerType.None:
                    return;
                case ResizerType.Top:
                    y1 += delta.Y;
                    TryResize (); 
                    break;
                case ResizerType.Left:
                    x1 += delta.X;
                    TryResize();
                    break;
                case ResizerType.Right:
                    x2 += delta.X;
                    TryResize();
                    break;
                case ResizerType.Bottom:
                    y2 += delta.Y;
                    TryResize();
                    break;
            }
            void TryResize()
            {
                var dy = (y2 - y1) / 2;
                if (dy < _min || dy > _max) return;

                var dx = (x2 - x1) / 2;
                if (dx < _min || dx > _max) return;

                DX = (byte)dx;
                DY = (byte)dy;
                X = (x1 + x2) / 2;
                Y = (y1 + y2) / 2;
            }
        }
        internal void Move(Vector2 delta)
        {
            X = X + delta.X;
            Y = Y + delta.Y;
        }
        #endregion

        #region Minimize, HitTest  ============================================
        static readonly int _ds = GraphDefault.HitMargin;

        public void Minimize(Extent e)
        {
            var t = e;
            var p = Center;
            t.Point2 = p;
            if (t.Diagonal < e.Diagonal) e.Point2 = p;
        }


        // quickly eliminate nodes that don't qaulify
        public bool HitTest(Vector2 p)
        {
            var x = p.X + 1;
            if (x < (X - DX - _ds)) return false;
            if (x > (X + DX + _ds)) return false;

            var y = p.Y + 1;
            if (y < (Y - DY - _ds)) return false;
            if (y > (Y + DY + _ds)) return false;
            return true;
        }

        public (HitLocation hit, Vector2 pnt) RefinedHit(Vector2 p)
        {
            float ds;
            var x = p.X + 1;
            var y = p.Y + 1;
            var hit = HitLocation.Node;

            if (DX >= _ds)
            {
                if (x < X)
                {
                    ds = X - DX - x;
                    if (ds < 0 && ds + _ds >= 0) hit |= HitLocation.Left;
                    else if (ds > 0 && ds - _ds <= 0) hit |= HitLocation.Left;
                }
                else
                {
                    ds = X + DX - x;
                    if (ds < 0 && ds + _ds >= 0) hit |= HitLocation.Right;
                    else if (ds > 0 && ds - _ds <= 0) hit |= HitLocation.Right;
                }
            }

            if (DY >= _ds)
            {
                if (y < Y)
                {
                    ds = Y - DY - y;
                    if (ds < 0 && ds + _ds >= 0) hit |= HitLocation.Top;
                    else if (ds > 0 && ds - _ds <= 0) hit |= HitLocation.Top;
                }
                else
                {
                    ds = Y + DY - y;
                    if (ds < 0 && ds + _ds >= 0) hit |= HitLocation.Bottom;
                    else if (ds > 0 && ds - _ds <= 0) hit |= HitLocation.Bottom;
                }
            }
            return (hit, p);
        }
        #endregion

        #region Properties/Methods  ===========================================
        internal bool HasOpenPaths => (OpenPathIndex >= 0);
        internal int OpenPathCount => (Owner == null) ? 0 : Owner.OpenPathCount(OpenPathIndex);
        public override string ToString() => Item.GetFullNameId();
        internal int EdgeCount => Owner.Node_Edges.TryGetValue(this, out List<Edge> list) ? list.Count : 0;
        internal List<Edge> EdgeList => Owner.Node_Edges.TryGetValue(this, out List<Edge> list) ? list : null;
        private void UpdateModels()
        {
            var graph = Owner;
            var root = graph.Owner.Owner.Owner;
            foreach (var pm in root.Items)
            {
                if (pm.LeadModel is GraphModel gm && gm.Graph == graph) gm.FullRefresh();
            }
        }
        #endregion
    }
}
