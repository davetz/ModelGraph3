using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class Ellipes : Central
    {
        protected override ShapeType ShapeType => ShapeType.Ellipse;

        internal Ellipes(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = 0.30f;
            Radius2 = 0.20f;
            DXY = new List<(float dx, float dy)>() { (0, 0) };
        }

        #region PrivateConstructor  ===========================================
        private Ellipes(Shape shape)
        {
            CopyData(shape);
        }
        private Ellipes(Shape shape, Vector2 center)
        {
            CopyData(shape);
            Center = center;
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override Shape Clone() =>new Ellipes(this);
        internal override Shape Clone(Vector2 center) => new Ellipes(this, center);

        internal override void AddDrawData(DrawData drawData, float size, float scale, Vector2 center, Coloring coloring = Coloring.Normal)
        {
            //var color = GetColor(coloring);
            var (_, _, _) = GetCenterRadius(center, scale);


            //if (FillStroke == Fill_Stroke.Filled)
            //    ds.FillEllipse(cp, r1, r2, color);
            //else
            //    ds.DrawEllipse(cp, r1, r2, color, strokeWidth, StrokeStyle());
        }
        internal override void AddDrawData(DrawData drawData, float scale, Vector2 center, FlipState flip)
        {
            //var color = GetColor(Coloring.Normal);
            var (_, _, _) = GetCenterRadius(flip, center, scale);

            //if (FillStroke == Fill_Stroke.Filled)
            //    ds.FillEllipse(cp, r1, r2, color);
            //else
            //    ds.DrawEllipse(cp, r1, r2, color, StrokeWidth, StrokeStyle());
        }
        protected override ShapeProperty PropertyFlags => ShapeProperty.Major | ShapeProperty.Minor;
        #endregion
    }
}
