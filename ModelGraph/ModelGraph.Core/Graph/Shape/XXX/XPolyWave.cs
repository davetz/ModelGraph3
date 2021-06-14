using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class XPolyWave : XPolySpline
    {
        protected override ShapeType ShapeType => ShapeType.PolyWave;
        internal XPolyWave(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = 0.75f;
            Radius2 = 0.25f;
            Dimension = 6;
            CreatePoints();
        }

        #region PrivateConstructor  ===========================================
        private XPolyWave(XShape shape)
        {
            CopyData(shape);
        }
        private XPolyWave(XShape shape, Vector2 p)
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
            var (r1, r2, _) = GetRadius();

            var n = 0;
            var N = 1 + D * 2; // number of points per spline
            DXY = new List<Vector2>(N);
            float dx = -r1, dy = r2, adx = r1 / D;

            Add(dx, 0);

            for (int i = 0; i < N; i++)
            {
                if (AddLobe()) break;
            }
            TransformPoints(Matrix3x2.CreateRotation(RadiansStart));
            SetCenter(cp);

            bool AddLobe()
            {
                dx += adx;
                if (Add(dx, dy)) return true;
                dx += adx;
                if(Add(dx, dy)) return true;
                dx += adx;
                if (Add(dx, dy)) return true;
                dx += adx;
                if (Add(dx, 0)) return true;
                dy = -1 * dy;
                return false;
            }
            bool Add(float x, float y)
            {
                DXY.Add(Limit(x, y));
                return (++n >= N); 
            }
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override XShape Clone() => new XPolyWave(this);
        internal override XShape Clone(Vector2 center) => new XPolyWave(this, center);
        protected override (int min, int max) MinMaxDimension => (1, 22);
        protected override XShapeProperty PropertyFlags => XShapeProperty.Rad2 | XShapeProperty.Rad1 | XShapeProperty.Dim;
        #endregion
    }
}
