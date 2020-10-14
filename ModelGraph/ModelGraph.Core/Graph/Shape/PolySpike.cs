using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class PolySpike : Polyline
    {
        protected override ShapeType ShapeType => ShapeType.PolySpike;
        internal PolySpike(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = 0.25f;
            Radius2 = 0.25f;
            AuxFactor = 0.50f;
            Dimension = 4;
            CreatePoints();
        }

        #region PrivateConstructor  ===========================================
        private PolySpike(Shape shape)
        {
            CopyData(shape);
        }
        private PolySpike(Shape shape, Vector2 p)
        {
            CopyData(shape);
            SetCenter(p.X, p.Y);
        }
        #endregion

        #region CreatePoints  =================================================
        protected override void CreatePoints()
        {
            var (cx, cy) = GetCenter();
            var D = Dimension;
            var n = 0;
            var N = 1 + D;
            DXY = new List<(float dx, float dy)>(N);

            var (r1, r2, f1) = GetRadius();

            var ax = r1 * 4 / D; //ax step size
            var bx = ax * f1; //ax step size

            var dx = -r1;
            var dy = r2;

            for (int i = 0; i < N; i++)
            {
                if (Add(dx, -dy)) return;
                if (Add(dx + bx, dy)) return;
                dx += ax;
            }
            TransformPoints(Matrix3x2.CreateRotation(RadiansStart));
            SetCenter(cx, cy);

            bool Add(float x, float y)
            {
                DXY.Add(Limit(x, y));
                return (++n >= N);
            }
        }
        #endregion

        #region RequiredMethods  ==============================================
        internal override Shape Clone() =>new PolySpike(this);
        internal override Shape Clone(Vector2 center) => new PolySpike(this, center);
        protected override (int min, int max) MinMaxDimension => (2, 20);
        protected override ShapeProperty PropertyFlags => ShapeProperty.Rad1 | ShapeProperty.Rad2 | ShapeProperty.Dim;
        #endregion
    }
}
