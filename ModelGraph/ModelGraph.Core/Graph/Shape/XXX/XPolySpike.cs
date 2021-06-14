using System.Collections.Generic;
using System.Numerics;
using Windows.ApplicationModel.Chat;

namespace ModelGraph.Core
{
    internal class XPolySpike : XPolyline
    {
        protected override ShapeType ShapeType => ShapeType.PolySpike;
        internal XPolySpike(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = 0.75f;
            Radius2 = 0.25f;
            AuxFactor = 0.50f;
            Dimension = 4;
            CreatePoints();
        }

        #region PrivateConstructor  ===========================================
        private XPolySpike(XShape shape)
        {
            CopyData(shape);
        }
        private XPolySpike(XShape shape, Vector2 p)
        {
            CopyData(shape);
            SetCenter(p);
        }
        #endregion

        #region CreatePoints  =================================================
        protected override void CreatePoints()
        {
            var cp = GetCenter();
            var D = Dimension;
            var n = 0;
            var N = 1 + D;
            DXY = new List<Vector2>(N);

            var (r1, r2, f1) = GetRadius();

            var ax = r1 * 4 / D; //ax step size
            var bx = ax * f1; //ax step size

            var dx = -r1;
            var dy = r2;

            for (int i = 0; i < N; i++)
            {
                if (Add(dx, -dy)) goto Restore;
                if (Add(dx + bx, dy)) goto Restore;
                dx += ax;
            }
            Restore:
            TransformPoints(Matrix3x2.CreateRotation(RadiansStart));
            SetCenter(cp);

            bool Add(float x, float y)
            {
                DXY.Add(Limit(x, y));
                return (++n >= N);
            }
        }
        #endregion

        #region RequiredMethods  ==============================================
        internal override XShape Clone() =>new XPolySpike(this);
        internal override XShape Clone(Vector2 center) => new XPolySpike(this, center);
        protected override (int min, int max) MinMaxDimension => (2, 20);
        protected override XShapeProperty PropertyFlags => XShapeProperty.Rad1 | XShapeProperty.Rad2 | XShapeProperty.Dim;
        #endregion
    }
}
