using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class XPolySpring : XPolySpline
    {
        protected override ShapeType ShapeType => ShapeType.PolySpring;
        internal XPolySpring(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = 0.75f;
            Radius2 = 0.35f;
            Dimension = 6;
            CreatePoints();
        }

        #region PrivateConstructor  ===========================================
        private XPolySpring(XShape shape)
        {
            CopyData(shape);
        }
        private XPolySpring(XShape shape, Vector2 p)
        {
            CopyData(shape);
            SetCenter(p);
        }
        #endregion

        #region CreatePoints  =================================================
        private enum PS { L1, L2, L3, L4 }
        protected override void CreatePoints()
        {
            var cp = GetCenter();
            var D = Dimension;
            var (r1, r2, _) = GetRadius();

            var n = 0;
            var N = 1 + D * 2; // number of points per spline
            DXY = new List<Vector2>(N);
            float dx = -r1, dy = r2, adx, bdx, cdx;

            var ps = (D < 7) ? PS.L1 : (D < 11) ? PS.L2 : (D < 15) ? PS.L3 : PS.L4;
            switch (ps)
            {
                case PS.L1:
                    SetDelta(2 * r1 / 6);

                    if (AddLobe(adx, -dy)) break;  // 1
                    if (AddLobe(bdx, dy)) break;   // 2
                    if (AddLobe(adx, -dy)) break;  // 3
                    break;

                case PS.L2:
                    SetDelta(2 * r1 / 9);

                    if (AddLobe(adx, -dy)) break;  // 1
                    if (AddLobe(bdx, dy)) break;   // 2
                    if (AddLobe(cdx, -dy)) break;  // 3
                    if (AddLobe(bdx, dy)) break;   // 4
                    if (AddLobe(adx, -dy)) break;  // 5
                    break;
                case PS.L3:
                    SetDelta(2 * r1 / 12);

                    if (AddLobe(adx, -dy)) break;  // 1
                    if (AddLobe(bdx, dy)) break;   // 2
                    if (AddLobe(cdx, -dy)) break;  // 3
                    if (AddLobe(bdx, dy)) break;   // 4
                    if (AddLobe(cdx, -dy)) break;  // 5
                    if (AddLobe(bdx, dy)) break;   // 6
                    if (AddLobe(adx, -dy)) break;  // 7
                    break;

                case PS.L4:
                    SetDelta(2 * r1 / 14);

                    if (AddLobe(adx, -dy)) break;  // 1
                    if (AddLobe(bdx, dy)) break;   // 2
                    if (AddLobe(cdx, -dy)) break;  // 3
                    if (AddLobe(bdx, dy)) break;   // 4
                    if (AddLobe(cdx, -dy)) break;  // 5
                    if (AddLobe(bdx, dy)) break;   // 6
                    if (AddLobe(cdx, -dy)) break;  // 7
                    if (AddLobe(bdx, dy)) break;   // 8
                    if (AddLobe(adx, -dy)) break;  // 9
                    break;
            }
            TransformPoints(Matrix3x2.CreateRotation(RadiansStart));
            SetCenter(cp);

            void SetDelta(float ds)
            {
                adx = ds * 2;
                bdx = ds * -1;
                cdx = ds * 2.5f;
                Add(dx, 0);
            }
            bool AddLobe(float tdx, float tdy)
            {
                if (Add(dx, tdy)) return true;
                dx += tdx;
                if (Add(dx, tdy)) return true;
                dx += tdx;
                if (Add(dx, tdy)) return true;
                if (Add(dx, 0)) return true;
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
        internal override XShape Clone() => new XPolySpring(this);
        internal override XShape Clone(Vector2 center) => new XPolySpring(this, center);
        protected override (int min, int max) MinMaxDimension => (1, 18);
        protected override XShapeProperty PropertyFlags => XShapeProperty.Rad2 | XShapeProperty.Rad1 | XShapeProperty.Dim;
        #endregion
    }
}
