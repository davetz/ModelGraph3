using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class PolyPulse : Polyline
    {
        protected override ShapeType ShapeType => ShapeType.PolyPulse;
        internal PolyPulse(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = 0.75f;
            Radius2 = 0.25f;
            AuxFactor = 0.25f;
            Dimension = 7;
            CreatePoints();
        }

        #region PrivateConstructor  ===========================================
        private PolyPulse(Shape shape)
        {
            CopyData(shape);
        }
        private PolyPulse(Shape shape, Vector2 p)
        {
            CopyData(shape);
            SetCenter(p.X, p.Y);
        }
        #endregion

        #region CreatePoints  =================================================
        protected override void CreatePoints()
        {
            var D = Dimension;
            var n = 0;
            var N = 1 + D;
            DXY = new List<(float dx, float dy)>(N);

            var (r1, r2, f1) = GetRadius();

            var d = D / 2;
            var ax = r1 * 2 / d; //ax step size
            var bx = ax * f1;
            var dx = -r1;
            var dy = r2;

            Add(dx, -dy);
            dx += bx;
            for (int i = 0; i < D; i++)
            {
                if (Add(dx + bx, dy)) return;
                dx += ax;
                if (Add(dx - bx, dy)) return;
                if (Add(dx + bx, -dy)) return;
                dx += ax;
                if (Add(dx - bx, -dy)) return;
            }
            TransformPoints(Matrix3x2.CreateRotation(RadiansStart));

            bool Add(float x, float y)
            {
                DXY.Add(Limit(x, y));
                return (++n >= N);
            }
        }
        #endregion

        #region RequiredMethods  ==============================================
        internal override Shape Clone() =>new PolyPulse(this);
        internal override Shape Clone(Vector2 center) => new PolyPulse(this, center);
        protected override (int min, int max) MinMaxDimension => (2, 19);
        protected override ShapeProperty PropertyFlags => ShapeProperty.Rad1 | ShapeProperty.Rad2 | ShapeProperty.Aux | ShapeProperty.Dim;
        #endregion
    }
}
