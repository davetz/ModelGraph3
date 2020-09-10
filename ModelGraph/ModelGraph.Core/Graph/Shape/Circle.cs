﻿using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class Circle : Central
    {
        protected override ShapeType ShapeType => ShapeType.Circle;
        internal Circle(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = Radius2 = 0.25f;
            DXY = new List<(float dx, float dy)>() { (0, 0) };
        }

        #region PrivateConstructor  ===========================================
        private Circle(ShapeBase shape)
        {
            CopyData(shape);
        }
        private Circle(ShapeBase shape, Vector2 center)
        {
            CopyData(shape);
            Center = center;
        }
        #endregion

        #region RequiredMethods  ==============================================
        internal override ShapeBase Clone() =>new Circle(this);
        internal override ShapeBase Clone(Vector2 center) => new Circle(this, center);

        internal override void AddDrawData(DrawData drawData, float scale, Vector2 center, float strokeWidth, Coloring coloring = Coloring.Normal)
        {
            //var color = GetColor(coloring);
            var (cp, r1, _) = GetCenterRadius(center, scale);

            drawData.AddShape(((cp, new Vector2(r1, r1)), ShapeStrokeWidth, (255, 255, 255, 255)));

            //if (FillStroke == Fill_Stroke.Filled)
            //    ds.FillCircle( cp, r1, color);
            //else
            //    ds.DrawCircle(cp, r1, color, strokeWidth, StrokeStyle());
        }
        internal override void AddDrawData(DrawData drawData, float scale, Vector2 center, FlipState flip)
        {
            //var color = GetColor(Coloring.Normal);
            //var (cp, r1, r2) = GetCenterRadius(center, scale);

            //if (FillStroke == Fill_Stroke.Filled)
            //    ds.FillCircle(cp, r1, color);
            //else
            //    ds.DrawCircle(cp, r1, color, StrokeWidth, StrokeStyle());
        }
        protected override void Scale(Vector2 scale)
        {
            if (scale.X == 1)
                Radius1 = Radius2 = (Radius1 * scale.Y);
            else
                Radius1 = Radius2 = (Radius1 * scale.X);
        }
        internal override HasSlider Sliders => HasSlider.None;
        #endregion
    }
}