using System;
using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class PolySide : Polygon
    {
        protected override ShapeType ShapeType => ShapeType.PolySide;
        internal PolySide(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = Radius2 = 0.5f;
            Dimension = 3;
            CreatePoints();
        }

        protected override void CreatePoints()
        {
            var (cx, cy) = GetCenter();
            var D = Dimension;
            DXY = new List<(float dx, float dy)>(D);
            var da = FullRadians / D;
            var (r1, _, _) = GetRadius();
            var a = RadiansStart;
            for (int i = 0; i <= D; i++)
            {
                DXY.Add(Limit((r1 * (float)Math.Cos(a), r1 * (float)Math.Sin(a))));
                a += da;
            }
            TransformPoints(Matrix3x2.CreateRotation(RadiansStart));
            SetCenter(cx, cy);
        }

        #region PrivateConstructor  ===========================================
        private PolySide(Shape shape)
        {
            CopyData(shape);
        }
        private PolySide(Shape shape, Vector2 p)
        {
            CopyData(shape);
            SetCenter(p.X, p.Y);
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override Shape Clone() =>new PolySide(this);
        internal override Shape Clone(Vector2 center) => new PolySide(this, center);
        protected override ShapeProperty PropertyFlags => ShapeProperty.Rad1 | ShapeProperty.Dim;

        protected override (float, float) GetCenter()
        {
            var n = DXY is null ? 0 : DXY.Count;
            if (n < 3) return (0, 0);

            float sa = 0, sx = 0, sy = 0, ds;
            for (int i = 0, j = 1; j < n;)
            {
                var (xi, yi) = DXY[i++];
                var (xj, yj) = DXY[j++];
                ds = (xi * yj - yi * xj);

                sa += ds;
                sx += (xi + xj) * ds;
                sy += (yi + yj) * ds;
            }
            var a = 3 * sa;
            var cx = sx / a;
            var cy = sy / a;

            return (cx, cy);
        }

        #endregion
    }
}
