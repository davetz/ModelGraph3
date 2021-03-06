﻿using System;
using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class XPolyGear : XPolygon
    {
        protected override ShapeType ShapeType => ShapeType.PolyGear;
        internal XPolyGear(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = 0.5f;
            Radius2 = 0.2f;
            AuxFactor = 0.5f;
            Dimension = 4;
            CreatePoints();
        }

        protected override void CreatePoints()
        {
            var cp = GetCenter();

            var D = Dimension;
            var N = 3 * D; //number of points
            var M = 2 * D; //number of angles
            DXY = new List<Vector2>(N);
            var (r1, r2, f1) = GetRadius();

            var da = FullRadians / M;
            var ta = da * f1;
            var a = RadiansStart;
            for (int i = 0; i < D; i++)
            {
                DXY.Add(Limit(r1 * (float)Math.Cos(a - ta), r1 * (float)Math.Sin(a - ta)));
                DXY.Add(Limit(r1 * (float)Math.Cos(a + ta), r1 * (float)Math.Sin(a + ta)));
                a += da;
                DXY.Add(Limit(r2 * (float)Math.Cos(a), r2 * (float)Math.Sin(a)));
                a += da;
            }
            TransformPoints(Matrix3x2.CreateRotation(RadiansStart));
            SetCenter(cp);
        }

        #region PrivateConstructor  ===========================================
        private XPolyGear(XShape shape)
        {
            CopyData(shape);
        }
        private XPolyGear(XShape shape, Vector2 p)
        {
            CopyData(shape);
            SetCenter(p);
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override XShape Clone() =>new XPolyGear(this);
        internal override XShape Clone(Vector2 center) => new XPolyGear(this, center);
        protected override XShapeProperty PropertyFlags => XShapeProperty.Rad1 | XShapeProperty.Rad2 | XShapeProperty.Aux | XShapeProperty.Dim;
        #endregion
    }
}
