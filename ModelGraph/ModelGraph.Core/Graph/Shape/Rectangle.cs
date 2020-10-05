using System.Collections.Generic;
using System.Numerics;

namespace ModelGraph.Core
{
    internal class Rectangle : Central
    {
        protected override ShapeType ShapeType => ShapeType.Rectangle;
        internal Rectangle(bool deserializing = false)
        {
            if (deserializing) return; // properties to be loaded from serialized data

            Radius1 = Radius2 = 0.30f;
            DXY = new List<(float dx, float dy)>() { (0, 0) };
        }

        #region PrivateConstructor  ===========================================
        private Rectangle(Shape shape)
        {
            CopyData(shape);
        }
        private Rectangle(Shape shape, Vector2 center)
        {
            CopyData(shape);
            Center = center;
        }
        #endregion

        #region OverideAbstract  ==============================================
        internal override Shape Clone() =>new Rectangle(this);
        internal override Shape Clone(Vector2 center) => new Rectangle(this, center);

        internal override void AddDrawData(DrawData drawData, float size, float scale, Vector2 center, Coloring coloring = Coloring.Normal)
        {
            var min = center + (Center - Radius) * scale;
            var len = Radius * 2 * scale;

            drawData.AddShape(((new Vector2(min.X, min.Y), new Vector2(len.X, len.Y)), ShapeStrokeWidth(scale / size), ShapeColor(coloring)));
        }
        internal override void AddDrawData(DrawData drawData, float scale, Vector2 center, FlipState flip)
        {
            var (cp, r1, r2) = GetCenterRadius(center, scale);
            var radius = new Vector2(r1, r2);
            var min = cp - radius;
            var len = radius * 2;

            drawData.AddShape(((new Vector2(min.X, min.Y), new Vector2(len.X, len.Y)), ShapeStrokeWidth(), ShapeColor()));
        }
        internal override HasSlider Sliders => HasSlider.Horz | HasSlider.Vert;
        #endregion
    }
}
