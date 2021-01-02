using System;
using System.Collections.Generic;
using System.Numerics;

namespace TestIntersection
{
    class Program
    {
        static void Main(string[] args)
        {
            for (float i = 0; i < 100; i++)
            {
                var points = new List<Vector2>(new Vector2[] { new Vector2(2, -7), new Vector2(2.01f, -7), new Vector2(2, -7), new Vector2(15, 2), new Vector2(8, 13), new Vector2(-6, 4), });
                var p = new Vector2(15.1f, 1.9f);
                var (isHit, isInflectionPoint, index, point) = PolyLine.HitTes(p, 0.6f, points.ToArray());
                var t = PolyLine.PointIsInside(p, points.ToArray());
                points.Reverse();
                var s = PolyLine.PointIsInside(p, points.ToArray());
                if (t != s)
                {
                    var kkk = 1;
                }
            }
        }
    }
    static class PolyLine
    {

        /// <summary>The point hit the polyline</summary>
        static public (bool, bool, int, Vector2) HitTes(Vector2 p, float hitMargin, Vector2[] points)
        {
            var xmin = p.X - hitMargin;
            var xmax = p.X + hitMargin;
            var ymin = p.Y - hitMargin;
            var ymax = p.Y + hitMargin;
            var N = points.Length;

            var p1 = points[0];
            if (IsInHitZone(p1)) return (true, true, 0, p1); //hit inflection point
            for (int i = 1; i < N; i++)
            {
                var p2 = points[i];
                if (IsInHitZone(p2)) return (true, true, i, p2); //hit inflection point
                var d = p2 - p1;
                if (d.X < -0.001 || d.X > 0.001)
                {
                    if (d.Y < -0.001 || d.Y > 0.001)
                    {
                        if ((d.X > 0 ? d.X : -d.X) > (d.Y > 0 ? d.Y : -d.Y))
                        {//- - - - - - - - - - - - - - - - - - - - - - - - - - - - -use p.Y to calculate x intercept
                            var x = (d.X * p.Y + p1.X * p2.Y - p2.X * p1.Y) / d.Y;
                            if (IsInHitZoneX(x)) return (true, false, i, new Vector2(x, p.Y));
                        }
                        else
                        {//- - - - - - - - - - - - - - - - - - - - - - - - - - - - -use p.X to calculate y intercept
                            var y = (d.Y * p.X + p2.X * p1.Y - p1.X * p2.Y) / d.X;
                            if (IsInHitZoneY(y)) return (true, false, i, new Vector2(p.X, y));
                        }
                    }
                    else if (IsInHitZoneY(p1.Y)) return (true, false, i, new Vector2(p.X, p1.Y)); //is horizontal line
                }
                else if (IsInHitZoneX(p1.X)) return (true, false, i, new Vector2(p1.X, p.Y));//is vertival line
                p1 = p2;
            }
            bool IsInHitZone(Vector2 q) => IsInHitZoneX(q.X) && IsInHitZoneY(q.Y);
            bool IsInHitZoneX(float v) => !(v < xmin || v > xmax);
            bool IsInHitZoneY(float v) => !(v < ymin || v > ymax);
            return (false, false, 0, Vector2.Zero);
        }
        /// <summary>The point is inside of the polygon</summary>
        static public bool PointIsInside(Vector2 p, Vector2[] points)
        {
            var count = 0;
            var N = points.Length;
            if (N > 2)
            {
                var s = false;
                var p1 = points[N - 1];
                for (int i = 0; i < N; i++)
                {
                    var p2 = points[i];
                    var d = p2 - p1;
                    if (d.X < -1|| d.X > 1  || d.Y < -1 || d.Y > 1)
                    {
                        var t = (p.X * d.Y - p.Y * d.X + p1.Y * p2.X - p1.X * p2.Y) > 0; // the CrossProduct is positive 
                        if (count == 0)
                            s = t; // all remaining CrossProducts must have this same sign
                        else if (t != s)
                            return false; //this CrossProduct does not have the correct sign
                        count++;
                        p1 = p2;
                    }
                }
                return count > 2;
            }
            return false;
        }
    }
}
