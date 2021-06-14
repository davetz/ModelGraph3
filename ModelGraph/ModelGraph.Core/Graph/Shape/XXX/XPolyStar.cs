using System;
using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class XPolyStar : XPolygon
    {
        protected override ShapeType ShapeType => ShapeType.PolyStar;
        internal XPolyStar(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = 0.5f;
            Radius2 = 0.2f;
            Dimension = 6;
            CreatePoints();
        }

        protected override void CreatePoints()
        {
            var cp = GetCenter();
            var D = Dimension;
            var N = 2 * D;
            DXY = new List<Vector2>(N);
            var (r1, r2, _) = GetRadius();
            var da = FullRadians / N;
            var a = RadiansStart;
            for (int i = 0; i < D; i++)
            {
                DXY.Add(Limit(r1 * (float)Math.Cos(a), r1 * (float)Math.Sin(a)));
                a += da;
                DXY.Add(Limit(r2 * (float)Math.Cos(a), r2 * (float)Math.Sin(a)));
                a += da;
            }
            TransformPoints(Matrix3x2.CreateRotation(RadiansStart));
            SetCenter(cp);
        }

        #region PrivateConstructor  ===========================================
        private XPolyStar(XShape shape)
        {
            CopyData(shape);
        }
        private XPolyStar(XShape shape, Vector2 p)
        {
            CopyData(shape);
            SetCenter(p);
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override XShape Clone() =>new XPolyStar(this);
        internal override XShape Clone(Vector2 center) => new XPolyStar(this, center);
        protected override XShapeProperty PropertyFlags => XShapeProperty.Rad2 | XShapeProperty.Rad1 | XShapeProperty.Dim;
        #endregion
    }
}
