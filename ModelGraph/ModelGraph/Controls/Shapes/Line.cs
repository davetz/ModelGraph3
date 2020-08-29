﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using ModelGraph.Core;
using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Controls
{
    internal class Line : Polyline
    {
        internal Line(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            DXY = new List<(float dx, float dy)>() {(-0.25f, 0), (0.25f, 0)};
        }

        #region PrivateConstructor  ===========================================
        private Line(MShape shape)
        {
            CopyData(shape);
        }
        private Line(MShape shape, Vector2 center)
        {
            CopyData(shape);
            SetCenter( new MShape[] { this }, center);
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override MShape Clone() => new Line(this);
        internal override MShape Clone(Vector2 center) => new Line(this, center);
        internal override HasSlider Sliders => HasSlider.Horz | HasSlider.Vert;
        protected override byte TypeCode => (byte)ShapeType.Line;
        #endregion
    }
}
