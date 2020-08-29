﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using ModelGraph.Core;
using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Controls
{
    internal class Ellipes : Central
    {
        internal Ellipes(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = 0.30f;
            Radius2 = 0.20f;
            DXY = new List<(float dx, float dy)>() { (0, 0) };
        }

        #region PrivateConstructor  ===========================================
        private Ellipes(MShape shape)
        {
            CopyData(shape);
        }
        private Ellipes(MShape shape, Vector2 center)
        {
            CopyData(shape);
            Center = center;
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override MShape Clone() =>new Ellipes(this);
        internal override MShape Clone(Vector2 center) => new Ellipes(this, center);

        internal override void Draw(CanvasControl cc, CanvasDrawingSession ds, float scale, Vector2 center, float strokeWidth, Coloring coloring = Coloring.Normal)
        {
            var color = GetColor(coloring);
            var (cp, r1, r2) = GetCenterRadius(center, scale);

            if (FillStroke == Fill_Stroke.Filled)
                ds.FillEllipse(cp, r1, r2, color);
            else
                ds.DrawEllipse(cp, r1, r2, color, strokeWidth, StrokeStyle());
        }
        internal override void Draw(CanvasControl cc, CanvasDrawingSession ds, float scale, Vector2 center, FlipState flip)
        {
            var color = GetColor(Coloring.Normal);
            var (cp, r1, r2) = GetCenterRadius(flip, center, scale);

            if (FillStroke == Fill_Stroke.Filled)
                ds.FillEllipse(cp, r1, r2, color);
            else
                ds.DrawEllipse(cp, r1, r2, color, StrokeWidth, StrokeStyle());
        }
        internal override HasSlider Sliders => HasSlider.Vert | HasSlider.Horz;
        protected override byte TypeCode => (byte)ShapeType.Ellipse;
        #endregion
    }
}
