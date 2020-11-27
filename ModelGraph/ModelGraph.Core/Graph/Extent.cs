using System;
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
            X1 = p.X + 1 - ds;
            X2 = p.X + 1 + ds;
            Y1 = p.Y + 1 - ds;
            Y2 = p.Y + 1 + ds;
        }

        public static Extent Create (IEnumerable<Node> nodes, int margin)
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
        public void Record(Vector2 p) { Point1 = Point2; Point2 = p; }

        public void Record(Vector2 p, float scale) { Point1 = Point2; SetPoint2(p, scale); }
        public void SetPoint1(Vector2 p, float scale) { X1 = (int)(p.X * scale); Y1 = (int)(p.Y * scale); }
        public void SetPoint2(Vector2 p, float scale) { X2 = (int)(p.X * scale); Y2 = (int)(p.Y * scale); }
        internal Vector2[] DrawPoints => new Vector2[] { TopLeft, TopRight, BottomRight, BottomLeft };

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
            Normalize(Point1, Point2);
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

        #region Comparison  ===================================================
        public bool IsLessThan(Extent e) => Diagonal < (e.Diagonal);
        public bool IsGreaterThan(Extent e) => Diagonal > (e.Diagonal); 
        #endregion

        #region Contains  =====================================================
        public bool ContainsX(float X)
        {
            if (X < X1) return false;
            if (X > X2) return false;
            return true;
        }

        public bool ContainsY(float Y)
        {
            if (Y < Y1) return false;
            if (Y > Y2) return false;
            return true;
        }

        public bool Contains(Vector2 p)
        {
            if (p.X < X1 && p.X < X2) return false;
            if (p.Y < Y1 && p.Y < Y2) return false;
            if (p.X > X2 && p.X > X1) return false;
            if (p.Y > Y2 && p.Y > Y1) return false;
            return true;
        }
        //public bool Contains(Vector2 p)
        //{
        //    if (p.X < X1) return false;
        //    if (p.Y < Y1) return false;
        //    if (p.X > X2) return false;
        //    if (p.Y > Y2) return false;
        //    return true;
        //}

        public bool Contains(ref Extent e)
        {
            if (e.X1 < X1) return false;
            if (e.Y1 < Y1) return false;
            if (e.X2 > X2) return false;
            if (e.Y2 > Y2) return false;
            return true;
        }
        #endregion

        #region HitTest  ======================================================
        // p as input is the point we are testing
        // E is the target range arround point p
        // p as output is the closest point that lies on the line defined by my Point1 and my Point2
        //
        //          my  Point1 o - - - -
        //                     :\      : <- my extent
        //            : - - - - -\ - : : 
        //       E -> :        :  p  : :
        //            :    p   :   \ : :
        //            :        :    \: :
        //            : - - - - - - -\ :
        //                     :      \:
        //                     - - - - o my Point2
        //
        public bool HitTest(ref Vector2 p, Extent E)
        {
            if (Intersects(E))  // my extent intersects with E
            {
                if (IsHorizontal)   // my Y1 == my Y2
                {
                    p.Y = Y1;
                    return true; ;
                }
                else if (IsVertical) // my X1 == my X2
                {
                    p.X = X1;
                    return true;
                }
                else
                {
                    var dx = (double)DX;
                    var dy = (double)DY;

                    int xi = (int)(X1 + (dx * (p.Y - Y1)) / dy);
                    if (E.ContainsX(xi))
                    {
                        p.X = xi;
                        return true;
                    }
                    xi = (int)(X2 + (dx * (p.Y - Y2)) / dy);
                    if (E.ContainsX(xi))
                    {
                        p.X = xi;
                        return true;
                    }

                    int yi = (int)(Y1 + (dy * (p.X - X1)) / dx);
                    if (E.ContainsY(yi))
                    {
                        p.Y = yi;
                        return true;
                    }
                    yi = (int)(Y2 + (dy * (p.X - X2)) / dx);
                    if (E.ContainsY(yi))
                    {
                        p.Y = yi;
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region Intersection  =================================================
        public bool Intersects(Extent e)
        {
            if (IsVertical)
            {
                if (e.IsVertical)
                {
                    if (ContainsY(e.Y1) && ContainsY(e.Y2)) return true;
                }
                else if (e.IsHorizontal)
                {
                    if (e.ContainsX(X1) && ContainsY(e.Y1)) return true;
                }
                else
                {
                    if (!e.ContainsX(X1)) return false;
                    if (ContainsY(e.Y1)) return true;
                    if (ContainsY(e.Y2)) return true;
                    if (e.ContainsY(Y1)) return true;
                }
            }
            else if (IsHorizontal)
            {
                if (e.IsVertical)
                {
                    if (e.ContainsY(Y1) && ContainsX(e.X1)) return true;
                }
                else if (e.IsHorizontal)
                {
                    if (ContainsX(e.X1) && ContainsX(e.X2)) return true;
                }
                else
                {
                    if (!e.ContainsY(Y1)) return false;
                    if (ContainsX(e.X1)) return true;
                    if (ContainsX(e.X2)) return true;
                    if (e.ContainsX(X1)) return true;
                }
            }
            else
            {
                if (e.IsVertical)
                {
                    if (!ContainsX(e.X1)) return false;
                    if (ContainsY(e.Y1)) return true;
                    if (ContainsY(e.Y2)) return true;
                    if (e.ContainsY(Y1)) return true;

                }
                else if (e.IsHorizontal)
                {
                    if (!ContainsY(e.Y1)) return false;
                    if (ContainsX(e.X1)) return true;
                    if (ContainsX(e.X2)) return true;
                    if (e.ContainsX(X1)) return true;
                }
                else
                {
                    if (Contains(e.TopLeft)) return true;
                    if (Contains(e.TopRight)) return true;
                    if (Contains(e.BottomLeft)) return true;
                    if (Contains(e.BottomRight)) return true;

                    if (e.Contains(TopLeft)) return true;
                    if (e.Contains(TopRight)) return true;
                    if (e.Contains(BottomLeft)) return true;
                    if (e.Contains(BottomRight)) return true;
                }
            }
            return false;
        }

        /// <summary>Dose line segment p1-->p2 intersect this extent</summary>
        internal bool Intersects(Vector2 p1, Vector2 p2) => 
            Contains(p1) || 
            Contains(p2) || 
            Intersects(p1.X, p1.Y, p2.X, p2.Y, X1, Y1, X2, Y1) || 
            Intersects(p1.X, p1.Y, p2.X, p2.Y, X2, Y1, X2, Y2) || 
            Intersects(p1.X, p1.Y, p2.X, p2.Y, X1, Y2, X2, Y2) ||
            Intersects(p1.X, p1.Y, p2.X, p2.Y, X1, Y1, X1, Y2);

        private bool Intersects(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            var a1 = y2 - y1;
            var b1 = x1 - x2;
            var c1 = x2 * y1 - x1 * y2;

            var r3 = a1 * x3 + b1 * y3 + c1;
            var r4 = a1 * x4 + b1 * y4 + c1;

            if (r3 != 0 && r4 != 0 && SameSigns(r3, r4)) return false;

            var a2 = y4 - y3;
            var b2 = x3 - x4;
            var c2 = x4 * y3 - x3 * y4;

            var r1 = a2 * x1 + b2 * y1 + c2;
            var r2 = a2 * x2 + b2 * y2 + c2;

            if (r1 != 0 && r2 != 0 && SameSigns(r1, r2)) return false;

            return (a1 * b2 - a2 * b1) != 0;
        }
        /// <summary>Determin if the line segments p1-->p2 and p3-->p4 intersect and if so return the intersection point</summary>
        internal (bool, Vector2) Inersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            return Inersection(p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y, p4.X, p4.Y);
        }
        private (bool, Vector2) Inersection(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            var zz = new Vector2();

            var a1 = y2 - y1;
            var b1 = x1 - x2;
            var c1 = x2 * y1 - x1 * y2;

            var r3 = a1 * x3 + b1 * y3 + c1;
            var r4 = a1 * x4 + b1 * y4 + c1;

            if (r3 != 0 && r4 != 0 && SameSigns(r3, r4)) return (false, zz);

            var a2 = y4 - y3;
            var b2 = x3 - x4;
            var c2 = x4 * y3 - x3 * y4;

            var r1 = a2 * x1 + b2 * y1 + c2;
            var r2 = a2 * x2 + b2 * y2 + c2;

            if (r1 != 0 && r2 != 0 && SameSigns(r1, r2)) return (false, zz);


            var denom = a1 * b2 - a2 * b1;
            if (denom == 0) return (false, zz); //line segments are collinear, not usefull

//            var offset = denom < 0 ? -denom / 2 : denom / 2;

            var vx = b1 * c2 - b2 * c1;
//          var xi = (v1 < 0 ? v1 - offset : v1 + offset) / denom;

            var vy = a2 * c1 - a1 * c2;
//            var yi = (v2 < 0 ? v2 - offset : v2 + offset) / denom;

            return (true, new Vector2(vx, vy));
        }
        private bool SameSigns(float a, float b) => (a > 0 && b > 0) || (a < 0 && b < 0);

        #region Original-C-Code  ==============================================
    // /* lines_intersect:  AUTHOR: Mukesh Prasad
    // *
    // *   This function computes whether two line segments,
    // *   respectively joining the input points (x1,y1) -- (x2,y2)
    // *   and the input points (x3,y3) -- (x4,y4) intersect.
    // *   If the lines intersect, the output variables x, y are
    // *   set to coordinates of the point of intersection.
    // *
    // *   All values are in integers.  The returned value is rounded
    // *   to the nearest integer point.
    // *
    // *   If non-integral grid points are relevant, the function
    // *   can easily be transformed by substituting floating point
    // *   calculations instead of integer calculations.
    // *    
    // *   Entry
    // *        x1, y1,  x2, y2   Coordinates of endpoints of one segment.
    // *        x3, y3,  x4, y4   Coordinates of endpoints of other segment.
    // *
    // *   Exit
    // *        x, y              Coordinates of intersection point.
    // *
    // *   The value returned by the function is one of:
    // *
    // *        DONT_INTERSECT    0
    // *        DO_INTERSECT      1
    // *        COLLINEAR         2
    // *
    // * Error conditions:
    // *
    // *     Depending upon the possible ranges, and particularly on 16-bit
    // *     computers, care should be taken to protect from overflow.
    // *
    // *     In the following code, 'long' values have been used for this
    // *     purpose, instead of 'int'.
    // *
    // */
    
    //# include <stdio.h>
    
    //#define DONT_INTERSECT    0
    //#define DO_INTERSECT      1
    //#define COLLINEAR         2

    //        /**************************************************************
    //         *                                                            *
    //         *    NOTE:  The following macro to determine if two numbers  *
    //         *    have the same sign, is for 2's complement number        *
    //         *    representation.  It will need to be modified for other  *
    //         *    number systems.                                         *
    //         *                                                            *
    //         **************************************************************/
    
    //#define SAME_SIGNS( a, b )	\
    //        (((long) ((unsigned long) a ^ (unsigned long) b)) >= 0 )
    
    //int lines_intersect(x1, y1,   /* First line segment */
    //             x2, y2,
    
    //             x3, y3,   /* Second line segment */
    //             x4, y4,
    
    //             x,
    //             y         /* Output value:
    //		                * point of intersection */
    //               )
    //long
    //    x1, y1, x2, y2, x3, y3, x4, y4,
    //    *x, *y;
    //{
    //    long a1, a2, b1, b2, c1, c2; /* Coefficients of line eqns. */
    //        long r1, r2, r3, r4;         /* 'Sign' values */
    //        long denom, offset, num;     /* Intermediate values */
    
    //        /* Compute a1, b1, c1, where line joining points 1 and 2
    //         * is "a1 x  +  b1 y  +  c1  =  0".
    //         */
    
    //        a1 = y2 - y1;
    //    b1 = x1 - x2;
    //    c1 = x2* y1 - x1* y2;
    
    //        /* Compute r3 and r4.
    //         */
    
    
    //        r3 = a1* x3 + b1* y3 + c1;
    //    r4 = a1* x4 + b1* y4 + c1;

    //    /* Check signs of r3 and r4.  If both point 3 and point 4 lie on
    //     * same side of line 1, the line segments do not intersect.
    //     */

    //    if (r3 != 0 &&
    //         r4 != 0 &&
    //         SAME_SIGNS(r3, r4 ))
    //        return (DONT_INTERSECT );

    //    /* Compute a2, b2, c2 */

    //    a2 = y4 - y3;
    //    b2 = x3 - x4;
    //    c2 = x4* y3 - x3* y4;

    //        /* Compute r1 and r2 */

    //        r1 = a2* x1 + b2* y1 + c2;
    //    r2 = a2* x2 + b2* y2 + c2;

    //    /* Check signs of r1 and r2.  If both point 1 and point 2 lie
    //     * on same side of second line segment, the line segments do
    //     * not intersect.
    //     */

    //    if (r1 != 0 &&
    //         r2 != 0 &&
    //         SAME_SIGNS(r1, r2 ))
    //        return (DONT_INTERSECT );

    //    /* Line segments intersect: compute intersection point. 
    //     */

    //    denom = a1* b2 - a2* b1;
    //    if (denom == 0 )
    //        return (COLLINEAR );
    //    offset = denom< 0 ? - denom / 2 : denom / 2;

    //    /* The denom/2 is to get rounding instead of truncating.  It
    //     * is added or subtracted to the numerator, depending upon the
    //     * sign of the numerator.
    //     */

    //    num = b1* c2 - b2* c1;
    //    *x = (num< 0 ? num - offset : num + offset ) / denom;

    //    num = a2* c1 - a1* c2;
    //    *y = (num< 0 ? num - offset : num + offset ) / denom;

    //    return (DO_INTERSECT );
    //    } /* lines_intersect */

    //    int main()
    //    {
    //        long int x1, x2, x3, x4, y1, y2, y3, y4;
    //        long int x, y;
    
    //        for (; ; )
    //        {
    //            printf("X1, Y1: ");
    //            scanf("%ld %ld", &x1, &y1);
    //            printf("X2, Y2: ");
    //            scanf("%ld %ld", &x2, &y2);
    //            printf("X3, Y3: ");
    //            scanf("%ld %ld", &x3, &y3);
    //            printf("X4, Y4: ");
    //            scanf("%ld %ld", &x4, &y4);

    //            switch (lines_intersect(x1, y1, x2, y2, x3, y3, x4, y4, &x, &y))
    //            {
    //                case DONT_INTERSECT:
    //                    printf("Lines don't intersect\n");
    //                    break;
    //                case COLLINEAR:
    //                    printf("Lines are collinear\n");
    //                    break;
    //                case DO_INTERSECT:
    //                    printf("Lines intersect at %ld,%ld\n", x, y); 
    //                    break;
    //            }
    //        }
    //    } /* main */
    #endregion
        #endregion
    }
}
