﻿using System;
using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class PolyGear : Polygon
    {
        protected override ShapeType ShapeType => ShapeType.PolyGear;
        internal PolyGear(bool deserializing = false)
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
            var D = Dimension;
            var N = 3 * D; //number of points
            var M = 2 * D; //number of angles
            DXY = new List<(float dx, float dy)>(N);
            var (r1, r2, f1) = GetRadius();

            var da = FullRadians / M;
            var ta = da * f1;
            var a = RadiansStart;
            for (int i = 0; i < D; i++)
            {
                DXY.Add(Limit((r1 * (float)Math.Cos(a - ta), r1 * (float)Math.Sin(a - ta))));
                DXY.Add(Limit((r1 * (float)Math.Cos(a + ta), r1 * (float)Math.Sin(a + ta))));
                a += da;
                DXY.Add(Limit((r2 * (float)Math.Cos(a), r2 * (float)Math.Sin(a))));
                a += da;
            }
        }

        #region PrivateConstructor  ===========================================
        private PolyGear(ShapeBase shape)
        {
            CopyData(shape);
        }
        private PolyGear(ShapeBase shape, Vector2 center)
        {
            CopyData(shape);
            SetCenter(new ShapeBase[] { this }, center);
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override ShapeBase Clone() =>new PolyGear(this);
        internal override ShapeBase Clone(Vector2 center) => new PolyGear(this, center);
        internal override HasSlider Sliders => HasSlider.Horz | HasSlider.Vert |HasSlider.Major | HasSlider.Minor | HasSlider.Aux | HasSlider.Dim;
        protected override byte TypeCode => (byte)ShapeType.PolyGear;
        #endregion
    }
}
