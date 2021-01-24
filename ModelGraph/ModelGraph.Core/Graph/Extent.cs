using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    public class Extent
    {
        public float X1;
        public float Y1;
        public float X2;
        public float Y2;

        #region Constructor  ==================================================
        public Extent(int s = 0)
        {
            X1 = X2 = Y1 = Y2 = s;
        }
        public Extent(Extent e)
        {
            X1 = e.X1;
            Y1 = e.Y1;
            X2 = e.X2;
            Y2 = e.Y2;
        }
        public (float, float, float, float) GetFloat() => (X1, Y1, X2, Y2);
        public Extent Clone => new Extent(this);

        public Extent(Vector2 p)
        {
            X1 = X2 = p.X;
            Y1 = Y2 = p.Y;
        }

        public Extent(float x, float y)
        {
            X1 = X2 = x;
            Y1 = Y2 = y;
        }

        public Extent(float x1, float y1, float x2, float y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public Extent((float x1, float y1, float x2, float y2) e, float ds)
        {
            X1 = e.x1 - ds;
            Y1 = e.y1 - ds;
            X2 = e.x2 + ds;
            Y2 = e.y2 + ds;
        }

        public Extent(Vector2 p, float ds)
        {
            X1 = p.X - ds;
            X2 = p.X + ds;
            Y1 = p.Y - ds;
            Y2 = p.Y + ds;
        }

        public static Extent Create(IEnumerable<Node> nodes, int margin)
        {
            var e = new Extent();
            return e.SetExtent(nodes, margin);
        }
        public Extent(Vector2 p1, Vector2 p2)
        {
            if (p1.X < p2.X)
            {
                X1 = p1.X;
                X2 = p2.X;
            }
            else
            {
                X1 = p2.X;
                X2 = p1.X;
            }
            if (p1.Y < p2.Y)
            {
                Y1 = p1.Y;
                Y2 = p2.Y;
            }
            else
            {
                Y1 = p2.Y;
                Y2 = p1.Y;
            }
        }
        #endregion

        #region Move  =========================================================
        internal void Move(float dx, float dy)
        {
            X1 += dx;
            X2 += dx;
            Y1 += dy;
            Y2 += dy;
        }
        internal void MoveX(float dx)
        {
            X1 += dx;
            X2 += dx;
        }
        internal void MoveY(float dy)
        {
            Y1 += dy;
            Y2 += dy;
        }
        #endregion

        #region Shape  ========================================================
        public bool IsTall { get { return DY > DX; } }
        public bool IsWide { get { return DX > DY; } }
        public bool IsEmpty { get { return (IsVertical || IsHorizontal); } }
        public bool HasArea { get { return (X1 != X2 && Y1 != Y2); } }
        public bool IsVertical => (X2 - X1 < 0) ? (X2 - X1 > -4) : (X2 - X1 < 4);
        public bool IsHorizontal => (Y2 - Y1 < 0) ? (Y2 - Y1 > -4) : (Y2 - Y1 < 4);
        #endregion

        #region Center  =======================================================
        public float CenterX => (X2 + X1) / 2;
        public float CenterY => (Y2 + Y1) / 2;
        public Vector2 Center => new Vector2(CenterX, CenterY);
        #endregion

        #region Points  =======================================================
        public void Points(Vector2 p1, Vector2 p2) { Point1 = p1; Point2 = p2; }
        public Vector2 Point1 { get { return new Vector2(X1, Y1); } set { X1 = value.X; Y1 = value.Y; } }
        public Vector2 Point2 { get { return new Vector2(X2, Y2); } set { X2 = value.X; Y2 = value.Y; } }
        #endregion

        #region LineSegment  ==================================================
        /// <summary>Begin the setup for stepwise polyline hit testing</summary>
        internal void LineSegmentStart(Vector2 p)
        {//- - - - - - - - - first point of a polyline
            X1 = X2 = p.X;
            Y1 = Y2 = p.Y;
        }
        /// <summary>Specify the next point which defines a line segment</summary>
        internal void LineSegmentNext(Vector2 p)
        {//- - - - - - - - - next point of the polyline
            X1 = X2;
            Y1 = Y2;
            X2 = p.X;
            Y2 = p.Y;
        }
        /// <summary>Test if the positive ray (in the X direction) intersects the specified line segment</summary>
        internal bool HasRayIntercept(Vector2 p)
        {
            float xmin, ymin, xmax, ymax;

            if (X1 < X2) { xmin = X1; xmax = X2; }
            else { xmin = X2; xmax = X1; }

            if (Y1 < Y2) { ymin = Y1; ymax = Y2; }
            else { ymin = Y2; ymax = Y1; }

            if (p.Y >= ymin && p.Y <= ymax)
            {
                var dx = X2 - X1;
                var dy = Y2 - Y1;
                if (dy < -0.1 || dy > 0.1)
                {
                    var x = (dx * p.Y + X1 * Y2 - X2 * Y1) / dy;
                    return (x >= xmin && x <= xmax);
                }
                return p.X <= xmax;
            }
            return false;
        }
        /// <summary>Test if the x,y point hit the specified line segment</summary>
        internal bool LineSegmentHit(Vector2 p, out Vector2 intersect)
        {
            const float ds = 2;
            float xmin, ymin, xmax, ymax;

            if (X1 < X2) { xmin = X1; xmax = X2; }
            else { xmin = X2; xmax = X1; }

            if (Y1 < Y2) { ymin = Y1; ymax = Y2; }
            else { ymin = Y2; ymax = Y1; }

            if (xmin - ds < p.X && xmax + ds > p.X && ymin - ds < p.Y && ymax + ds > p.Y)
            {
                var dx = X2 - X1;
                var dy = Y2 - Y1;

                var mdx = xmax - xmin;
                var mdy = ymax - ymin;

                if (mdx > ds) // is the line segment vertical ?
                {//- - - - - - -  No
                    if (mdy > ds) // is the line segment horizontal ?
                    {//- - - - - - - No
                        if (mdx > mdy)// is the bounding rectangle wider than it is tall ?
                        {//- - - - - - - - - - - - - - - - - - - - - - - - - - - - -wider, so use p.Y to calculate x intercept
                            var x = (dx * p.Y + X1 * Y2 - X2 * Y1) / dy;
                            if (p.ContainsX(x))
                            {
                                var y = (dy * x + X2 * Y1 - X1 * Y2) / dx;
                                intersect = new Vector2(x, y);
                                return true;
                            }
                        }
                        else
                        {//- - - - - - - - - - - - - - - - - - - - - - - - - - - - -taller, so use p.X to calculate y intercept
                            var y = (dy * p.X + X2 * Y1 - X1 * Y2) / dx;
                            if (p.ContainsY(y))
                            {
                                var x = (dx * y + X1 * Y2 - X2 * Y1) / dy;
                                intersect = new Vector2(x, y);
                                return true;
                            }
                        }
                    }
                    else if (p.ContainsY(Y1))
                    {//- - - - - - - - - - - - - - - - - - - - - - - - the line segment is or is very close to being horizontal
                        intersect = new Vector2(p.X, Y1);
                        return true;
                    }
                }
                else if (p.ContainsX(X1))
                {//- - - - - - - - - - - - - - - - - - - - - - - - - - the line segment is or is very close to being vertival 
                    intersect = new Vector2(X1, p.Y);
                    return true;
                }
            }

            intersect = Vector2.Zero;
            return false;
        }
        #endregion

        #region Expand  =======================================================
        public void Expand(int margin)
        {
            X1 -= margin;
            Y1 -= margin;
            X2 += margin;
            Y2 += margin;
        }

        public void Expand(float x, float y)
        {
            if (x < X1) X1 = x;
            if (y < Y1) Y1 = y;
            if (x > X2) X2 = x;
            if (y > Y2) Y2 = y;
        }
        public void Expand(Vector2 p)
        {
            if (p.X < X1) X1 = p.X;
            if (p.Y < Y1) Y1 = p.Y;
            if (p.X > X2) X2 = p.X;
            if (p.Y > Y2) Y2 = p.Y;
        }
        #endregion

        #region Clear  ========================================================
        public void Clear()
        {
            X1 = 0;
            Y1 = 0;
            X2 = 0;
            Y2 = 0;
        }
        #endregion

        #region Diagonal  =====================================================
        public float DX => X2 - X1;
        public float DY => Y2 - Y1;
        public Vector2 Delta => new Vector2(DX, DY);
        public float Diagonal =>  Vector2.DistanceSquared( Point1, Point2);
        #endregion

        #region Normalize  ====================================================
        // enforce  (X1 < X2) and  (Y1 < Y2)

        public void Normalize()
        {
            if (X1 > X2)
            {
                var Xt = X1;
                X1 = X2;
                X2 = Xt;
            }
            if (Y1 > Y2)
            {
                var Yt = Y1;
                Y1 = Y2;
                Y2 = Yt;
            }
        }

        public void Normalize(Vector2 p1, Vector2 p2)
        {
            if (p2.X < p1.X)
            {
                X1 = p2.X;
                X2 = p1.X;
            }
            else
            {
                X1 = p1.X;
                X2 = p2.X;
            }

            if (p2.Y < p1.Y)
            {
                Y1 = p2.Y;
                Y2 = p1.Y;
            }
            else
            {
                Y1 = p1.Y;
                Y2 = p2.Y;
            }
        }
        #endregion

        #region SetExtent  ====================================================
        // enforce  (X1 < X2) and  (Y1 < Y2)
        public void SetExtent(Node node, int margin)
        {
            var (x, y, w, h) = node.Values();

            X1 = x - w - margin;
            Y1 = y - h - margin;
            X2 = x + w + margin;
            Y2 = y + h + margin;
        }
        public Extent SetExtent(IEnumerable<Node> nodeList, int margin)
        {
            X1 = Y1 = int.MaxValue;
            X2 = Y2 = int.MinValue;
            foreach (var node in nodeList)
            {
                var e = node.Extent;
                if (e.X1 < X1) X1 = e.X1;
                if (e.Y1 < Y1) Y1 = e.Y1;

                if (e.X2 > X2) X2 = e.X2;
                if (e.Y2 > Y2) Y2 = e.Y2;
            }
            if (X1 == int.MaxValue)
            {
                X1 = -margin;
                Y1 = -margin;
                X2 = margin;
                Y2 = margin;
            }
            else
            {
                X1 -= margin;
                Y1 -= margin;
                X2 += margin;
                Y2 += margin;
            }
            return this;
        }

        public Extent SetExtent(Vector2[] points, int margin)
        {
            var N = (points == null) ? 0 : points.Length;
            if (N == 0)
            {
                X1 = Y1 = X2 = Y2 = 0;
            }
            else
            {
                X1 = Y1 = int.MaxValue;
                X2 = Y2 = int.MinValue;
                for (int i = 0; i < N; i++)
                {
                    if (points[i].X < X1) X1 = points[i].X;
                    if (points[i].Y < Y1) Y1 = points[i].Y;

                    if (points[i].X > X2) X2 = points[i].X;
                    if (points[i].Y > Y2) Y2 = points[i].Y;
                }
                X1 -= margin;
                Y1 -= margin;
                X2 += margin;
                Y2 += margin;
            }
            return this;
        }
        #endregion

        #region Rectangle  ====================================================
        // independant of order (x1,y1), (x2,y2)
        public float Xmin => (X1 < X2) ? X1 : X2;
        public float Ymin => (Y1 < Y2) ? Y1 : Y2;
        public float Xmax => (X2 > X1) ? X2 : X1;
        public float Ymax => (Y2 > Y1) ? Y2 : Y1;

        public float Width => (Xmax - Xmin);
        public float Hieght => (Ymax - Ymin);
        public Vector2 TopLeft => new Vector2(Xmin, Ymin);
        public Vector2 TopRight => new Vector2(Xmax, Ymin);
        public Vector2 BottomLeft => new Vector2(Xmin, Ymax);
        public Vector2 BottomRight => new Vector2(Xmax, Ymax);
        #endregion

        #region RoundedRectanglePoints  =======================================
        internal List<Vector2> RoundedRectanglePoints()
        {
            const int r = 4;
            const int c = 1;
            var list = new List<Vector2>(12);
            var tl = TopLeft;
            var tr = TopRight;
            var bl = BottomLeft;
            var br = BottomRight;

            list.Add(new Vector2(tl.X, tl.Y + r));
            list.Add(new Vector2(tl.X + c, tl.Y + c));
            list.Add(new Vector2(tl.X + r, tl.Y));

            list.Add(new Vector2(tr.X - r, tl.Y));
            list.Add(new Vector2(tr.X - c, tr.Y + c));
            list.Add(new Vector2(tr.X, tr.Y + r));

            list.Add(new Vector2(br.X, br.Y - r));
            list.Add(new Vector2(br.X - c, br.Y - c));
            list.Add(new Vector2(br.X - r, br.Y));

            list.Add(new Vector2(bl.X + r, bl.Y));
            list.Add(new Vector2(bl.X + c, bl.Y - c));
            list.Add(new Vector2(bl.X, bl.Y - r));

            return list;
        }
        #endregion

        #region Contains  =====================================================
        public bool Contains(Vector2 p)
        {
            if (p.X < X1) return false;
            if (p.Y < Y1) return false;
            if (p.X > X2) return false;
            if (p.Y > Y2) return false;
            return true;
        }
        #endregion
    }
}
