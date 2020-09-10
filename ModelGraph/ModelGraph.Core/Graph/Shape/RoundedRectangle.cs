using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class RoundedRectangle : Central
    {
        protected override ShapeType ShapeType => ShapeType.RoundedRectangle;
        internal RoundedRectangle(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = 0.5f;
            Radius2 = 0.3f;
            DXY = new List<(float dx, float dy)>() { (0, 0) };
        }
        internal float Corner => 0.1f;

        #region PrivateConstructor  ===========================================
        internal RoundedRectangle(ShapeBase shape)
        {
            CopyData(shape);
        }

        internal RoundedRectangle(ShapeBase shape, Vector2 center)
        {
            CopyData(shape);
            Center = center;
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override ShapeBase Clone() =>new RoundedRectangle(this);
        internal override ShapeBase Clone(Vector2 center) => new RoundedRectangle(this, center);

        internal override void AddDrawData(DrawData drawData, float scale, Vector2 center, float strokeWidth, Coloring coloring = Coloring.Normal)
        {
            //var color = GetColor(coloring);
            //var min = center + (Center - Radius) * scale;
            //var len = Radius * 2 * scale;
            //var corner = Corner * scale;
            //if (FillStroke == Fill_Stroke.Filled)
            //    ds.FillRoundedRectangle( min.X, min.Y, len.X, len.Y, corner, corner, color);
            //else
            //    ds.DrawRoundedRectangle(min.X, min.Y, len.X, len.Y, corner, corner, color, strokeWidth, StrokeStyle());
        }
        internal override void AddDrawData(DrawData drawData, float scale, Vector2 center, FlipState flip)
        {
            //var color = GetColor(Coloring.Normal);
            //var (cp, r1, r2) = GetCenterRadius(center, scale);
            //var radius = new Vector2(r1, r2);
            //var min = cp - radius;
            //var len = radius * 2;
            //var corner = Corner * scale;

            //if (FillStroke == Fill_Stroke.Filled)
            //    ds.FillRoundedRectangle(min.X, min.Y, len.X, len.Y, corner, corner, color);
            //else
            //    ds.DrawRoundedRectangle(min.X, min.Y, len.X, len.Y, corner, corner, color, StrokeWidth, StrokeStyle());
        }
        internal override HasSlider Sliders => HasSlider.Horz | HasSlider.Vert;
        #endregion
    }
}
