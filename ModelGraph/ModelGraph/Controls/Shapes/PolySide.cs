using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Controls
{
    internal class PolySide : Polygon
    {
        internal PolySide(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = Radius2 = 0.5f;
            Dimension = 3;
            CreatePoints();
        }

        protected override void CreatePoints()
        {

            var D = Dimension;
            DXY = new List<(float dx, float dy)>(D);
            var da = FullRadians / D;
            var (r1, r2, f1) = GetRadius();
            var a = RadiansStart;
            for (int i = 0; i < D; i++)
            {
                DXY.Add(Limit((r1 * (float)Math.Cos(a), r1 * (float)Math.Sin(a))));
                a += da;
            }
        }

        #region PrivateConstructor  ===========================================
        private PolySide(MShape shape)
        {
            CopyData(shape);
        }
        private PolySide(MShape shape, Vector2 center)
        {
            CopyData(shape);
            SetCenter(new MShape[] { this }, center);
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override MShape Clone() =>new PolySide(this);
        internal override MShape Clone(Vector2 center) => new PolySide(this, center);
        internal override HasSlider Sliders => HasSlider.Horz | HasSlider.Vert | HasSlider.Major | HasSlider.Dim;
        protected override byte TypeCode => (byte)ShapeType.PolySide;

        #endregion
    }
}
