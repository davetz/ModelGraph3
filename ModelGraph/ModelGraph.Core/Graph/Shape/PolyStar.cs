﻿using System;
using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class PolyStar : Polygon
    {
        protected override ShapeType ShapeType => ShapeType.PolyStar;
        internal PolyStar(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = 0.5f;
            Radius2 = 0.2f;
            Dimension = 6;
            CreatePoints();
        }

        protected override void CreatePoints()
        {
            var D = Dimension;
            var N = 2 * D;
            DXY = new List<(float dx, float dy)>(N);
            var (r1, r2, _) = GetRadius();
            var da = FullRadians / N;
            var a = RadiansStart;
            for (int i = 0; i < D; i++)
            {
                DXY.Add(Limit((r1 * (float)Math.Cos(a), r1 * (float)Math.Sin(a))));
                a += da;
                DXY.Add(Limit((r2 * (float)Math.Cos(a), r2 * (float)Math.Sin(a))));
                a += da;
            }
        }

        #region PrivateConstructor  ===========================================
        private PolyStar(ShapeBase shape)
        {
            CopyData(shape);
        }
        private PolyStar(ShapeBase shape, Vector2 center)
        {
            CopyData(shape);
            SetCenter(new ShapeBase[] { this }, center);
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override ShapeBase Clone() =>new PolyStar(this);
        internal override ShapeBase Clone(Vector2 center) => new PolyStar(this, center);
        internal override HasSlider Sliders => HasSlider.Horz | HasSlider.Vert | HasSlider.Minor | HasSlider.Major | HasSlider.Dim;
        #endregion
    }
}